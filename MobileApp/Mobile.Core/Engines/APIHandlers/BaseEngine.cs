using Mobile.Core.Worker;

namespace Mobile.Core.Engines.APIHandlers
{
    class BaseEngine
    {
        private readonly HttpWorker _httpWorker;

        public BaseEngine(HttpWorker httpWorker)
        {
            _httpWorker = httpWorker;
        }
    }
}