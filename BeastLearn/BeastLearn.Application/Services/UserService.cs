using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BeastLearn.Application.Generators;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Users;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.User;
using BeastLearn.Infra.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Application.Services
{

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IWalletService _walletservice;

        public UserService(IUserRepository userRepository, IWalletService walletservice)
        {
            _userRepository = userRepository;
            _walletservice = walletservice; 
        }

        public bool IsExistUserName(string userName)
        {
            return _userRepository.IsExistUserName(userName);
        }

        public bool IsExistEmail(string email)
        {
            return _userRepository.IsExistEmail(email);
        }

        public int AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public int GetUserIdByUserName(string userName)
        {
            var user = _userRepository.GetSideBarUserPanelData();

            return user.Single(u => u.UserName == userName).UserId;
        }

        public User LoginUser(string email, string password)
        {
            return _userRepository.LoginUser(FixedText.FixedEmail(email), PasswordHelper.EncodePasswordMd5(password));
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _userRepository.GetUserByActiveCode(activeCode);

            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _userRepository.save();

            return true;
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(FixedText.FixedEmail(email));
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _userRepository.GetUserByActiveCode(activeCode);
        }

        public User GetUserByUserName(string userName)
        {
            return _userRepository.GetUserByUserName(userName);
        }

        public User GetUserByUserId(int userId)
        {
            return _userRepository.GetUserByUserId(userId);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public InformationUserViewModel GetInformationUser(string userName)
        {
            var user = GetUserByUserName(userName);

            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.FullName = user.FullName;
            information.PhoneNumber = user.PhoneNumber;
            information.Description = user.Description;
            information.Gender = user.Gender;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = _walletservice.BalanceWalletUser(userName);

            return information;
        }

        public SideBarUserPanelViewModel GetInformationSideBar(string userName)
        {
            var user = _userRepository.GetSideBarUserPanelData();

            return user.Where(u => u.UserName == userName)
                .Select(c => new SideBarUserPanelViewModel()
                {
                    RegisterDate = c.RegisterDate,
                    ImageName = c.UserAvatar,
                    UserName = c.UserName

                }).SingleOrDefault();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            var user = _userRepository.GetSideBarUserPanelData();

            return user.Where(u => u.UserName == userName)
                .Select(c => new EditProfileViewModel()
                {
                    UserName = c.UserName,
                    Email = c.Email,
                    FullName = c.FullName,
                    Gender = c.Gender,
                    PhoneNumber = c.PhoneNumber,
                    AvatarName = c.UserAvatar,
                    Description = c.Description

                }).Single();

        }

        public void SaveImage(string imgPath, string imgName , string newImage, IFormFile avatar)
        {
            string path = "";

            if (imgName != "Defult.jpg")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), imgPath, imgName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            newImage = NameGenerator.GenerateUniqCode() + Path.GetExtension(avatar.FileName);
            path = Path.Combine(Directory.GetCurrentDirectory(), imgPath, newImage);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                avatar.CopyTo(stream);
            }
        }

        public void EditProfile(string userName, EditProfileViewModel profile)
        {
           
            if (profile.UserAvatar != null)
            {
                string imgPath = "";

                if (profile.AvatarName != "Defult.jpg")
                {
                    imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);

                    if (File.Exists(imgPath))
                    {
                        File.Delete(imgPath);
                    }
                }

                profile.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);

                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByUserName(userName);

            user.FullName = profile.FullName;
            user.PhoneNumber = profile.PhoneNumber;
            user.Gender = profile.Gender;
            user.UserAvatar = profile.AvatarName;
            user.Description = profile.Description;

            UpdateUser(user);
        }

        public bool CompareOldPassword(string userName, string oldPassword)
        {
            var user = _userRepository.GetSideBarUserPanelData();

            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);

            return user.Any(u => u.UserName == userName && u.Password == hashOldPassword);
        }

        public void ChangeUserPassword(string userName, string password)
        {
            var user = _userRepository.GetUserByUserName(userName);

            user.Password = PasswordHelper.EncodePasswordMd5(password);
            UpdateUser(user);
        }

        public UsersForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "")
        {
            var result = _userRepository.GetUser();

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            //Paging For Users

            int take = 15;
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel user = new UsersForAdminViewModel();
            user.CurrentPage = pageId;
            user.PageCount = result.Count() / take;
            user.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return user;

        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "")
        {
            var result = _userRepository.GetUser().IgnoreQueryFilters().Where(u => u.IsDelete);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email == filterEmail);
            }

            //Paging For Users

            int take = 15;
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel user = new UsersForAdminViewModel();
            user.CurrentPage = pageId;
            user.PageCount = result.Count() / take;
            user.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return user;
        }

        public int AddUserByAdmin(CreateUserViewModel createUser)
        {


            User user = new User();
            user.UserName = createUser.UserName;
            user.Email = FixedText.FixedEmail(createUser.Email);
            user.Password = PasswordHelper.EncodePasswordMd5(createUser.Password);
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            user.IsActive = true;
            user.RegisterDate = DateTime.Now;

            //save Image
           
            if (createUser.UserAvatar != null)
            {
                string imagePath = "";

                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(createUser.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);

                using (var stream = new FileStream(imagePath , FileMode.Create))
                {
                    createUser.UserAvatar.CopyTo(stream);
                }
            }

            return AddUser(user);
        }

        public EditUserViewModel GetUserForEditShow(int userId)
        {
            var user = _userRepository.GetUser();

            return user.Where(u => u.UserId == userId)
               
                .Select(u => new EditUserViewModel()
                {
                    UserName = u.UserName,
                   UserId = u.UserId,
                   Email = u.Email,
                   AvatarName = u.UserAvatar,
                   Roles = u.UserRoles.Select(r=>r.RoleId).ToList()

                }).Single();
        }

        public void EditUserByAdmin(EditUserViewModel editUser)
        {
            var user = _userRepository.GetUserByUserId(editUser.UserId);
            user.Email = FixedText.FixedEmail(editUser.Email);
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }

            if (editUser.UserAvatar != null)
            {
                //Delete Old Image

                if (editUser.AvatarName != "Defult.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }
                //save New Image
                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            _userRepository.UpdateUser(user);
        }

        public InformationUserViewModel GetInformationUser(int userId)
        {
            var user = GetUserByUserId(userId);

            InformationUserViewModel information = new InformationUserViewModel();
            information.FullName = user.FullName;
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.Description = user.Description;
            information.PhoneNumber = user.PhoneNumber;
            information.Gender = user.Gender;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = _walletservice.BalanceWalletUser(user.UserName);

            return information;
        }

        public void DeleteUser(int userId)
        {
            var user = GetUserByUserId(userId);

            user.IsDelete = true;
            UpdateUser(user);
        }

        public void Dispose()
        {
            _userRepository?.Dispose();
        }
    }
}
