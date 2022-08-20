using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Courses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageCourses
{
    [PermissionChecker(20)]
    public class DeleteCourseModel : PageModel
    {
        private ICourseService _courseService;

        public DeleteCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public ShowCourseForDeleteViewModel ShowCourse { get; set; }
        public void OnGet(int id)
        {
            ViewData["CourseId"] = id;
            ShowCourse = _courseService.GetCourseForDelete(id);
        }

        public IActionResult OnPost(int CourseId)
        {
            _courseService.DeleteCourse(CourseId);

            return RedirectToPage("Index");
        }
    }
}