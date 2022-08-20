using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeastLearn.Web.Areas.UserPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Area("UserPanel")]
    [Authorize]
    public class MyOrderController : Controller
    {
        private IOrderService _orderService;
        private IWalletService _walletService;

        public MyOrderController(IOrderService orderService, IWalletService walletService)
        {
            _orderService = orderService;
            _walletService = walletService;
        }
  
        public IActionResult Index()
        {
            var order = _orderService.GetUserOrder(User.Identity.Name);
            return View(order);
        }

        public IActionResult ShowOrder(int id , bool finaly = false , string type = "")
        {
            ViewBag.Wallet = _walletService.BalanceWalletUser(User.Identity.Name);
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);
            ViewBag.typeDiscount = type;

            if (order == null)
            {
                return NotFound();
            }
            ViewBag.finaly = finaly;
            return View(order);
        }

        public IActionResult FinalyOrder(int id)
        {
            if (_orderService.FinalyOrder(User.Identity.Name, id))
            {
                return Redirect("/UserPanel/MyOrder/ShowOrder/" + id + "?finaly=true");
            }
            return BadRequest();
        }

        public IActionResult UseDiscount(int orderId, string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);

            return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId + "?type=" + type.ToString());
        }
    }
}