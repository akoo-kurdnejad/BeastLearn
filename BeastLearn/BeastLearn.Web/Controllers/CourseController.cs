using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Domain.Models.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeastLearn.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;

        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }

        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all", int maxPrice = 0, int minPrice = 0, int take = 0, string ordeByType = "date", List<int> selectedGroups = null)
        {
            ViewBag.Groups = _courseService.GetCourseGroups();
            ViewBag.SelectedGroup = selectedGroups;
            ViewBag.GetType = getType;
            ViewBag.pageId = pageId;

            return View(_courseService.GetCourse
                (pageId , filter , getType , maxPrice , minPrice , 9 , ordeByType , selectedGroups));
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id)
        {
            var course = _courseService.GetSingleCourse(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            int orderId = _orderService.AddOrder(User.Identity.Name, id);

            return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId);
        }

        [Authorize]
        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseService.GetCourseEpisodeById(episodeId);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFile", episode.EpisodeFileName);
            string fileName = episode.EpisodeFileName;

            if (episode.IsFree)
            {
               
                    byte[] file = System.IO.File.ReadAllBytes(filePath);
                    return File(file, "application/force-download", fileName);
                
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_orderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filePath);
                    return File(file, "application/force-download", fileName);
                }
            }

            ViewBag.AccessDenaid = true;
            return Forbid();

        }

        
        [HttpGet]
        public IActionResult CreateComment(CourseComment comment , int?id)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.IsAdminRead = false;
            comment.UserId = _userService.GetUserIdByUserName(User.Identity.Name);

            _courseService.AddComment(comment);

            return View("ShowComment", _courseService.GetAllComment(comment.CourseId));
        }

        public IActionResult ShowComment(int id)
        {
            return View(_courseService.GetAllComment(id));
        }

        public void DeleteComment(int id)
        {
            _courseService.DeleteComment(id);
        }

        public IActionResult CourseVote(int id)
        {
            if (!_courseService.IsFree(id))
            {
                if (!_orderService.IsUserInCourse(User.Identity.Name, id))
                {
                    ViewBag.NotAccess = true;
                }
            }
            return PartialView(_courseService.GetCourseVote(id));
        }

        public IActionResult AddCourseVote(int id, bool vote)
        {
            var userId = _userService.GetUserIdByUserName(User.Identity.Name);
            _courseService.AddCourseVote(userId , id ,vote);
            return PartialView("CourseVote" , _courseService.GetCourseVote(id));
        }
    }
}