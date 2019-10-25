﻿using CoreEngine.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Worker
{
    public class HttpWorker
    {
        private readonly HttpClient _httpClient;
        public HttpWorker(string baseurl)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseurl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LoggedIn(string sessionKey)
        {
            var data = Encoding.UTF8.GetBytes((sessionKey + ":" + sessionKey).ToCharArray());
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(data));
        }

        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> SendRequest(HttpMethod method, string requestPath, Dictionary<string, string> parameters)
        {
            try
            {
                HttpResponseMessage msg = null;
                var log = string.Empty;
                if (method == HttpMethod.Get)
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        requestPath += "?";
                        using (var httpContent = new FormUrlEncodedContent(parameters))
                        {
                            var data = await httpContent.ReadAsStringAsync();
                            requestPath += data;
                        }
                    }
                    msg = await _httpClient.GetAsync(requestPath);
                }
                else if (method == HttpMethod.Post)
                {
                    parameters = parameters ?? new Dictionary<string, string>();
                    using (var httpContent = new FormUrlEncodedContent(parameters))
                    {
                        msg = await _httpClient.PostAsync(requestPath, httpContent);
                        log = await msg.Content.ReadAsStringAsync();
                    }
                }
                else if (method == HttpMethod.Put)
                {
                    parameters = parameters ?? new Dictionary<string, string>();
                    using (var httpContent = new FormUrlEncodedContent(parameters))
                    {
                        msg = await _httpClient.PutAsync(requestPath, httpContent);
                        log = await msg.Content.ReadAsStringAsync();
                    }
                }

                if (msg == null)
                    throw new Exception("Invalid Method");
                else if (msg.StatusCode != HttpStatusCode.OK)
                    throw new Exception(msg.ReasonPhrase + " " + log);
                else
                {
                    var data = await msg.Content.ReadAsStringAsync();
                    return data;
                }
            }
            catch (Exception e)
            {
                LogEngine.Error(e);
                return string.Empty;
            }
        }


        public async Task<T> SendRequest<T>(HttpMethod method, string requestPath, Dictionary<string, string> parameters)
        {
            var res = await SendRequest(method, requestPath, parameters);
            if (string.IsNullOrEmpty(res)) return default;
            else
            {
                try
                {
                    return await Task.Run(() => JsonConvert.DeserializeObject<T>(res));
                }
                catch (Exception ex)
                {
                    LogEngine.Error(ex);
                    return default;
                }
            }
        }
    }
}
