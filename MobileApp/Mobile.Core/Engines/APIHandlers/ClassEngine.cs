using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    internal class LessonEngine : ILessonHandler
    {
        Task<List<Lesson>> ILessonHandler.GetLessons()
        {
            throw new NotImplementedException();
        }

        Task<List<Lesson>> ILessonHandler.GetLessons(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
