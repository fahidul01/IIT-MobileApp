using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreEngine.Engine
{
    public interface ICourseDataService
    {
        Task<List<Course>> GetCourses(int batchId, int semesterId);
    }
}
