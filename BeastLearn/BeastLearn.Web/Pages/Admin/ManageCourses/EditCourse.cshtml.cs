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
    [PermissionChecker(19)]
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;

        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }
        public void OnGet(int id)
        {
            Course = _courseService.GetCourseById(id);

            var group = _courseService.GetGroupForAddCourse();
            ViewData["Groups"] = new SelectList(group, "Value", "Text" , Course.GroupId);

            List<SelectListItem> subgroups = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "لطفا انتخاب کنید" , Value = ""}
            };

            subgroups.AddRange(_courseService.GetSubGroupForAddCourse(Course.GroupId));
            var subGroup = _courseService.GetSubGroupForAddCourse(int.Parse(group.First().Value));
            string selectedSubGroup = null;
            if (Course.SubGroupId != null)
            {
                selectedSubGroup = Course.SubGroupId.ToString();
            }
            ViewData["SubGroups"] = new SelectList(subgroups, "Value", "Text", selectedSubGroup);

            var teacher = _courseService.GetTeachaer();
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text" , Course.TeacherId);

            var levels = _courseService.GetLevel();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text" , Course.LevelId);

            var statues = _courseService.GetStatus();
            ViewData["Statues"] = new SelectList(statues, "Value", "Text" , Course.StatusId);
        }

        public IActionResult OnPost(IFormFile ImgUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.EditCourse(Course , ImgUp , demoUp);

            return RedirectToPage("Index");
        }
    }
}