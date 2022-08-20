using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Domain.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeastLearn.Web.Pages.Admin.ManageDiscounts
{
    [PermissionChecker(31)]
    public class EditDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public EditDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public Discount Discount { get; set; }
        public void OnGet(int id)
        {
            Discount = _orderService.GetDiscountById(id);
        }

        public IActionResult OnPost(string startDate = "", string endDate = "")
        {
            if (startDate != "")
            {
                string[] start = startDate.Split('/');
                Discount.StartDate = new DateTime(int.Parse(start[0]),
                    int.Parse(start[1]),
                    int.Parse(start[2]),
                    new PersianCalendar()
                );
            }

            if (endDate != "")
            {
                string[] end = endDate.Split('/');
                Discount.EndDate = new DateTime(int.Parse(end[0]),
                    int.Parse(end[1]),
                    int.Parse(end[2]),
                    new PersianCalendar()
                );
            }

            _orderService.UpdateDiscount(Discount);

            return RedirectToPage("Index");
        }
    }
}