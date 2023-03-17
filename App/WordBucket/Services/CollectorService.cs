using ReactiveUI;
using System.ComponentModel.Design.Serialization;
using System.Net;

namespace WordBucket.Services
{
    public class CollectorService : ServiceBase
    {
        private CollectorService()
        { }

        public static CollectorService Instance => new();

        private HttpListener _httpListener = new();

        public override async Task InitializeAsync()
        {
            InitializeHttpListener();
            await base.InitializeAsync();
        }

        public void InitializeHttpListener()
        {
            string listenAddress = $"http://localhost:{AppConfig.HttpListenerPort}/";

            _httpListener.Prefixes.Add(listenAddress);

            Thread thread = new Thread(() =>
            {
                _httpListener.Start();

                while (true)
                {
                    var context = _httpListener.GetContext();
                    var request = context.Request;
                    var response = context.Response;

                    var responseString = "Hello, world!";
                    var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                    response.ContentLength64 = responseBuffer.Length;
                    response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                    response.OutputStream.Close();
                }
            });
            thread.Start();
        }
    }
}
