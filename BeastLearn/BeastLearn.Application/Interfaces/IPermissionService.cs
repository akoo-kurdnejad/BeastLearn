using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Domain.Models.Permission;
using BeastLearn.Domain.Models.User;

namespace BeastLearn.Application.Interfaces
{
    public interface IPermissionService: IDisposable
    {
        #region Roles

        List<Role> GetRoles();
        List<UserRole> GetUserRole();
        List<int> GetUserRole(int userId);
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void AddRolesToUser(List<int> roleIds, int userId);
        bool CheckUserRole(int roleId);
        void EditRolesToUser(List<int> roleIds, int userId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        IEnumerable<Role> GetDeleteRoles();
        #endregion

        #region Permissions

        List<Permission> GetAllPermission();
        List<int> PermissionRole(int roleId);
        List<int> RolePermission(int permissionId);
        void AddPermissionRole(List<int> permission, int roleId);
        void EditRolePermission(List<int> permission, int roleId);
        bool CheckPermission(int permissionId, string userName);


        #endregion
    }
}
