using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using SpaceDealer;
using SpaceDealerModels.Repositories;
using SpaceDealerService.Extensions;
using SpaceDealerService.Repos;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using Scalar.AspNetCore;

namespace SpaceDealerService
{
    public class Program
	{
		public static SpaceDealerGame TheGame { get; set; }
		public static Logger TheLogger {get; set;}
		public static SqlPersistor Persistor { get; set; } 
	
	
		public static void Main(string[] args)
		{

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            var configBuilder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                           .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                           .AddEnvironmentVariables();

            var builder = WebApplication.CreateBuilder(args);


            var config = configBuilder.Build();

            var cs = new AppSettings();
            cs.LoadSettings(config);

            var logs = "logs".UnitizePath();
            var logFilePath = $"{cs.Settings["FileStorageRootPath"].UnitizePath()}{logs}space_dealer_{Environment.MachineName}_.txt";

            // Controllers + JWT Authentication
            builder.Services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
                .AddXmlSerializerFormatters();

            builder.Services.AddOpenApi(options =>
            {
                //options.AddSecurityScheme(JwtBearerDefaults.AuthenticationScheme, scheme =>
                //{
                //    scheme.In = ParameterLocation.Header;
                //    scheme.Type = SecuritySchemeType.Http;
                //    scheme.Scheme = JwtBearerDefaults.AuthenticationScheme;
                //    scheme.BearerFormat = "JWT";
                //});
                //options.AddAuthResponse();
            });

            builder.Host.UseSerilog((ctx, services, cfg) => cfg.ReadFrom.Configuration(config)
                                                                    .MinimumLevel.ControlledBy(DynamicLogLevel.Switch)
                                                                    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                                                                    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                                                                    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                                                                    .MinimumLevel.Override("Microsoft.AspNetCore.Cors.Infrastructure.CorsService", LogEventLevel.Warning)
                                                                    .Enrich.FromLogContext()
                                                                    .WriteTo.Console()
                                                                    .WriteTo.File(
                                                                        path: logFilePath,
                                                                        outputTemplate: "{Timestamp:o} [{Level:u3}] ({SourceContext}) {Message} {NewLine}{Exception}",
                                                                        rollingInterval: RollingInterval.Day)
                                                                   );


            var app = builder.Build();


            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                .WithTitle("SpaceDealer API")
                .WithTheme(ScalarTheme.Saturn)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
            app.UseRouting();
            app.MapControllers();


            TheLogger = new Logger(TraceEventType.Verbose);
			Persistor = new SqlPersistor(TheLogger, $"{GetAppLocation()}db\\spacedealer.db");

            TheGame = new SpaceDealerGame(TheLogger);
            TheGame.Init();

            var productRepo = new ProductsRepository(Persistor);
            foreach (var product in Repository.ProductLibrary)
            {
                var p = productRepo.GetItem(product.Name, null);
                if (p == null)
                {
                    TheLogger.Log($"Adding product {product.Name} to database.", TraceEventType.Information);
                    productRepo.Save(product);
                }
                else
                {
                    product.Id = p.Id;
                }
            }

            foreach (var planet in TheGame.Galaxy)
            {
                if (planet.Industry.ProductsNeeded.Count <= 0)
                {
                    var p = Repository.ProductLibrary.GetRandomProduct();
                    planet.Industry.AddNeededProduct(p);
                }
                if (planet.Industry.GeneratedProducts.Count <= 0)
                {
                    var p = Repository.ProductLibrary.GetRandomProduct();
                    planet.Industry.AddGeneratedProduct(p);
                }
            }


            Persistor.SaveGalaxy(TheGame.Galaxy);
            Persistor.SavePlayers(TheGame.FleetCommanders);

            var engine = new GameEngine(TheLogger, TheGame.Galaxy, TheGame.FleetCommanders);
			var engineThread = new Thread(engine.Play) { IsBackground = false };
			engineThread.Start();

            //CreateHostBuilder(args).Build().Run();

            app.MapGet("/", () => Results.Ok("API is running"));
            app.MapMethods("/", new[] { "HEAD" }, () => Results.Ok());
            app.Run();
        }

		public static string GetAppLocation()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		// Additional configuration is required to successfully run gRPC on macOS.
		// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
		//public static IHostBuilder CreateHostBuilder(string[] args) =>
		//	Host.CreateDefaultBuilder(args)
		//		.ConfigureWebHostDefaults(webBuilder =>
		//		{
		//			webBuilder.UseStartup<Startup>();
		//		});
	}
}
