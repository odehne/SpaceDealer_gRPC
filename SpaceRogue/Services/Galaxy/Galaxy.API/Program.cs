using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Net;
using Cope.SpaceRogue.Galaxy.API.Proto;
using Cope.SpaceRogue.Infrastructure.Model;
using Cope.SpaceRogue.Infrastructure.Domain;

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

	public static class AutoMap
	{
		public static MapperConfiguration Config { get; set; }

		public static IMapper Mapper { get; set; }

		public static void Init()
		{
			Config = new MapperConfiguration(
				cfg =>
				{
					cfg.CreateMap<UpdateProductCommand, UpdateProductRequest>();
					cfg.CreateMap<Feature, FeatureDto>();
					cfg.CreateMap<Product, ProductDto>();
					cfg.CreateMap<Player, PlayerDto>();
					cfg.CreateMap<Planet, PlanetDto>();
					cfg.CreateMap<Catalog, CatalogDto>();
					cfg.CreateMap<ProductGroup, ProductGroupDto>();
					cfg.CreateMap<AddCatalogRequest, CatalogDto> ();
					cfg.CreateMap<CatalogDto, Catalog>();
					cfg.CreateMap<CatalogItem, CatalogItemDto>();
					cfg.CreateMap<MarketPlace, MarketPlaceDto>();
					cfg.CreateMap<AddPlanetReply, PlanetDto>();
					cfg.CreateMap<PlanetDto, GetPlanetReply>();
					cfg.CreateMap<Planet, AddPlanetCommand>();
					cfg.CreateMap<AddProductGroupCommand, Planet>();
					cfg.CreateMap<ProductDto, GetProductReply>();
					cfg.CreateMap<ProductGroupDto, GetProductGroupReply>();
					cfg.CreateMap<FeatureDto, GetFeatureReply>();
					cfg.CreateMap<PlayerDto, GetPlayerReply>();
					cfg.CreateMap<ShipDto, GetShipReply>();
				});

			Mapper = Config.CreateMapper();
		}
	}

	public class Program
	{
		static void Main(string[] args)
		{

			var configuration = GetConfiguration();
			Log.Logger = CreateSerilogLogger(configuration);
			Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);

			AutoMap.Init();
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
