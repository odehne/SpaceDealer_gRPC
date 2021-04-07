﻿using Cope.SpaceRogue.Galaxy.Creator;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator
{

    class Program
	{
		static async Task Main(string[] args)
		{

			var configuration = GetConfiguration();
			Log.Logger = CreateSerilogLogger(configuration);
			Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
			var host = CreateHostBuilder(configuration, args);

			// var p = new Product();
			// p.PricePerUnit = 11.78;
			// p.SizeInUnits = 0.3;
		
			// Console.Write("Double value: ");
			// var line = Console.ReadLine();
			// p.SizeInUnits = double.Parse(line);
			// Console.WriteLine($"Size: {p.SizeInUnits}t");
			
			// Environment.Exit(0);
		

			//using var galContext = new GalaxyDbContext();
			var menu = new Menu(new GalaxyDbContext("c:\\temp\\GalaxyRouge.db"));
			await menu.ShowMenu();

			// var prodRepo = new ProductRepository(galContext);
			// var planetRepo = new PlanetRepository(galContext);
			// var featRepo = new FeatureRepository(galContext);
			// var playerRepo = new PlayerRepository(galContext);
			// var shipRepo = new ShipRepository(galContext);
			
			// var podGroupsRepo = new ProductGroupRepository(galContext);
			// podGroupsRepo.AddDefaults();

			// prodRepo.AddDefaults();
		
			// planetRepo.AddDefaults();
			// var marketsRepo = new MarketPlaceRepository(galContext);
			// var markets = marketsRepo.GetItems();
			// var ps = planetRepo.GetItems();	
			
			// featRepo.AddDefaults();
		
			// foreach(var p in ps)
			// {
			// 	Console.WriteLine($"[{p.Name}] {p.Description}");
			// 	Console.WriteLine("Zu kaufende Produkte: ");
			// 	foreach (var item in p.Market.ProductOfferings.CatalogItems)
			// 	{
			// 		Console.WriteLine($"[{item.Title}] {item.Price}");
			// 	}
			// 	Console.WriteLine("Zu kaufende Produkte: ");
			// 	foreach (var item in p.Market.ProductDemands.CatalogItems)
			// 	{
			// 		Console.WriteLine($"[{item.Title}] {item.Price}");
			// 	}
			// }

			// // var ship = new Ship("Wick01", 3, 3);
			// // shipRepo.AddItem(ship);
		
			// // var player = new Player("Oliver", planetRepo.GetItemByName("Erde"), 10000, Player.PlayerTypes.Human);
			// // player.Fleet.Add(ship);
			// // playerRepo.AddItem(player);

			// var player = playerRepo.GetItemByName("Oliver");
			// Console.WriteLine($"Player: {player.Name} Raumschiff: {player.Fleet[0].Name}");
		}

		public static string Namespace = typeof(Startup).Namespace;
		public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);


		public static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
				.CaptureStartupErrors(false)
				.ConfigureKestrel(options =>
				{
					var ports = GetDefinedPorts(configuration);
					options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
					{
						listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
					});
					options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
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
