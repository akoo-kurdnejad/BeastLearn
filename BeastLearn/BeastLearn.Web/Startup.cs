using System;
using System.Security.Cryptography.X509Certificates;
using BeastLearn.Application.Jobs;
using BeastLearn.Infra.Data.Context;
using BeastLearn.Infra.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace BeastLearn.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();


            //For Upload Vidue in host Linux and mac

            //services.Configure<FormOptions>(option =>
            //{
            //    option.MultipartBodyLengthLimit = 6000000;
            //});


            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.Cookie.Name = "Cookie";
                options.CookieHttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(423110);

            });

            #endregion

            #region DataBase Context

            services.AddDbContext<BestLearnContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("BestLearnConnection"));
                }
            );

            #endregion

            #region Qurtes

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<RemoveCartJob>();

            services.AddSingleton(new JobSchedule(jobType: typeof(RemoveCartJob), cronExpression:
                "0 0 */6 ? * *"
            ));

            services.AddHostedService<QuartzHostedService>();

            #endregion


            // Layer IoC
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
            
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });
     

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        //Layer IoC
        public static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}
