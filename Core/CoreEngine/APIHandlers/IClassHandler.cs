using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IClassHandler
    {
        Task<List<Class>> GetClasses();
        Task<List<Class>> GetClasses(DateTime dateTime);
    }
}
