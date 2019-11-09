using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IClassHandler
    {
        Task<List<Lesson>> GetClasses();
        Task<List<Lesson>> GetClasses(DateTime dateTime);
    }
}
