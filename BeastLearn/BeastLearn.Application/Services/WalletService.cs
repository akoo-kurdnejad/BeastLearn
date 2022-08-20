using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.ViewModels.Users;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Wallet;

namespace BeastLearn.Application.Services
{
    public class WalletService:IWalletService
    {
        private IWalletRepository _walletRepository;
     

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
          
        }
        public List<WalletViewModel> GetWalletUser(string userName)
        {
            int userId = GetUserIdByUserName(userName);
            var wallet = _walletRepository.GetWallet();

            return wallet.Where(w => w.UserId == userId && w.IsPay)
                .Select(w => new WalletViewModel()
                {
                    Description = w.Description,
                    Amount = w.Amount,
                    DateTime = w.CreateDate,
                    Type = w.TypeId

                }).ToList();


        }

        public int BalanceWalletUser(string userName)
        {
            int userId = GetUserIdByUserName(userName);
            var wallet = _walletRepository.GetWallet();

            var paymentMoney = wallet.Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay)
                .Select(w => w.Amount).ToList();

            var recivedMoney = wallet.Where(w => w.UserId == userId && w.TypeId == 2 && w.IsPay)
                .Select(w => w.Amount).ToList();

            return (paymentMoney.Sum() - recivedMoney.Sum());

        }

        public int GetUserIdByUserName(string userName)
        {
            var user = _walletRepository.GetUser();

            return user.Single(u => u.UserName == userName).UserId;
        }

        public int AddWallet(Wallet wallet)
        {
            return _walletRepository.AddWallet(wallet);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _walletRepository.Updatewallet(wallet);
        }

        public int ChargeWallet(string userName, string description, int amount, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                Description = description,
                IsPay = isPay,
                UserId = GetUserIdByUserName(userName),
                CreateDate = DateTime.Now,
                TypeId = 1
            };

            return AddWallet(wallet);

        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _walletRepository.GetWalletByWalletId(walletId);
        }

        public void Dispose()
        {
            _walletRepository?.Dispose();
        }
    }
}

