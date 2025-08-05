using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RACAS.Utils;
using Quartz;
using RACAS.Models;
using RACAS.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using RACAS.Filters;
using RACAS.DAL;

namespace RACAS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BaseDataAccess.InitializeAppSettings(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddControllersWithViews()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");

                var cultures = new CultureInfo[]
                {
                        new CultureInfo("en-US"),
                        new CultureInfo("ar-SA")
                };

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

           

            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });


            //mvc compatibility


            services.AddDbContext<RACASContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnStr"))
                .EnableSensitiveDataLogging());
            services.AddSignalR();
            services.AddMemoryCache();


            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(30 * 13);
            });


            services.AddRazorPages();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "RACAS.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });





            services.AddScoped<IUsersServices, UsersServices>();
            services.AddScoped<IBranchServices, BranchServices>();
            services.AddScoped<ILookupServices, LookupServices>();
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<IPartnersServices, PartnersServices>();
            services.AddScoped<IPaymentServices, PaymentServices>();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<AuthorizeActionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRequestLocalization();
            loggerFactory.AddFile("Logs/logs-{date.txt}");

            
            app.UseDeveloperExceptionPage();

            //if (env.EnvironmentName == "Production" || env.EnvironmentName == "Staging")
            //    dataContext.Database.Migrate();
            //gotta run this DBCC CHECKIDENT('AspNetUsers', RESEED, 80001)


            if (env.EnvironmentName != "Development")
                app.UseHsts();

            app.Use(async (context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 405;
                    return;
                }
                await next();
            });

            app.UseRouting();
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseResponseCaching();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //endpoints.MapHub<ClientConnectivityHub>("/clients-hub");
                endpoints.MapControllers();
            });
        }
    }


    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await _next(context);
        }
    }
}