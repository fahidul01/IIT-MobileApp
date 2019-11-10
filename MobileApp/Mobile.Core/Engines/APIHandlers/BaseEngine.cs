using Mobile.Core.Worker;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    public class BaseEngine
    {
        protected readonly HttpWorker _httpWorker;
        protected readonly string controller;

        public BaseEngine(HttpWorker httpWorker)
        {
            _httpWorker = httpWorker;
        }

        public BaseEngine(HttpWorker httpWorker, string Controller)
        {
            _httpWorker = httpWorker;
            controller = Controller;
        }

        protected async Task<string> SendStringRequest(HttpMethod httpMethod, object data, [CallerMemberName]string member = "")
        {
            var jsonData = JsonConvert.SerializeObject(data);
            using var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonData));
            return await _httpWorker.SendRequest<string>(httpMethod, controller + "/" + member, byteContent);
        }
        protected async Task<bool> SendBoolRequest(HttpMethod httpMethod, object data, [CallerMemberName]string member = "")
        {
            var jsonData = await FormHelper.GetPair(data);
            using var content = new FormUrlEncodedContent(jsonData);
            var res = await _httpWorker.SendRequest<string>(httpMethod, controller + "/" + member, content);
            bool.TryParse(res, out bool response);
            return response;
        }

        protected async Task<T> SendRequest<T>(HttpMethod httpMethod, object data, [CallerMemberName]string member = "")
        {
            var jsonData = await FormHelper.GetPair(data);
            using var content = new FormUrlEncodedContent(jsonData);
            return await _httpWorker.SendRequest<T>(httpMethod, controller + "/" + member, content);
        }
    }
}