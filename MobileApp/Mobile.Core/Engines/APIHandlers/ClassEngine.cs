using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    class ClassEngine : IClassHandler
    {
        public Task<List<Class>> GetClasses()
        {
            throw new NotImplementedException();
        }

        public Task<List<Class>> GetClasses(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
