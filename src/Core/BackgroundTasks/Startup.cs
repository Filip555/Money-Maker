using System;
using System.Reflection;

using Infrastructure.Repository.Account;
using Infrastructure.Repository.Chart;
using Infrastructure.Repository.InstrumentRepository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace BackgroundTasks
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;
    using Infrastructure.Services;
   

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var elasticUri = Configuration["ElasticConfiguration:Uri"];

            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            AutoRegisterTemplate = true,
                        })
                    .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AddHttpClient(services);

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IChartRepository, ChartRepository>();
            services.AddScoped<IInstrumentRepository, InstrumentRepository>();
            services.AddScoped<INotifier, Notifier>();
            services.AddHostedService<MoneyMakerHostedService>();

            var dataAccess = Assembly.GetExecutingAssembly();
            services.AddMediatR(dataAccess);

            services.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "azure-diagnostics-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;
            });
        }
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
        private void AddHttpClient(IServiceCollection services)
        {
            services.AddHttpClient(
                "custom",
                c =>
                    {
                        var serviceProvider = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
                        c.DefaultRequestHeaders.Add("Accept", "*/*");
                        c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
                    });
        }
    }
}
