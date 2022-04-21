using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using Ardalis.GuardClauses;
using Microsoft.Azure.Cosmos.Fluent;
using ChargingStationApi.Handlers;
using ChargingStationApi.Services;
using ChargingStationApi.Repository;
using ChargingStationApi.CosmosInitializers;
using ChargingStationApi.Repository.Interfaces;
using ChargingStationApi.CosmosInitializers.Interfaces;

namespace ChargingStationApi
{
    public class Startup
    {
        private const string DbNamePath = "Repository:CosmosDb:DbName";
        public const string DbAccessKeyPath = "Repository:CosmosDb:DbAccessKey";
        public const string DbPath = "Repository:CosmosDb:DbEndpoint";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddMemoryCache();
            services.AddApplicationInsightsTelemetry();
            services
                .AddMediatR(typeof(CreateChargingStationCommandHandler).GetTypeInfo().Assembly)
                .AddScoped<IChargingStationService, ChargingStationService>()
                .AddSingleton(serviceProvider => RegisterCosmosClient(serviceProvider))
                .AddSingleton<ICosmosFeedReader, CosmosFeedReader>();

            services.AddTransient(serviceProvider =>
            {
                var cosmosClient = serviceProvider.GetService<CosmosClient>();
                var configuration = serviceProvider.GetService<IConfiguration>();
                var logger = serviceProvider.GetService<ILogger<ChargingStationCosmosRepository>>();
                var feedReader = serviceProvider.GetService<ICosmosFeedReader>();
                IChargingStationCosmosRepository repository = new ChargingStationCosmosRepository(cosmosClient, configuration, logger, feedReader);

                return repository;
            });

            services.AddTransient<IInitializer>(services =>
                new ContainerInitializer(
                    "ChargingStations",
                    services.GetService<IConfiguration>(),
                    services.GetService<ILogger<ContainerInitializer>>(),
                    services.GetService<CosmosClient>()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChargingStationApi", Version = "v1" });
            });
        }

        private static readonly Func<IServiceProvider, CosmosClient> RegisterCosmosClient = (serviceProvider) =>
        {
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            string endpoint = Guard.Against.NullOrEmpty(
                configuration[DbPath],
                DbPath,
                $"Please specify a valid {DbPath} in the appSettings.json file or your Azure App Service Settings.");

            string authKey = Guard.Against.NullOrEmpty(
                configuration[DbAccessKeyPath],
                DbAccessKeyPath,
                $"Please specify a valid {DbPath} in the appSettings.json file or your Azure App Service Settings.");

            CosmosClientBuilder configurationBuilder = new CosmosClientBuilder(endpoint, authKey)
                .WithSerializerOptions(new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase });

            return configurationBuilder.Build();
        };

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChargingStationApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
