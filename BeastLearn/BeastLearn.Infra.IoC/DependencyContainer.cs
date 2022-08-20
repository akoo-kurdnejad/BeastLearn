using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Application.Convertors;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Services;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Infra.Data.Repository;

namespace BeastLearn.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IMailSender, SendEmail>();


            #endregion


            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            #endregion
        }
    }
}
