using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Services;

namespace Cope.SpaceRogue.Galaxy.Creator
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			using var context = new GalaxyDbContext();
			context.Database.EnsureCreated();
			services.AddGrpc();
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

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
	}
}
