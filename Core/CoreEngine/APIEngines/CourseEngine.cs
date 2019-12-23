﻿using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    class CourseEngine : BaseApiEngine, ICourseHandler
    {
        private const string controllerName = "Courses";
        List<Semester> Semesters;
        public CourseEngine(HttpWorker httpWorker) : base(httpWorker, controllerName)
        {
        }

        public Task<ActionResponse> AddMaterial(int courseId, List<DBFile> dbFile, List<IFormFile> formFiles = null)
        {
            return SendMultiPartRequest<ActionResponse>(new { courseId }, dbFile);
        }


        public Task<ActionResponse> DeleteCourse(Course course)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, course);
        }

        public Task<ActionResponse> DeleteCouseMaterial(DBFile dBFile)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, dBFile);
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

        public Task<List<Course>> GetBatchCourses(int batchId)
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

        public Task<ActionResponse> CreateCourse(int semesterId, Course course, List<DBFile> dBFiles, List<IFormFile> formFiles = null)
        {
            Semesters = null;
            return SendRequest<ActionResponse>(HttpMethod.Post, new { course, semesterId });
        }

        public Task<ActionResponse> CreateAdminCourse(int semesterId, Course course, List<DBFile> dBFiles, List<IFormFile> formFiles = null)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { course, semesterId });
        }

        public Task<ActionResponse> UploadCourseResult(int courseId, DBFile dBFile, IFormFile formFile)
        {
            return SendMultiPartRequest<ActionResponse>(new { courseId }, dBFile);
        }

        public Task<List<SemesterData>> GetResult()
        {
            return SendRequest<List<SemesterData>>(HttpMethod.Get, null);
        }

        public Task<List<Course>> SearchCourse(string search)
        {
            return SendRequest<List<Course>>(HttpMethod.Get, new { search });
        }

        public Task<List<SemesterData>> GetStudentResult(string userId)
        {
            return SendRequest<List<SemesterData>>(HttpMethod.Get, new { userId });
        }
    }
}
