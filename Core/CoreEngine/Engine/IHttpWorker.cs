using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreEngine.Engine
{
    public interface IHttpWorker
    {
        Task<T> GetJsonAsync<T>(string requestUri);
        Task<T> PostJsonAsync<T>(string requestUri, object content);
    }
}
