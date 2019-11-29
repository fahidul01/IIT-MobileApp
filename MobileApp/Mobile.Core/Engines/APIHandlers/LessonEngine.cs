using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Mobile.Core.Worker;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    internal class LessonEngine : BaseEngine, ILessonHandler
    {
        private const string Controller = "Lessons";
        public LessonEngine(HttpWorker httpWorker) : base(httpWorker, Controller)
        {
        }

        public Task<List<Lesson>> GetLessons()
        {
            return SendRequest<List<Lesson>>(HttpMethod.Get, null);
        }

        public Task<List<Lesson>> GetLessons(DateTime dateTime)
        {
            return SendRequest<List<Lesson>>(HttpMethod.Post, dateTime);
        }
    }
}
