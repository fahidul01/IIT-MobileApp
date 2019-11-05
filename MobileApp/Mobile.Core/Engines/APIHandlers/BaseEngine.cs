using Mobile.Core.Worker;

namespace Mobile.Core.Engines.APIHandlers
{
    internal class BaseEngine
    {
        private readonly HttpWorker _httpWorker;

        public BaseEngine(HttpWorker httpWorker)
        {
            _httpWorker = httpWorker;
        }
    }
}