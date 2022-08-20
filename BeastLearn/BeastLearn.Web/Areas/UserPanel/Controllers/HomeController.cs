using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeastLearn.Web.Areas.UserPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public HomeController(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }
        public IActionResult Index()
        {
            return View(_userService.GetInformationUser(User.Identity.Name));
        }

        #region EditProfile


        [Route("UserPane/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }

        [HttpPost]
        [Route("UserPane/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View(profile);
            }

            _userService.EditProfile(User.Identity.Name , profile);
            ViewBag.IsSuccess = true;

            return Redirect("/UserPanel");
        }
        #endregion

        #region ChangePassword

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            if (!_userService.CompareOldPassword(User.Identity.Name, change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمیباشد");
                return View(change);
            }

            _userService.ChangeUserPassword(User.Identity.Name , change.Password);
            ViewBag.IsSuccess = true;
            return View();
        }

        #endregion

        [Route("UserPanel/CourseTeacher")]
        public IActionResult ListCoursesTeacher()
        {
            var courseTeacher = _courseService.GetCourseTeacher(User.Identity.Name);

            if (courseTeacher == null)
            {
                ViewBag.IsTrue = true;
            }
            return View(courseTeacher);
        }

    }
}