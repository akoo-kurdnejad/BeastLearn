using System;
using System.Collections.Generic;
using System.Linq;
using BeastLearn.Domain.Models.User;

namespace BeastLearn.Domain.Interfaces
{
    public interface IUserRepository: IDisposable
    {
        #region User

        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(string email, string password);
        User GetUserByActiveCode(string activeCode);
        User GetUserByEmail(string email);
        User GetUserByUserId(int userId);
        User GetUserByUserName(string userName);
        void UpdateUser(User user);
        IQueryable<User> GetUser();
        void save();

        #endregion

        #region UserPanel

        IEnumerable<User> GetSideBarUserPanelData();


        #endregion
    }
}
