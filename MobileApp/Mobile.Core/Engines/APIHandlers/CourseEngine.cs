﻿using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Mobile.Core.Worker;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    public class CourseEngine : BaseEngine, ICourseHandler
    {
        private const string controllerName = "Courses";
        List<Semester> Semesters;
        public CourseEngine(HttpWorker httpWorker) : base(httpWorker, controllerName)
        {
        }

        public Task<ActionResponse> AddMaterial(int courseId, DBFile dbFile)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { courseId, dbFile });
        }


        public Task<ActionResponse> DeleteCourse(Course course)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, course);
        }

        public Task<ActionResponse> DeleteCouseMaterial(DBFile obj)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, obj);
        }

        public Task<ActionResponse> DeleteLesson(int lessonId)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { lessonId });
        }

        public Task<Course> GetCourse(int courseId)
        {
            return SendRequest<Course>(HttpMethod.Post, new { courseId });
        }

        public Task<List<Course>> GetCourses()
        {
            return SendRequest<List<Course>>(HttpMethod.Get, null);
        }

        public Task<List<Course>> GetCourses(int batchId)
        {
            return SendRequest<List<Course>>(HttpMethod.Post, new { batchId });
        }

        public Task<ActionResponse> Update(Lesson lesson)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, lesson);
        }

        public Task<ActionResponse> UpdateCourse(Course course)
        {
            Semesters = null;
            return SendRequest<ActionResponse>(HttpMethod.Post, course);
        }

        public async Task<List<Semester>> GetCurrentSemester()
        {
            if (Semesters == null)
            {
                Semesters = await SendRequest<List<Semester>>(HttpMethod.Get, null);
            }
            return Semesters;
        }

        public Task<ActionResponse> CreateCourse(Course course, int semesterId)
        {
            Semesters = null;
            return SendRequest<ActionResponse>(HttpMethod.Post, new { course, semesterId });
        }
    }
}
