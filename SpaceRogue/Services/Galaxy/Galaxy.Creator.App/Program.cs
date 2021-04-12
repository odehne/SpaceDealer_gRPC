using Cope.SpaceRogue.Galaxy.Creator.App;
using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Cope.SpaceRogue.Galaxy.API.Proto.MarketPlacesService;
using static Cope.SpaceRogue.Galaxy.API.Proto.PlanetsService;

namespace Galaxy.Creator.App
{
	public static class Factory
	{
		public static PlanetsServiceClient PlanetsApiClient
		{
			get 
			{
				var serverAddress = "http://localhost:8891";
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					// The following statement allows you to call insecure services. To be used only in development environments.
					AppContext.SetSwitch(
						"System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
					serverAddress = "http://localhost:8891";
				}

				var channel = GrpcChannel.ForAddress(serverAddress);
				return new PlanetsServiceClient(channel);
			}
		
		}
		public static MarketPlacesServiceClient MarketPlacessApiClient
		{
			get 
			{
				var serverAddress = "http://localhost:8891";
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					// The following statement allows you to call insecure services. To be used only in development environments.
					AppContext.SetSwitch(
						"System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
					serverAddress = "http://localhost:8891";
				}

				var channel = GrpcChannel.ForAddress(serverAddress);
				return new MarketPlacesServiceClient(channel);
			}
		
		}

		
	}
	public class Program
	{
		static async Task Main(string[] args)
		{
			var reply = await Factory.PlanetsApiClient.GetPlanetsAsync(new Cope.SpaceRogue.Galaxy.API.Proto.GetPlanetsRequest());
			foreach (var planet in reply.Planets)
			{
				Console.WriteLine($"[{planet.Id}]\t{planet.Name}");
			}

			var menu = new Menu();
			await menu.ShowMenu();

			Console.WriteLine("Hit any key to exit");
			Console.ReadKey();
		}
	}
}
