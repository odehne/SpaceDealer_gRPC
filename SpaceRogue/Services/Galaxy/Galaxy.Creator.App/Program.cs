using Cope.SpaceRogue.Galaxy.API.Proto;
using Cope.SpaceRogue.Galaxy.Creator.App;
using Cope.SpaceRogue.Infrastructure;
using Galaxy.Creator.App.Model;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Cope.SpaceRogue.Galaxy.API.Proto.MarketPlacesService;
using static Cope.SpaceRogue.Galaxy.API.Proto.PlanetsService;
using static Cope.SpaceRogue.Travelling.API.Proto.TravelService;

namespace Galaxy.Creator.App
{
	public static class Factory
	{
		//Start rabbit mq
		// docker run --rm -it --hostname my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
		// docker build -f  C:\git\priv\SpaceDealer\SpaceRogue\Services\Galaxy\Galaxy.Creator.App\Dockerfile --force-rm -t galaxy.creator  --label cope.spacerogue.galaxy.creator .
		// docker run --rm -it --hostname cope.galaxy.creator -e "galaxy_api_grpc_url=http://galaxy.api:8891" --name galaxy.creator galaxy.creator:latest


		public static TravelServiceClient TravellingApiClient
		{
			get
			{
				var serverAddress = Environment.GetEnvironmentVariable("galaxy_api_grpc_url");
				if (string.IsNullOrEmpty(serverAddress))
					serverAddress = "http://localhost:50991";
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					// The following statement allows you to call insecure services. To be used only in development environments.
					AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
					serverAddress = "http://travelling.api:50991";
				}

				var channel = GrpcChannel.ForAddress(serverAddress);
				return new TravelServiceClient(channel);
			}
		}

