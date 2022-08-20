using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageCourses
{
    [PermissionChecker(28)]
    public class ListDeleteCourseModel : PageModel
    {
        private ICourseService _courseService;

        public ListDeleteCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public CourseForAdminViewModel CourseForAdmin { get; set; }
        public void OnGet(string filterTitle = "", int pageId = 1)
        {
            CourseForAdmin = _courseService.GetListDeleteCourse(filterTitle, pageId);
        }
    }
}