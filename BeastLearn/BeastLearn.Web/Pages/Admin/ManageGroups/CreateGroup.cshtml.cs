using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Domain.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageGroups
{
    [PermissionChecker(13)]
    public class CreateGroupModel : PageModel
    {
        private ICourseService _courseService;

        public CreateGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseGroup CourseGroup { get; set; }

        public void OnGet(int? id)
        {
            CourseGroup = new CourseGroup()
            {
                ParentId = id
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.AddCourseGroup(CourseGroup);
            return RedirectToPage("Index");
        }
    }
}