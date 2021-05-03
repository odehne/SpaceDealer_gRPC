using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Cope.SpaceRogue.Infrastructure.Model;
using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API
{

	public static class Factory
	{
		public static AutofacServiceProvider ServiceProvider { get; set; }
		public static IMediator Mediator => (IMediator)ServiceProvider.GetRequiredService(typeof(IMediator));
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
					cfg.CreateMap<Ship, ShipModel>();
					cfg.CreateMap<Planet, PlanetModel>();
				});

			Mapper = Config.CreateMapper();
		}
	}

	public class Program
	{
		public static string Namespace = typeof(Startup).Namespace;
		public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
		public static Cache TravelCache { get; set; }
		static async Task Main(string[] args)
		{
			var configuration = GetConfiguration();
			Log.Logger = CreateSerilogLogger(configuration);
			Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);

			AutoMap.Init();
			var host = CreateHostBuilder(configuration, args);

			TravelCache = new Cache(Factory.Mediator);
			await TravelCache.Load();

			host.Run();
		}

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


		static ILogger CreateSerilogLogger(IConfiguration configuration)
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