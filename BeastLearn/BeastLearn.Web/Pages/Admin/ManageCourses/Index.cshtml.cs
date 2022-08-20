using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageCourse
{
    [PermissionChecker(17)]
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public CourseForAdminViewModel CourseForAdmin { get; set; }
        public void OnGet(string filterTitle = "" , int pageId = 1)
        {
            CourseForAdmin = _courseService.GetCourse(filterTitle, pageId);
           
        }
    }
}