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
    [PermissionChecker(24)]
    public class ListUserBuyCourseModel : PageModel
    {
        private ICourseService _courseService;

        public ListUserBuyCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public Tuple<List<UserCourseViewModel>, int> UserCourse { get; set; }

        public void OnGet(int id , int pageId =1 , int take = 40 , string filterEmail ="")
        {
            ViewData["UserId"] = id;
            UserCourse = _courseService.GetUserBuyCourse(id, pageId,take, filterEmail);
        }
    }
}