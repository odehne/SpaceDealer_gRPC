using Autofac.Extensions.DependencyInjection;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API
{
	public static class Factory
	{
		public static AutofacServiceProvider ServiceProvider { get; set; }

		public static IShipRepository ShipRepository => (IShipRepository)ServiceProvider.GetService(typeof(IShipRepository));
		public static IPlanetRepository PlanetRepository => (IPlanetRepository)ServiceProvider.GetService(typeof(IPlanetRepository));
		public static IPlayerRepository PlayerRepository => (IPlayerRepository)ServiceProvider.GetService(typeof(IPlayerRepository));
		public static IProductGroupRepository ProductGroupRepository => (IProductGroupRepository)ServiceProvider.GetService(typeof(IProductGroupRepository));
		public static IProductRepository ProductRepository => (IProductRepository)ServiceProvider.GetService(typeof(IProductRepository));
		public static IMarketPlaceRepository MarketPlaceRepository => (IMarketPlaceRepository)ServiceProvider.GetService(typeof(IMarketPlaceRepository));
		public static ICatalogItemsRepository CatalogItemsRepository => (ICatalogItemsRepository)ServiceProvider.GetService(typeof(ICatalogItemsRepository));
	}

	public class Program
	{
		

		static async Task Main(string[] args)
		{

			var configuration = GetConfiguration();
			Log.Logger = CreateSerilogLogger(configuration);
			Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
			var host = CreateHostBuilder(configuration, args);
			host.Run();
		}

		public static string Namespace = typeof(Startup).Namespace;
		public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);


		public static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
				.CaptureStartupErrors(true)
				.ConfigureKestrel(options =>
				{
					options.Listen(IPAddress.Loopback, 51777, listenOptions =>
					{
						listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
					});
					options.Listen(IPAddress.Loopback, 8891, listenOptions =>
					{
						listenOptions.Protocols = HttpProtocols.Http2;
					});

				})
				.UseStartup<Startup>()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseWebRoot("Galaxy")
				.UseSerilog()
				.Build();
		}

		static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
		{
			var grpcPort = config.GetValue("GRPC_PORT", 81);
			var port = config.GetValue("PORT", 80);
			return (port, grpcPort);
		}


		static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();

			var config = builder.Build();
			return builder.Build();
		}


		static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
		{
			var seqServerUrl = configuration["Serilog:SeqServerUrl"];
			var logstashUrl = configuration["Serilog:LogstashgUrl"];
			return new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.Enrich.WithProperty("ApplicationContext", Program.AppName)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
				.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}

	}
}
