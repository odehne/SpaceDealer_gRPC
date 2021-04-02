using Galaxy.API.Domain;
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

namespace Cope.SpaceRogue.Galaxy.Creator
{
	class Program
	{
		static void Main(string[] args)
		{

			using var galContext = new GalaxyDbContext();
			var prodRepo = new ProductRepository(galContext);
			var planetRepo = new PlanetRepository(galContext);
			planetRepo.AddDefaults();
			var ps = planetRepo.GetItems();

			foreach(var p in ps)
			{
				Console.WriteLine($"[{p.Name}] {p.Description}");
				Console.WriteLine("Zu kaufende Produkte: ");
				foreach (var item in p.Market.ProductOfferings)
				{
					Console.WriteLine($"[{item.Title}] {item.Price}");
				}
				Console.WriteLine("Zu kaufende Produkte: ");
				foreach (var item in p.Market.ProductOfferings)
				{
					Console.WriteLine($"[{item.Title}] {item.Price}");
				}
			}

			//var planets = galContext.Planets.ToList();
			var configuration = GetConfiguration();
			Log.Logger = CreateSerilogLogger(configuration);
			Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
			var host = CreateHostBuilder(configuration, args);

			Console.WriteLine("Hello World!");
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
