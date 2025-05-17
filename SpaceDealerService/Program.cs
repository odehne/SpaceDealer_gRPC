using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using SpaceDealer;
using System.Threading;
using SpaceDealerService.Repos;
using SpaceDealerModels.Repositories;

namespace SpaceDealerService
{
    public class Program
	{
		public static SpaceDealerGame TheGame { get; set; }
		public static Logger TheLogger {get; set;}
		public static SqlPersistor Persistor { get; set; } 
	
	
		public static void Main(string[] args)
		{
			TheLogger = new Logger(TraceEventType.Verbose);
			Persistor = new SqlPersistor(TheLogger, $"{GetAppLocation()}db\\spacedealer.db");
			
			TheGame = new SpaceDealerGame(TheLogger);
			TheGame.Init();

            var productRepo = new ProductsRepository(Persistor);
			foreach(var product in Repository.ProductLibrary)
			{
				var p = productRepo.GetItem(product.Name, null);
                if ( p == null)
				{
					TheLogger.Log($"Adding product {product.Name} to database.", TraceEventType.Information);
					productRepo.Save(product);
				}
				else
				{
					product.Id = p.Id;
                }
            }

            Persistor.SaveGalaxy(Program.TheGame.Galaxy);
            Persistor.SavePlayers(Program.TheGame.FleetCommanders);

            var engine = new GameEngine(TheLogger, TheGame.Galaxy, TheGame.FleetCommanders);
			var engineThread = new Thread(engine.Play) { IsBackground = false };
			engineThread.Start();

			CreateHostBuilder(args).Build().Run();
		}

		public static string GetAppLocation()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
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
