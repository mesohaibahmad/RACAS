using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;
using Serilog.Events;

namespace RACAS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string logOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} || {Level} || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}";
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .MinimumLevel.Override("Default", LogEventLevel.Information)
              .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
              .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
              .Enrich.FromLogContext()
              .WriteTo.File($"{AppContext.BaseDirectory}Logs/fileindexing.txt", rollingInterval: RollingInterval.Day, outputTemplate: logOutputTemplate)
              .CreateLogger();

            CreateWebHostBuilder(args).Build().Run();

            
        }
               
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseSerilog().ConfigureKestrel(options => options.AddServerHeader = false)
            .ConfigureServices((hostContext, services) => {

                //services.AddQuartz(q =>
                //{
                //    q.UseMicrosoftDependencyInjectionScopedJobFactory();

                //    // Register the job, loading the schedule from configuration
                //});
               
                //services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            })
                .UseStartup<Startup>();
    }
   }


