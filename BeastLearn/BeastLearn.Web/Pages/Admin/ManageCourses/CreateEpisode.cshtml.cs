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

namespace BeastLearn.Web.Pages.Admin.ManageCourses
{
    [PermissionChecker(21)]
    public class CreateEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }

        public void OnGet(int id)
        {
            CourseEpisode = new CourseEpisode();
            CourseEpisode.CourseId = id;
        }

        public IActionResult OnPost(IFormFile EpisodeFile)
        {
            if (!ModelState.IsValid && EpisodeFile == null)
                return Page();

            if (_courseService.CheckIsExistFile(EpisodeFile.FileName))
            {
                ViewData["IsExistFile"] = true;
                return Page();
            }
            _courseService.AddCourseEpisode(CourseEpisode, EpisodeFile);

            return Redirect("/Admin/ManageCourses/IndexEpisode/" + CourseEpisode.CourseId);
        }
    }
}