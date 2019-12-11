using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Web.Areas.Admin.ViewModels;
using Web.Controllers;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : BaseController
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService course)
        {
            _courseService = course;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseAsync(id);
            return View(course);
        }

        public async Task<IActionResult> Create(CreateCoursePopupModel createCoursePopupModel)
        {
            if (ModelState.IsValid)
            {
                var course = new Course()
                {
                    CourseCredit = createCoursePopupModel.CourseCredit,
                    CourseId = createCoursePopupModel.CourseId,
                    CourseName = createCoursePopupModel.CourseName
                };
                var newCourse = await _courseService.AddCourse(course, createCoursePopupModel.SemesterId, createCoursePopupModel.BatchId);
                if (newCourse != null)
                {
                    var res = await _courseService.GetSemesterAsync(createCoursePopupModel.SemesterId);
                    return PartialView("_Courses", res.Courses);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> UploadResult(int courseId, IFormFile file)
        {
            if (courseId > 0 && file != null && file.Length > 0)
            {
                var filePath = Path.GetTempFileName();

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (file.FileName.EndsWith("csv"))
                    {
                        var res = await _courseService.UploadResult(courseId, filePath);
                        if (res.Actionstatus)
                            return PartialView("_Result", await _courseService.GetResult(courseId));
                        else
                        {
                            Failed(res.Message);
                            return PartialView("_Result", null);
                        }
                    }
                    else
                    {
                        Failed("Invalid File format");
                        return PartialView("_Result", null);
                    }
                }
                catch (Exception ex)
                {
                    Failed(ex.Message);
                    return PartialView("_Result", null);
                }
            }
            else
            {
                Failed("Invalid File format");
                return PartialView("_Students", new List<User>());
            }
        }
    }
}