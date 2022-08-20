using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Application.ViewModels.Users;
using BeastLearn.Domain.Models.Wallet;

namespace BeastLearn.Application.Interfaces
{
    public interface IWalletService: IDisposable
    {
        List<WalletViewModel> GetWalletUser(string userName);
        int BalanceWalletUser(string userName);
        int GetUserIdByUserName(string userName);
        int AddWallet(Wallet wallet);
        void UpdateWallet(Wallet wallet);
        int ChargeWallet(string userName, string description, int amount, bool isPay = false);
        Wallet GetWalletByWalletId(int walletId);
    }
}
