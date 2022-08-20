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
    [PermissionChecker(22)]
    public class EditEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public EditEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id)
        {
            CourseEpisode = _courseService.GetCourseEpisodeById(id);
        }

        public IActionResult OnPost(IFormFile EpisodeFile)
        {
            if (!ModelState.IsValid)
                return Page();

            if (EpisodeFile != null)
            {
                if (_courseService.CheckIsExistFile(EpisodeFile.FileName))
                {
                    ViewData["IsExistFile"] = true;
                    return Page();
                }
            }
            _courseService.EditCourseEpisode(CourseEpisode , EpisodeFile);

            return Redirect("/Admin/ManageCourses/IndexEpisode/" + CourseEpisode.CourseId);
        }
    }
}