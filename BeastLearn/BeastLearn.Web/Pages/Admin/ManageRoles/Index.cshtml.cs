using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageRoles
{
    [PermissionChecker(7)]
    public class IndexModel : PageModel
    {
        private IPermissionService _permissionService;

        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService; 
        }

        public List<Role> Roles { get; set; }
        public void OnGet()
        {
            Roles = _permissionService.GetRoles().ToList();
        }
    }
}