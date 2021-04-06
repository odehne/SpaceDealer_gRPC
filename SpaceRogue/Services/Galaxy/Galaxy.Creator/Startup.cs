using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Cope.SpaceRogue.Galaxy.Creator
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public virtual IServiceProvider ConfigureServices(IServiceCollection services)
		{
			using var context = new GalaxyDbContext();
			context.Database.EnsureCreated();
			services
				.AddGrpc(options =>
				{
					options.EnableDetailedErrors = true;
				})
				.Services
				.AddSwaggerGen()
				.AddEventBus(Configuration);

			var container = new ContainerBuilder();
			container.Populate(services);

			container.RegisterModule(new MediatorModule());
			container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));

			return new AutofacServiceProvider(container.Build());
		}

		public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
				{
					var subscriptionClientName = configuration["SubscriptionClientName"];
					var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
					var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
					var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
					var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

					var retryCount = 5;
					if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
					{
						retryCount = int.Parse(configuration["EventBusRetryCount"]);
					}

					return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
				});
			
			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

			return services;
		}
		
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				// Communication with gRPC endpoints must be made through a gRPC client.
				// To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909
				endpoints.MapGrpcService<PlanetService>();
				endpoints.MapGrpcService<MarketPlaceService>();
			});
		}
	}
}
