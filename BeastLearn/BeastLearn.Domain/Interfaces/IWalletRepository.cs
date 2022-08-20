using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Domain.Models.User;
using BeastLearn.Domain.Models.Wallet;

namespace BeastLearn.Domain.Interfaces
{
    public interface IWalletRepository: IDisposable
    {
        IEnumerable<Wallet> GetWallet();
        IEnumerable<User> GetUser();
        int AddWallet(Wallet wallet);
        void Updatewallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void save();
    }
}