		public static PlanetsServiceClient PlanetsApiClient
		{
			get 
			{
				var serverAddress = Environment.GetEnvironmentVariable("galaxy_api_grpc_url");
				if (string.IsNullOrEmpty(serverAddress))
					serverAddress = "http://localhost:8892";
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					// The following statement allows you to call insecure services. To be used only in development environments.
					AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
					serverAddress = "http://galaxy.api:8892";
				}
				var channel = GrpcChannel.ForAddress(serverAddress);
				return new PlanetsServiceClient(channel);
			}
		
		}
		public static MarketPlacesServiceClient MarketPlacessApiClient
		{
			get 
			{
				var serverAddress = Environment.GetEnvironmentVariable("galaxy_api_grpc_url");
				if (string.IsNullOrEmpty(serverAddress))
					serverAddress = "http://localhost:8892";
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					// The following statement allows you to call insecure services. To be used only in development environments.
					AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
					serverAddress = "http://galaxy.api:8892";
				}
				var channel = GrpcChannel.ForAddress(serverAddress);
				return new MarketPlacesServiceClient(channel);
			}
		}
	}

	public static class GalaxyModel
	{
		public static List<PlanetModel> Planets { get; set; }
		public static List<PlayerModel> Players { get; set; }
		public static List<ShipModel> Ships { get; set; }
		public static List<ProductGroupModel> ProductGroups { get; set; }
		public static List<ProductModel> Products { get; set; }

		public static PlanetModel GetRandomPlanet()
		{
			var random = new Random();
			var i = random.Next(0, Planets.Count -1);
			return Planets[i];				
		}

		public static async Task Load()
		{
			var planetsReply = await Factory.PlanetsApiClient.GetPlanetsAsync(new PlanetsEmpty());

			Planets = new List<PlanetModel>();
			foreach (var planet in planetsReply.Planets)
			{
				var model = new PlanetModel
				{
					Id = planet.Id,
					Name = planet.Name,
					Sector = new Position(planet.PosX, planet.PosY, planet.PosZ),
					MarketId = planet.MarketPlaceId
				};
				Planets.Add(model);
			}

			var productGroupsReply = await Factory.MarketPlacessApiClient.GetProductGroupsAsync(new Empty());

			ProductGroups = new List<ProductGroupModel>();
			foreach (var pg in productGroupsReply.ProductGroups)
			{
				var model = new ProductGroupModel
				{
					Id = pg.Id,
					Name = pg.Name
				};

				model.Products = new List<ProductModel>();
				foreach (var p in pg.Products)
				{
					var product = new ProductModel
					{
						Id = p.Id,
						Name = p.Name,
						Price = p.PricePerUnit,
						Rarity = p.Rarity,
						Size = p.SizeInUnits,
						PricePerUnit = p.PricePerUnit
					};
					model.Products.Add(product);
				}

				ProductGroups.Add(model);
			}

			var productsReply = await Factory.MarketPlacessApiClient.GetProductsAsync(new Empty());

			Products = new List<ProductModel>();
			foreach (var p in productsReply.Products)
			{
				var model = new ProductModel
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.PricePerUnit,
					Rarity = p.Rarity,
					Size = p.SizeInUnits,
					PricePerUnit = p.PricePerUnit
				};
				Products.Add(model);
			}

			var shipsReply = await Factory.PlanetsApiClient.GetShipsAsync(new PlanetsEmpty());

			Ships = new List<ShipModel>();
			foreach (var ship in shipsReply.Ships)
			{
				var model = new ShipModel
				{
					Id = ship.Id,
					Name = ship.Name,
					Hull = ship.Hull,
					Shields = ship.Shields,
					PlayerId = ship.PlayerId
				};
				foreach (var feat in ship.Features)
				{
					model.Features.Add(new FeatureModel(feat.Id, feat.Name, feat.Description, feat.BattleAdvantage, feat.BattleDisadvantage, feat.FreightCapacityAdvantage, feat.FreightCapacityDisadvantage, feat.SensorRangeAdvantage));
				}
				Ships.Add(model);
			}

			var playersReply = await Factory.PlanetsApiClient.GetPlayersAsync(new PlanetsEmpty());

			Players = new List<PlayerModel>();
			foreach (var player in playersReply.Players)
			{
				var model = new PlayerModel
				{
					Id = player.Id,
					Name = player.Name,
					Credits = player.Credits,
					PlayerType = player.PlayerType,
					HomePlanet = new PlanetModel() 
										{ 
											Id = player.HomePlanet.Id, 
											Name = player.HomePlanet.Name, 
											MarketId = player.HomePlanet.MarketPlaceId, 
											Sector = new Position(player.HomePlanet.PosX, player.HomePlanet.PosY, player.HomePlanet.PosZ) 
										}
				};
				foreach (var shipId in player.ShipIds)
				{
					model.Ships.Add(Ships.FirstOrDefault(x => x.Id.Equals(shipId)));
				}
				Players.Add(model);
			}
		}



		public static double CalculatePriceDiff(double orgPrice, int percentValue)
		{
			var newPrice = 0.0;
			if (percentValue == 100)
				return orgPrice;

			if (percentValue > 0)
			{
				newPrice = ((orgPrice * percentValue) / 100) + orgPrice;
			}
			else
			{
				newPrice = orgPrice - ((orgPrice * (percentValue * -1))) / 100;
			}
			return newPrice;
		}

	}

	public class Program
	{
		static async Task Main(string[] args)
		{
			await GalaxyModel.Load();

			//var ship = await Factory.PlanetsApiClient.GetShipAsync(new GetShipRequest { Id = "1fb93d11-a8f6-425d-a197-d1be696bfb02" });
			//var planet = await Factory.PlanetsApiClient.GetPlanetAsync(new GetPlanetRequest { Id = "75e78e83-505e-4df6-9cb9-e37546627f92" });
			//var journey = await Factory.TravellingApiClient.StartTravelAsync(new Cope.SpaceRogue.Travelling.API.Proto.StartTravelRequest
			//{
			//	ShipId = ship.Id,
			//	TargetPosX = planet.PosX,
			//	TargetPosY = planet.PosY,
			//	TargetPosZ = planet.PosZ
			//});
			//Console.WriteLine(journey.Message);

			var menu = new Menu();
			await menu.ShowMenu();

			Console.WriteLine("Hit any key to exit");
			Console.ReadKey();
		}

		public static string GetAppLocation()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}
	}
}
