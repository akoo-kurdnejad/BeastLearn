﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageUsers
{
    [PermissionChecker(6)]
    public class ListDeleteUserModel : PageModel
    {
        private IUserService _userService;

        public ListDeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public UsersForAdminViewModel UsersForAdmin { get; set; }

        public void OnGet(int pageId = 1, string filterEmail = "")
        {
            UsersForAdmin = _userService.GetUsers(pageId, filterEmail);
        }
    }
}