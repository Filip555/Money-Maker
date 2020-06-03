using Api.Application.Queries;
using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.ChartAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;
using Infrastructure.Repository.Account;
using Infrastructure.Repository.Chart;
using Infrastructure.Repository.InstrumentRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Api2
{
    using Api.Infrastructure.Filters;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AddHttpClient(services);
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IChartRepository, ChartRepository>();
            services.AddScoped<IInstrumentRepository, InstrumentRepository>();
            services.AddScoped<IChartQueries, ChartQueries>();
            services.AddScoped<ISignalQueries, SignalQueries>();
            services.AddScoped<ISimulatorQueries, SimulatorQueries>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Money Maker API", Version = "v1" });
            });
            services.AddMvc(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); });
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Money Maker API");
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void AddHttpClient(IServiceCollection services)
        {
            services.AddHttpClient(
                "custom",
                c =>
                    {
                        services.BuildServiceProvider().GetService<IHttpContextAccessor>();
                        c.DefaultRequestHeaders.Add("Accept", "*/*");
                        c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
                    });
        }
    }
}
