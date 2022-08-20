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
    public class ListBuyCourseModel : PageModel
    {
        private ICourseService _courseService;

        public ListBuyCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public Tuple<List<ListBuyCourseViewModel>, int> Course { get; set; }
        public void OnGet(int pageId = 1 , int take = 20)
        {
            Course = _courseService.GetListBuyCourse(pageId, take);
        }
    }
}