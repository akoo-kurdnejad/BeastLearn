using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Domain.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageCourses
{
    [PermissionChecker(24)]
    public class ListOrdersModel : PageModel
    {
        private IOrderService _orderService;

        public ListOrdersModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public List<Order> Orders { get; set; }
        public void OnGet(int id)
        {
            Orders = _orderService.GetUserOrder(id);
        }
    }
}