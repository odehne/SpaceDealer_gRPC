﻿using Cope.SpaceRogue.Galaxy.API.Model;
using Galaxy.API.Domain;
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

namespace Cope.SpaceRogue.Services.Galaxy.API
{
	class Program
	{
		static void Main(string[] args)
		{
			//using var galContext = new GalaxyDbContext();

			//galContext.Add(new Planet
			//{
			//	PlanetId = Guid.NewGuid(),
			//	PosX = 0,
			//	PosY = 0,
			//	PosZ = 0,
			//	Description = "Erde",
			//	Name = "Erde",
			//	MarketPlaceId = Guid.NewGuid()
			//});
			//galContext.SaveChanges();

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