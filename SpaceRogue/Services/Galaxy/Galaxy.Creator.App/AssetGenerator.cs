using Cope.SpaceRogue.Galaxy.API.Proto;
using Cope.SpaceRogue.Infrastructure;
using Galaxy.Creator.App.Model;
using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Creator.App
{
	public class AssetGenerator
	{
		public enum AssetTypes
		{
			Products,
			Players,
			Ships,
			Planets
		}

		public const int NPC = 1;

		public static List<string> ShipNames { get; set; }
		public static List<string> PlanetNames { get; set; }
		public static List<string> PlayerNames { get; set; }
		public static List<string> SatelliteNames { get; set; }


		private async Task<List<string>> LoadNamesFile(string filename)
		{
			var lst = new List<string>();
			using var reader = new StreamReader(filename);
			string line;
			while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
			{
				lst.Add(line);
			}
			return lst;
		}

		public async Task LoadItemFile(AssetTypes assetType)
		{
			switch (assetType)
			{
				case AssetTypes.Products:
					await LoadProductsFile();
					break;
				case AssetTypes.Ships:
					await LoadShipsFile();
					await LoadSatellitesFile();
					break;
				case AssetTypes.Players:
					await LoadPlayersFile();
					break;
				case AssetTypes.Planets:
					await LoadPlanetsFile();
					break;
				default:
					Console.WriteLine("Unbekanntes Kommando.");
					break;
			}
		}

		private async Task LoadPlanetsFile()
		{
			PlanetNames = await LoadNamesFile(Program.GetAppLocation() + "\\PlanetNames.txt");

			foreach (var planetName in PlanetNames)
			{
				if (!GalaxyModel.Planets.Any(x => x.Name.Equals(planetName)))
				{

					var pgs = GetRandomDemandsAndOfferings();
					var pgDemands = pgs.Item1;
					var pgOfferings = pgs.Item2;
					var position = Position.GetRandomSector();
					var marketPlaceName = GetRandomMarketPlaceName(planetName);

					var offerings = new AddCatalogRequest { Id = Guid.NewGuid().ToString() };
					var demands = new AddCatalogRequest { Id = Guid.NewGuid().ToString() };

					foreach (var product in pgDemands.Products)
					{
						var price = GalaxyModel.CalculatePriceDiff(product.PricePerUnit, 15);
						demands.CatalogItems.Add(new AddCatalogItemRequest { Id = Guid.NewGuid().ToString(), ProductId = product.Id, Price = price, Title = product.Name });
					}
					foreach (var product in pgOfferings.Products)
					{
						var price = GalaxyModel.CalculatePriceDiff(product.PricePerUnit, 15);
						offerings.CatalogItems.Add(new AddCatalogItemRequest { Id = Guid.NewGuid().ToString(), ProductId = product.Id, Price = price, Title = product.Name });
					}

					var market = new AddMarketPlaceRequest { Id = Guid.NewGuid().ToString(), MarketPlaceId = Guid.NewGuid().ToString(), Offerings = offerings, Demands = demands, Name = marketPlaceName };

					var marketPlaceReply = await Factory.MarketPlacessApiClient.AddMarketPlaceAsync(market);

					var addPlanetRequest = new AddPlanetRequest { Name = planetName, PosX = position.X, PosY = position.Y, PosZ = position.Z, MarketPlaceId = marketPlaceReply.Id };

					var planetReply = await Factory.PlanetsApiClient.AddPlanetAsync(addPlanetRequest);

					GalaxyModel.Planets.Add(new PlanetModel { Id = planetReply.Id, MarketId = marketPlaceReply.Id, Name = planetName, Sector = new Position(position.X, position.Y, position.Z) });
					Console.WriteLine($"[{planetName}] hinzugefügt.");
				}
				else
				{
					Console.WriteLine($"[{planetName}] schon bekannt.");
				}
			}

		}

		private  string GetRandomMarketPlaceName(string planetName)
		{
			return $"Markt von {planetName}";
		}


		public Tuple<ProductGroupModel, ProductGroupModel> GetRandomDemandsAndOfferings()
		{
			var randomDemand = GalaxyModel.ProductGroups[SimpleDiceRoller.GetRandomNumber(GalaxyModel.ProductGroups.Count - 1)];
			var randomOfferings = GalaxyModel.ProductGroups[SimpleDiceRoller.GetRandomNumber(GalaxyModel.ProductGroups.Count - 1)];
			return new Tuple<ProductGroupModel, ProductGroupModel>(randomDemand, randomOfferings);
		}

		private async Task LoadShipsFile()
		{
			ShipNames = await LoadNamesFile(Program.GetAppLocation() + "\\ShipNames.txt");
		}

		private  async Task LoadSatellitesFile()
		{
			SatelliteNames = await LoadNamesFile(Program.GetAppLocation() + "\\SatelliteNames.txt");
		}

		private async Task LoadPlayersFile()
		{

			ShipNames = await LoadNamesFile(Program.GetAppLocation() + "\\ShipNames.txt");
			SatelliteNames = await LoadNamesFile(Program.GetAppLocation() + "\\SatelliteNames.txt");
			PlayerNames = await LoadNamesFile(Program.GetAppLocation() + "\\PlayerNames.txt");



			foreach (var name in PlayerNames)
			{
				if (!GalaxyModel.Players.Any(x => x.Name.Equals(name)))
				{

					var playerId = Guid.NewGuid().ToString();
					var homePlanetId = GalaxyModel.GetRandomPlanet().Id;
					var shipId = Guid.NewGuid().ToString();

					var pResult = await Factory.PlanetsApiClient.AddPlayerAsync(new AddPlayerRequest
					{
						Id = playerId,
						Name = name,
						Credits = 5000,
						PlanetId = homePlanetId,
						PlayerType = NPC
					});

					if (pResult.Ok)
					{
						var shipName = ShipNames[SimpleDiceRoller.GetRandomNumber(ShipNames.Count - 1)];
						var sResult = await Factory.PlanetsApiClient.AddShipAsync(new AddShipRequest
						{
							Hull = SimpleDiceRoller.GetRandomNumber(4),
							Id = shipId,
							Name = shipName,
							PlayerId = playerId,
							Shields = SimpleDiceRoller.GetRandomNumber(2),
							ShipType = SimpleDiceRoller.GetRandomNumber(8)
						});
						if (!sResult.Ok)
							Console.WriteLine($"Schiff {shipName} konnte nicht angelegt werden!");
						else
							Console.WriteLine($"Schiff {shipName} von {name} angelegt.");
					}
					else
						Console.WriteLine($"Player {name} konnte nicht angelegt werden!");
				}
				else
					Console.WriteLine($"Player {name} schon angelegt.");
			}
		}

		private  async Task LoadProductsFile()
		{
			var filename = Program.GetAppLocation() + "\\Products.txt";
			using var reader = new StreamReader(filename);
			var line = "";
			while ((line = reader.ReadLine()) != null)
			{
				var s = line.Split("|");
				var addProductRequest = new AddProductRequest
				{
					Id = Guid.NewGuid().ToString(),
					Name = s[0],
					GroupId = s[1],
					Capacity = double.Parse(s[2]),
					Rarity = double.Parse(s[3]),
					PricePerUnit = double.Parse(s[4])
				};
				await Factory.MarketPlacessApiClient.AddProductAsync(addProductRequest);
			}
		}

		

	}
}
