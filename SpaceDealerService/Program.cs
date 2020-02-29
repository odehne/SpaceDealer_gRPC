using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using SpaceDealer;
using System.Threading;
using SpaceDealerModels.Repositories;

namespace SpaceDealerService
{
	public class Program
	{
		public static SpaceDealerGame TheGame { get; set; }
		public static Logger TheLogger {get; set;}
	
		public static void Main(string[] args)
		{
			TheLogger = new Logger(TraceEventType.Verbose);
			TheGame = new SpaceDealerGame(TheLogger);
			TheGame.Init();

			var engine = new GameEngine(TheLogger, TheGame.Galaxy, TheGame.FleetCommanders);
			var engineThread = new Thread(engine.Play) { IsBackground = false };
			engineThread.Start();

			CreateHostBuilder(args).Build().Run();


	

		}

		// Additional configuration is required to successfully run gRPC on macOS.
		// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
