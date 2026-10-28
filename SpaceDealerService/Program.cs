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
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using Scalar.AspNetCore;

namespace SpaceDealerService
{
    public class Program
	{
		public static SpaceDealerGame TheGame { get; set; }
	
	
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
                .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy => policy
                    .WithOrigins("http://localhost:5144", "https://localhost:7008")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

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
            app.UseCors();
            app.MapControllers();


            var appPath = Path.Combine(GetAppLocation(), "db");
            var persistor = new SqlPersistor(Log.Logger, Path.Combine(appPath, "spacedealer.db"));

            TheGame = new SpaceDealerGame(Log.Logger, persistor);
            TheGame.Init();

            var productRepo = new ProductsRepository(persistor);
            foreach (var product in Repository.ProductLibrary)
            {
                var p = productRepo.GetItemByName(product.Name);
                if (p == null)
                {
                    Log.Logger.Information($"Adding product {product.Name} to database.");
                    productRepo.Save(product);
                }
                else
                {
                    product.Id = p.Id;
                }
            }

            foreach (var planet in TheGame.Galaxy.Values)
            {
                if (planet.Industry.ProductsNeeded.Count <= 0)
                {
                    var p = Repository.ProductLibrary.GetRandomProduct();
                    planet.Industry.AddNeededProduct(Repository.CloneProduct(p, Repository.GetRandomNeededInterest()));
                    Repository.AlignNeedsWithNeighbourhood(planet, TheGame.Galaxy.Values);
                }
                if (planet.Industry.GeneratedProducts.Count <= 0)
                {
                    var p = Repository.ProductLibrary.GetRandomProduct();
                    planet.Industry.AddGeneratedProduct(Repository.CloneProduct(p, Repository.GetRandomGeneratedInterest()));
                }
            }

            Log.Logger.Information("Starting Space Dealer service...");

            var engineThread = new Thread(TheGame.Play) { IsBackground = false };
   		    engineThread.Start();

            app.MapGet("/", () => Results.Ok("API is running"));
            app.MapMethods("/", new[] { "HEAD" }, () => Results.Ok());


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseCors("AllowSubdomains");
            app.UseRouting();
            app.UseAuthorization();
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
