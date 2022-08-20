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
    public class WalletController : Controller
    {
        private IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService; 
        }

        [Route("UserPanel/WalletUser")]
        public IActionResult Index()
        {
            ViewBag.ListWallet = _walletService.GetWalletUser(User.Identity.Name);
            ViewBag.Wallet = _walletService.BalanceWalletUser(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/ChargWallet")]
        public IActionResult ChargeWallet()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChargWallet")]
        public IActionResult ChargeWallet(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListWallet = _walletService.GetWalletUser(User.Identity.Name);
                return View(charge);
            }

            int walletId = _walletService.ChargeWallet(User.Identity.Name, "شارژ حساب ", charge.Amount);


            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(charge.Amount);

            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44315/OnlinePayment/" + walletId, "akoo.kurdnejad@gmail.com", "09363062331");

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion


            return null;
        }
    }
}