using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Domain.Models.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastLearn.Web.Pages.Admin.ManageCourses
{
    [PermissionChecker(18)]
    public class CreateCourseModel : PageModel
    {
        private ICourseService _courseService;

        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }
        public void OnGet()
        {
            var group = _courseService.GetGroupForAddCourse();
            ViewData["Groups"] = new SelectList(group , "Value", "Text");

            var subGroup = _courseService.GetSubGroupForAddCourse(int.Parse(group.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroup , "Value" , "Text");

            var teacher = _courseService.GetTeachaer();
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text");

            var levels = _courseService.GetLevel();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");

            var statues = _courseService.GetStatus();
            ViewData["Statues"] = new SelectList(statues, "Value", "Text");
        }

        public IActionResult OnPost(IFormFile ImgUp , IFormFile demoUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.AddCourse(Course, ImgUp, demoUp);

            return RedirectToPage("Index");
        }
    }
}