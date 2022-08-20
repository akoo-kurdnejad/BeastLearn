using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Application.Interfaces;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Permission;
using BeastLearn.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Application.Services
{
    public class PermissionService: IPermissionService
    {
        private IPermissionRepository _permissionRepository;
        private IUserService _userService;

        public PermissionService(IPermissionRepository permissionRepository, IUserService userService)
        {
            _permissionRepository = permissionRepository;
            _userService = userService;
        }
        public List<Role> GetRoles()
        {
            return _permissionRepository.GetRole().ToList();
        }

        public List<UserRole> GetUserRole()
        {
            var userRole = _permissionRepository.GetUserRoles();

            return userRole.ToList();
        }

        public List<int> GetUserRole(int userId)
        {
           return _permissionRepository.GetUserRoles()
               .Where(u => u.UserId == userId)
               .Select(u=>u.RoleId).ToList();

           
        }

        public int AddRole(Role role)
        {
            _permissionRepository.AddRole(role);
            return role.RoleId;
        }

        public Role GetRoleById(int roleId)
        {
            return _permissionRepository.GetRoleById(roleId);
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var item in roleIds)
            {
                _permissionRepository.AddRole(new UserRole()
                {
                    RoleId = item,
                    UserId = userId
                });
                _permissionRepository.Save();
            }
        }

        public bool CheckUserRole(int roleId)
        {
            var userRole = _permissionRepository.GetUserRoles();

            return userRole.Any(r => r.RoleId == roleId);
        }

        public void EditRolesToUser(List<int> roleId, int userId)
        {
            // Delete Roles
           _permissionRepository.EditRolesToUser(userId);
           AddRolesToUser(roleId , userId);

        }

        public void UpdateRole(Role role)
        {
            _permissionRepository.UpdateRole(role);
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }

        public IEnumerable<Role> GetDeleteRoles()
        {
            var result = _permissionRepository.GetDeleteRole()
                .IgnoreQueryFilters().Where(r => r.IsDelete);

            return result.ToList();
        }

        public List<Permission> GetAllPermission()
        {
            return _permissionRepository.GetPermissions().ToList();
        }

        public List<int> PermissionRole(int roleId)
        {
            var rolePermission = _permissionRepository.GetRolePermission();

            return rolePermission.Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId).ToList();
        }

        public List<int> RolePermission(int permissionId)
        {
            return _permissionRepository.GetRolePermission()
                .Where(r => r.PermissionId == permissionId)
                .Select(r => r.RoleId).ToList();
        }

        public void AddPermissionRole(List<int> permission, int roleId)
        {
            foreach (var item in permission)
            {
               _permissionRepository.AddRolePermission(new RolePermission()
               {
                   RoleId = roleId,
                   PermissionId = item
               });
            }
            _permissionRepository.Save();
        }

        public void EditRolePermission(List<int> permission, int roleId)
        {
            //Delete RolePermission

            _permissionRepository.EditRolePermission(roleId);
            AddPermissionRole(permission,roleId);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            List<int> userRoles = GetUserRole(userId);
            List<int> rolePermission = RolePermission(permissionId);

            return rolePermission.Any(p => userRoles.Contains(p));
        }

        public void Dispose()
        {
            _permissionRepository?.Dispose();
        }
    }
}
