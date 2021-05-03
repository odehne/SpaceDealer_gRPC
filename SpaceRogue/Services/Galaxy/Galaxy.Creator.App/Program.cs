using Cope.SpaceRogue.Galaxy.API.Proto;
using Cope.SpaceRogue.Galaxy.Creator.App;
using Galaxy.Creator.App.Model;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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

	public static class CachedGalaxy
	{
		public static List<PlanetModel> Planets { get; set; }
		public static List<PlayerModel> Players { get; set; }
		public static List<ShipModel> Ships { get; set; }
		public static List<ProductGroupModel> ProductGroups { get; set; }
		public static List<ProductModel> Products { get; set; }

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
					Size = p.SizeInUnits
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
					Shields = ship.Shields
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
					Credits = player.Credits
				};
				foreach (var shipId in player.ShipIds)
				{
					model.Ships.Add(Ships.FirstOrDefault(x => x.Id.Equals(shipId)));
				}
				Players.Add(model);
			}
		}
	}

	public class Program
	{
		static async Task Main(string[] args)
		{
			await CachedGalaxy.Load();
						
			var menu = new Menu();
			await menu.ShowMenu();

			Console.WriteLine("Hit any key to exit");
			Console.ReadKey();
		}
	}
}
