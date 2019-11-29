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

        public BaseEngine(HttpWorker httpWorker, string Controller)
        {
            _httpWorker = httpWorker;
            controller = Controller;
        }

        protected async Task<T> SendRequest<T>(HttpMethod httpMethod, object data, [CallerMemberName]string member = "")
        {
            var jsonData = await FormHelper.GetPair(data);
            using var content = new FormUrlEncodedContent(jsonData);
            return await _httpWorker.SendRequest<T>(httpMethod, controller + "/" + member, content);
        }
    }
}