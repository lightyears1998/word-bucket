using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordBucket.Services
{
    public class CollectorService : ServiceBase
    {
        public event EventHandler<HttpRequestPayload>? HttpMessageReceived;

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

                        var requestBufferLength = request.ContentLength64;
                        var requestBuffer = new byte[requestBufferLength];
                        request.InputStream.Read(requestBuffer);
                        var inputString = System.Text.Encoding.UTF8.GetString(requestBuffer);

                        var message = JsonSerializer.Deserialize<HttpRequestPayload>(inputString)!;
                        HttpMessageReceived?.Invoke(this, message);

                        var responseString = "<html><body><p>OK!</p></body></html>";
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

        public record class HttpRequestPayload
        {
            [JsonPropertyName("title")]
            public string Title { set; get; } = string.Empty;

            [JsonPropertyName("text")]
            public string Text { set; get; } = string.Empty;

            [JsonPropertyName("uri")]
            public string Uri { set; get; } = string.Empty;
        }
    }
}
