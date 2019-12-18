using CoreEngine.Engine;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace IIT.Client.Services
{
    public class HttpService : IHttpWorker
    {
        private HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<T> GetJsonAsync<T>(string requestUri)
        {
            return _httpClient.GetJsonAsync<T>(requestUri);
        }

        public Task<T> PostJsonAsync<T>(string requestUri, object content)
        {
            return _httpClient.PostJsonAsync<T>(requestUri, content);
        }
    }
}
