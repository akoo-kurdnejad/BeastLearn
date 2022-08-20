using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Permission;
using BeastLearn.Domain.Models.User;
using BeastLearn.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Infra.Data.Repository
{
    public class PermissionRepository:IPermissionRepository
    {
        private BestLearnContext _context;

        public PermissionRepository(BestLearnContext context)
        {
            _context = context; 
        }
        public IEnumerable<Role> GetRole()
        {
            return _context.Roles;
        }

        public IQueryable<Role> GetDeleteRole()
        {
            return _context.Roles;
        }

        public IEnumerable<UserRole> GetUserRoles()
        {
            return _context.UserRoles;
        }

        public void AddRole(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
           
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            Save();
            return role.RoleId;
        }

        public void EditRolesToUser(int userId)
        {
            _context.UserRoles.Where(r => r.UserId == userId)
                .ForEachAsync(r => _context.UserRoles.Remove(r));

        }

        public void EditRolePermission(int roleId)
        {
            _context.RolePermissions.Where(r => r.RoleId == roleId)
                .ForEachAsync(r => _context.RolePermissions.Remove(r));
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return _context.Permissions;
        }

        public IEnumerable<RolePermission> GetRolePermission()
        {
            return _context.RolePermissions;
        }

        public void AddRolePermission(RolePermission permission)
        {
            _context.RolePermissions.Add(permission);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
