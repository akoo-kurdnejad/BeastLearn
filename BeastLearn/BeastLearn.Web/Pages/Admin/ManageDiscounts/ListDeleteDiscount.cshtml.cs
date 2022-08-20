using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Domain.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageDiscounts
{
    [PermissionChecker(33)]
    public class ListDeleteDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public ListDeleteDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }


        public List<Discount> Discounts { get; set; }
        public void OnGet()
        {
            Discounts = _orderService.GetListDeleteCode();
        }
    }
    
}