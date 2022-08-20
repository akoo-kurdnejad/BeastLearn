using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Models.Course;
using BeastLearn.Domain.Models.Order;
using BeastLearn.Domain.Models.Permission;
using BeastLearn.Domain.Models.User;
using BeastLearn.Domain.Models.Wallet;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Infra.Data.Context
{
    public class BestLearnContext : DbContext
    {
        public BestLearnContext(DbContextOptions<BestLearnContext> options) : base(options)
        {

        }

        #region Users

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        #endregion

        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletTypes { get; set; }

        #endregion

        #region Permission

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region Course

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        public DbSet<CourseVote> CourseVote { get; set; }
       

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetailses { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion

        // Has QueryFilter For DeleteUsers
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //For Contorol EntityFreamwork By Migration
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;



            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<CourseGroup>()
                .HasQueryFilter(g => !g.IsDelete);

            modelBuilder.Entity<Course>()
                .HasQueryFilter(g => !g.IsDelete);

            modelBuilder.Entity<CourseEpisode>()
                .HasQueryFilter(g => !g.IsDelete);

            modelBuilder.Entity<Discount>()
                .HasQueryFilter(d => !d.IsDelete);

            modelBuilder.Entity<CourseComment>()
                .HasQueryFilter(c => !c.IsDelete);
        }
    }
}


