using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Cope.SpaceRogue.Infrastructure.Domain;
using Cope.SpaceRogue.Infrastructure.Model;
using Cope.SpaceRogue.Fighting.API.Models;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API
{

	//Start rabbit mq
	// docker run --rm -it --hostname my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management

	public static class Factory
	{
		public static AutofacServiceProvider ServiceProvider { get; set; }
		public static IMediator Mediator
		{
			get
			{
				if (ServiceProvider != null)
					return (IMediator)ServiceProvider.GetRequiredService(typeof(IMediator));
				return null;
			}
		}
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
				});

			Mapper = Config.CreateMapper();
		}
	}

	public class Program
	{
		public static string Namespace = typeof(Startup).Namespace;
		public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
		static async Task Main(string[] args)
		{
			var configuration = GetConfiguration();
			Log.Logger = CreateSerilogLogger(configuration);
			Log.Information("Configuring web host ({ApplicationContext})...", AppName);

			AutoMap.Init();
			var host = CreateHostBuilder(configuration, args);

			await Engine.Init();

			var engineThread = new Thread(Engine.Play) { IsBackground = true };
			engineThread.Start();

			//var command = new StartJourneyCommand { ShipId = Engine.Galaxy.Ships[0].ShipId.ToString(), TargetPosX = 0, TargetPosY = 10, TargetPosZ = 1 };
			//var p = await Factory.Mediator.Send(command);
			host.Run();
		}

		public static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args)
		{
			var http1Port = int.Parse(configuration["Kestrel:http1Port"]);
			var http2Port = int.Parse(configuration["Kestrel:http2Port"]);

			return WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
				.CaptureStartupErrors(true)
				.ConfigureKestrel(options =>
				{
					options.Listen(IPAddress.Loopback, http1Port, listenOptions =>
					{
						listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
					});
					options.Listen(IPAddress.Loopback, http2Port, listenOptions =>
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