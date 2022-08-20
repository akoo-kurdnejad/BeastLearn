using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.Services;
using BeastLearn.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageRoles
{
    [PermissionChecker(8)]
    public class CreateRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public CreateRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService; 
        }

        [BindProperty]
        public Role Roles { get; set; }
        public void OnGet()
        {
            ViewData["Permission"] = _permissionService.GetAllPermission();
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();

            Roles.IsDelete = false;

            int roleId = _permissionService.AddRole(Roles);
            _permissionService.AddPermissionRole(SelectedPermission , roleId);

            return RedirectToPage("Index");


        }
    }
}