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
    [PermissionChecker(32)]
    public class DeleteDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public DeleteDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public Discount Discount { get; set; }
        public void OnGet(int id)
        {
            Discount = _orderService.GetDiscountById(id);
            ViewData["codeId"] = id;
        }

        public IActionResult OnPost(int codeId)
        {
            _orderService.DeleteDiscount(codeId);
            return RedirectToPage("Index");
        }
    }
}