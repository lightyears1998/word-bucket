using System.Diagnostics;
using System.Net;

namespace WordBucket.Services
{
    public class CollectorService : ServiceBase
    {
        private CollectorService()
        { }

        private static readonly CollectorService _instance = new();

        public static CollectorService Instance => _instance;

        private HttpListener? _httpListener;

        private Thread? _listenerThread;

        private CancellationTokenSource? _tokenSource;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            InitializeHttpListener();
        }

        public void InitializeHttpListener()
        {
            string listenAddress = $"http://localhost:{AppConfig.HttpListenerPort}/";

            _httpListener = new();
            _httpListener.Prefixes.Add(listenAddress);
            _tokenSource = new CancellationTokenSource();

            _listenerThread = new(() =>
            {
                _httpListener.Start();

                while (!_tokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        var context = _httpListener.GetContext();
                        var request = context.Request;
                        var response = context.Response;

                        var responseString = "OK.";
                        var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                        response.ContentLength64 = responseBuffer.Length;
                        response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                        response.OutputStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"{nameof(CollectorService)} {ex}");
                    }
                }
            });

            _listenerThread.Start();
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
            _httpListener?.Stop();
            _listenerThread?.Join();

            _tokenSource = null;
            _httpListener = null;
            _listenerThread = null;
        }
    }
}
