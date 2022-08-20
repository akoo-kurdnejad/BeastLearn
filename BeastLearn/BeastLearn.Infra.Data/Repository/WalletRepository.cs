using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.User;
using BeastLearn.Domain.Models.Wallet;
using BeastLearn.Infra.Data.Context;

namespace BeastLearn.Infra.Data.Repository
{
    public class WalletRepository:IWalletRepository
    {
        private BestLearnContext _context;

        public WalletRepository(BestLearnContext context)
        {
            _context = context; 
        }
        public IEnumerable<Wallet> GetWallet()
        {
            return _context.Wallets;
        }

        public IEnumerable<User> GetUser()
        {
            return _context.Users;
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            save();
            return wallet.WalletId;
        }

        public void Updatewallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            save();
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

}
