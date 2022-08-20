using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.User;
using BeastLearn.Infra.Data.Context;

namespace BeastLearn.Infra.Data.Repository
{ 
    public class UserRepository:IUserRepository
    {
        private BestLearnContext _context;

        public UserRepository(BestLearnContext context)
        {
            _context = context; 
        }
        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            save();
            return user.UserId;
        }

        public User LoginUser(string email, string password)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByUserId(int userId)
        {
            return _context.Users.Find(userId);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            save();
        }

        public IQueryable<User> GetUser()
        {
            return _context.Users;
        }

        public void save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<User> GetSideBarUserPanelData()
        {
            return _context.Users;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
