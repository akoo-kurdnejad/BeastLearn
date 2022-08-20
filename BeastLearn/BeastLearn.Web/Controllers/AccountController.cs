using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeastLearn.Application.Convertors;
using BeastLearn.Application.Generators;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Users;
using BeastLearn.Domain.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BeastLearn.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        private IMailSender _sender;

        public AccountController(IUserService userService, IViewRenderService viewRender, IMailSender sender)
        {
            _userService = userService;
            _viewRender = viewRender;
            _sender = sender;
        }

        #region RegisterUser

        
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if(_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName" , "نام کاربری وارد شده معتبر نمیباشد.");
                return View(register);
            }

            if (_userService.IsExistEmail(register.Email))
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده معتبر نمیباشد.");
                return View(register);
            }

            User user = new User()
            {
                ActiveCode = NameGenerator.GenerateUniqCode(),
                IsActive = false,
                UserName = register.UserName,
                RegisterDate = DateTime.Now,
                Email = FixedText.FixedEmail(register.Email),
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                UserAvatar = "Defult.jpg",
                Gender = "تعرف نشده",
                IsDelete = false
            };
            _userService.AddUser(user);
            #region SendEmail

            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            _sender.Send(user.Email,"فعالسازی " , body);

            #endregion
          
            return View("SuccessRegister", user);
        }

        #endregion

        #region LoginUser

        [Route("Login")]
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel login )
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login.Email, login.Password);

            if (user != null)
            {
                if (user.IsActive)
                {
                   var claims = new List<Claim>()
                   {
                       new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                       new Claim(ClaimTypes.Name,user.UserName)
                   };

                   var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                   var principal = new ClaimsPrincipal(identity);
                   var properties = new AuthenticationProperties
                   {
                       IsPersistent = login.RememberMe
                   };

                   HttpContext.SignInAsync(principal, properties);

                  
                       return Redirect("/");
                   

                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");
                }
            }

            ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);

        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }


        #endregion


        #region ActiveAccount

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }

        #endregion

        #region ForgotPassword

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            var user = _userService.GetUserByEmail(forgot.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgot);
            }

            string body = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            _sender.Send(user.Email , "بازیابی کلمه عبور" , body);
            ViewBag.IsSuccess = true;
            return View();
        }

        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var user = _userService.GetUserByActiveCode(reset.ActiveCode);
            if (user == null)
                return NotFound();

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hashNewPassword;
            _userService.UpdateUser(user);

            return Redirect("/Login");
        }

        #endregion
    }
}