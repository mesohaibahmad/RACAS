using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RACAS;
using RACAS.Constants;
using RACAS.DAL;
using RACAS.Filters;
using RACAS.Middleware;
using RACAS.Models;
using RACAS.Services;
using RACAS.Utils;
using Serilog;
using Serilog.Events;
using System;
using System.Globalization;
using System.Linq;



using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Quartz;
using System.Collections.Generic;
using System.Text;









var builder = WebApplication.CreateBuilder(args);

// Logging
string logOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} || {Level} || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Default", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File($"{AppContext.BaseDirectory}Logs/fileindexing.txt", rollingInterval: RollingInterval.Day, outputTemplate: logOutputTemplate)
    .CreateLogger();

builder.Host.UseSerilog();

// Configuration
BaseDataAccess.InitializeAppSettings(builder.Configuration);

// Services
builder.Services.AddControllersWithViews().AddViewLocalization();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-SA")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddDbContext<RACASContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr"))
    .EnableSensitiveDataLogging());

builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "RACAS.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Dependency Injection
builder.Services.AddScoped<IUsersServices, UsersServices>();
builder.Services.AddScoped<IBranchServices, BranchServices>();
builder.Services.AddScoped<ILookupServices, LookupServices>();
builder.Services.AddScoped<ICommonServices, CommonServices>();
builder.Services.AddScoped<IPartnersServices, PartnersServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<AuthorizeActionFilter>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

var app = builder.Build();

// Middleware
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseDeveloperExceptionPage();

if (!app.Environment.IsDevelopment())
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
    endpoints.MapControllers();
});

// DB Seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RACASContext>();

    if (!db.Users.Any(u => u.UserTypeId == 1))
    {
        db.Users.Add(new User
        {
            FirstName = "Admin",
            LastName = "User",
            UserName = "admin",
            Password = "123", // Use a hashed password in production!
            UserTypeId = 1,
            Email = "admin@gmail.com",
            ModifiedBy = CommonDataParam.LoginId,
            ModifiedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = CommonDataParam.LoginId,
            RecordStatus = "Active",
        });

        db.SaveChanges();
        Console.WriteLine("✅ Default admin user created.");
    }
}

app.Run();

