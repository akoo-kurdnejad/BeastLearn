using BeastLearn.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace BeastLearn.Application.Interfaces
{
   public interface IUserService: IDisposable
    {
        #region Users

        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        int GetUserIdByUserName(string userName);
        User LoginUser(string email, string password);
        bool ActiveAccount(string activeCode);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activeCode);
        User GetUserByUserName(string userName);
        User GetUserByUserId(int userId);
        void UpdateUser(User user);

        #endregion

        #region UserPanel

        InformationUserViewModel GetInformationUser(string userName);
        SideBarUserPanelViewModel GetInformationSideBar(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string userName);
        void SaveImage(string imgPath, string imgName , string newImage , IFormFile avatar);
        void EditProfile(string userName, EditProfileViewModel profile);
        bool CompareOldPassword(string userName, string oldPassword);
        void ChangeUserPassword(string userName, string password);

        #endregion

        #region AdminPanel

        UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "");
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "");
        int AddUserByAdmin(CreateUserViewModel createUser);
        EditUserViewModel GetUserForEditShow(int userId);
        void EditUserByAdmin(EditUserViewModel editUser);
        InformationUserViewModel GetInformationUser(int userId);
        void DeleteUser(int userId);

        #endregion
    }
}
