using CoreEngine.Model.DBModel;
using Mobile.Core.Worker;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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

        protected async Task<T> SendMultiPartRequest<T>(object data, List<DBFile> dBFiles,[CallerMemberName]string member = "")
        {
            using var requestContent = new MultipartFormDataContent();
            var jsonData = await FormHelper.GetPair(data);
            foreach(var item in jsonData)
            {
                requestContent.Add(new StringContent(item.Value),item.Key);
            }

            foreach(var dbFile in dBFiles)
            {
                var streamContent = new StreamContent(File.OpenRead(dbFile.FilePath));
                var fName = Path.GetFileName(dbFile.FilePath);
                requestContent.Add(streamContent, "formfiles", fName);
            }
            return await _httpWorker.SendRequest<T>(HttpMethod.Post, controller + "/" + member, requestContent);
        }
    }
}