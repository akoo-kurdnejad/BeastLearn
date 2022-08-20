using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Models.Permission;
using BeastLearn.Domain.Models.User;

namespace BeastLearn.Domain.Interfaces
{
   public interface IPermissionRepository: IDisposable
    {
        #region Roles

        IEnumerable<Role> GetRole();
        IQueryable<Role> GetDeleteRole();
        IEnumerable<UserRole> GetUserRoles();
        void AddRole(UserRole userRole);
        int AddRole(Role role);
        void EditRolesToUser(int userId);
        Role GetRoleById(int roleId);
        void UpdateRole(Role role);
        void Save();

        #endregion

        #region Permissions

        IEnumerable<Permission> GetPermissions();
        IEnumerable<RolePermission> GetRolePermission();
        void AddRolePermission(RolePermission permission);
        void EditRolePermission(int roleId);


        #endregion
    }
}
