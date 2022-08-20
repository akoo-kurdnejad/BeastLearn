using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeastLearn.Web.ViewComponents
{
    public class CourseGroupComponentViewComponent : ViewComponent
    {
        private ICourseService _courseService;

        public CourseGroupComponentViewComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("CourseGroup", _courseService.GetCourseGroups()));

        }
    }
}