using Cope.SpaceRogue.Galaxy.API.Proto;
using Cope.SpaceRogue.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Creator.App
{
	public static class AssetGenerator
	{
		public enum AssetTypes
		{
			Products,
			Players,
			Ships,
			Planets
		}

		public static List<string> ShipNames { get; set; }
		public static List<string> PlanetNames { get; set; }
		public static List<string> PlayerNames { get; set; }

		private async static Task<List<string>> LoadNamesFile(string filename)
		{
			var lst = new List<string>();
			using var reader = new StreamReader(filename);
			var line = await reader.ReadLineAsync().ConfigureAwait(false);
			{
				lst.Add(line);
			}
			return lst;
		}

		public static async Task LoadItemFile(AssetTypes assetType)
		{
			switch (assetType)
			{
				case AssetTypes.Products:
					await LoadProductsFile();
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

		private static async Task LoadPlanetsFile()
		{
			PlanetNames = await LoadNamesFile(Program.GetAppLocation() + "\\PlanetNames.txt");
			var newSector = Position.GetRandomSector();
		}

		private static async Task LoadPlayersFile()
		{
			ShipNames = await LoadNamesFile(Program.GetAppLocation() + "\\ShipNames.txt");
			PlayerNames = await LoadNamesFile(Program.GetAppLocation() + "\\PlayerNames.txt");

		}

		private static async Task LoadProductsFile()
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
