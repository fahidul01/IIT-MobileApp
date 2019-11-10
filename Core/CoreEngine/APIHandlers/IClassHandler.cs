using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ILessonHandler
    {
        Task<List<Lesson>> GetLessons();
        Task<List<Lesson>> GetLessons(DateTime dateTime);
    }
}
