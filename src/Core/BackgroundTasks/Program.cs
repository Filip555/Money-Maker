using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;

namespace BackgroundTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                using (var logger = BuildSerilog())
                {
                    logger.Fatal(e, "Critical error - BackgroundTasks.");
                }
            }
        }

        private static Logger BuildSerilog()
        {
            var logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(@"http://localhost:9200/"))
                        {
                            AutoRegisterTemplate = true,
                        })
                    .CreateLogger();

            return logger;
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
                  logging.AddDebug();
                  logging.AddEventSourceLogger();
                  logging.AddAzureWebAppDiagnostics();
              })
                .UseUrls("http://*:61105")
                .UseStartup<Startup>();
    }
}
