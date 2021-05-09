using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cope.SpaceRogue.Galaxy.API.Proto;
using Galaxy.Creator.App;
using Galaxy.Creator.App.Model;
using Microsoft.Extensions.Logging;

namespace Cope.SpaceRogue.Galaxy.Creator.App
{
	public class Menu
	{
       private readonly ILogger<Menu> _logger;
	   

		public async Task ShowMenu()
       {
			var exitRecieved = false;
			do
			{
				var result = "";
				Console.WriteLine("Hauptmenü");
				Console.WriteLine("1.  Neuer Planet");
				Console.WriteLine("2.  Neues Raumschiff Feature");
				Console.WriteLine("3.  Neue Produktgruppe");
				Console.WriteLine("4.  Neues Produkt");
				Console.WriteLine("5.  Planeten anzeigen");
				Console.WriteLine("6.  Spieler anzeigen");
				Console.WriteLine("7.  Raumschiffe anzeigen");
				Console.WriteLine("8.  Produkte anzeigen");
				Console.WriteLine("9.  Produktgruppen anzeigen");
				Console.WriteLine("10.  Planeten löschen");
				Console.WriteLine("11.  Produktgruppe löschen");
				Console.WriteLine("12. Produkt löschen");
				Console.WriteLine("13. Produkt aktualisieren");
                Console.WriteLine("14. Raumschiff erstellen");
				Console.WriteLine("15. Spieler erstellen");
				Console.WriteLine("16. Beenden");

				Console.Write("Wähle zwischen 1-16: ");
				var selection = Console.ReadLine();
				if(selection=="14")
					exitRecieved=true;
		    	if(selection=="1")
					result = await AddNewPlanet();
               if(selection=="3")
					await AddProductGroup();
			    if(selection=="4")
					await AddProduct();
			    if(selection=="5")
					ListPlanets();
				if (selection == "6")
					ListPlayers();
				if (selection == "7")
					ListShips();
				if (selection=="8")
					ListProducts();
				if (selection=="9")
					ListProductGroups();
			    if(selection=="10")
					await DeletePlanet();
			    if(selection=="11")
					await DeleteProductGroup();
			    if(selection=="12")
					await DeleteProduct();
			    if(selection=="13")
					await UpdateProduct();
				if(selection=="14")
					await AddNewShip();
				if (selection == "15")
					await AddNewPlayer();
				Console.WriteLine(result);
			} while (!exitRecieved);
       }

		private async Task AddNewPlayer()
		{
			Console.Write("Name des neuen Spielers: ");
			var name = Console.ReadLine();
			var homePlanetId = GalaxyModel.GetRandomPlanet().Id;
			var reply = await Factory.PlanetsApiClient.PlayerNameTakenAsync(new GetPlayerByNameRequest { Name = name });

			if (reply.Taken)
			{
				Console.WriteLine($"Ein Spieler mit dem Namen existiert bereits.");
			}
			else
			{
				Console.Write("Spieler [0] oder NichtSpieler [1]: ");
				var line = Console.ReadLine();
				int playerType;
				int.TryParse(line, out playerType);

				var p = await Factory.PlanetsApiClient.AddPlayerAsync(new AddPlayerRequest
				{
					Id = Guid.NewGuid().ToString(),
					Name = name,
					Credits = 5000,
					PlanetId = homePlanetId,
					PlayerType = playerType
				});

			}
		}


		private async Task AddNewShip()
        {
			Console.Write("Player Id: ");
			var line = Console.ReadLine();
			var player = await Factory.PlanetsApiClient.GetPlayerAsync(new GetPlayerRequest { Id = line });

			if (player == null)
			{
				Console.WriteLine("Spieler nicht gefunden.");
				return;
			}

			Console.Write("Name des Schiffes: ");
			var shipName = Console.ReadLine();
			var b = await Factory.PlanetsApiClient.AddShipAsync(new AddShipRequest { PlayerId = player.Id, Id = Guid.NewGuid().ToString(), Hull = 3, Shields = 3, Name = shipName });

			Console.WriteLine($"Neues Schiff erzeugt: {b}.");
		}

		private async Task UpdateProduct()
       {
			Console.Write("Produkt Id: ");
			var line = Console.ReadLine();
			var prod = await Factory.MarketPlacessApiClient.GetProductAsync(new GetProductRequest { Id = line });
       
            if (prod == null)
			{
				Console.WriteLine(prod + " nicht gefunden.");
				return;
			}

            var request = new UpdateProductRequest { Id = prod.Id, Name = prod.Name, PricePerUnit = prod.PricePerUnit, Rarity = prod.Rarity, SizeInUnits = prod.SizeInUnits };

            Console.Write($"Neuer Name [{request.Name}]: ");
			var newName = Console.ReadLine();
			if (!string.IsNullOrEmpty(newName))
                request.Name = newName;

			Console.Write($"Neuer Preis pro Einheit [{request.PricePerUnit}]: ");
			var newPricePerUnit = Console.ReadLine();
			if (!string.IsNullOrEmpty(newPricePerUnit))
                request.PricePerUnit = double.Parse(newPricePerUnit);

			Console.Write($"Neue Seltenheit [{request.Rarity}]: ");
			var newRarity = Console.ReadLine();
			if (!string.IsNullOrEmpty(newRarity))
                request.Rarity = double.Parse(newRarity);

			Console.Write($"Neue Größe pro Einheit [{request.SizeInUnits}]: ");
			var newSize = Console.ReadLine();
			if (!string.IsNullOrEmpty(newSize))
                request.SizeInUnits = double.Parse(newSize);

            var reply = await Factory.MarketPlacessApiClient.UpdateProductAsync(request);

    	}

       private async Task DeleteProduct()
       {
			Console.Write("Id des zu löschenden Produktes: ");
			var line = Console.ReadLine();
            var deleteProductRequest = new GetProductRequest { Id = line };
            var result = await Factory.MarketPlacessApiClient.DeleteProductAsync(deleteProductRequest);
		}

		private async Task DeleteProductGroup()
        {
            Console.Write("Id der zu löschenden Produktegruppe: ");
            var line = Console.ReadLine();
            var deleteProductRequest = new GetProductGroupRequest { Id = line };
            var result = await Factory.MarketPlacessApiClient.DeleteProductGroupAsync(deleteProductRequest);
        }

      
       private async Task AddProductGroup()
       {
           Console.Write("Name des neuen Produktgruppe: ");
           var line = Console.ReadLine();
           var reply = await Factory.MarketPlacessApiClient.AddProductGroupAsync(new AddProductGroupRequest());
       }

       private async Task AddProduct()
       {
			Console.Write("Produktgruppe ID: ");
			var groupId = Console.ReadLine();
			var group = await Factory.MarketPlacessApiClient.GetProductGroupAsync(new GetProductGroupRequest { Id = groupId });
			if (group == null)
			{
				Console.WriteLine($"Gruppe {groupId} nicht gefunden.");
				return;
			}
			Console.Write("Name des neuen Produktes: ");
			var prodName = Console.ReadLine();
			Console.Write("Tonnen pro Einheit: ");
			var tonsPerUnit = Console.ReadLine();
			Console.Write("Seltenheit [10000-nicht selten, 1-sehr selten]: ");
			var rarity = Console.ReadLine();
			Console.Write("Preis pro Tonne: ");
			var price = Console.ReadLine();
			var addProductRequest = new AddProductRequest { 
											Id = Guid.NewGuid().ToString(), 
											Name = prodName, 
											GroupId = group.Id, 
											PricePerUnit = double.Parse(price), 
											Rarity = int.Parse(rarity), 
										 	Capacity =  double.Parse(tonsPerUnit) };
			var result = await Factory.MarketPlacessApiClient.AddProductAsync(addProductRequest);
		}

        private async Task DeletePlanet()
        {
			Console.Write("Id des zu löschenden Planeten: ");
			var planetId = Console.ReadLine();
			var planet = await Factory.PlanetsApiClient.GetPlanetAsync(new GetPlanetRequest { Id = planetId });
			if (planet == null)
			{
				Console.WriteLine(planetId + " nicht gefunden.");
				return;
			}
			var result = await Factory.PlanetsApiClient.DeletePlanetAsync(new DeletePlanetRequest { Id = planetId });
		}

       private void ListPlanets()
       {
			var i = 0;
			foreach (var planet in GalaxyModel.Planets)
			{
				i++;
				Console.WriteLine($"{i}. {planet.Name} [{planet.Id}]");
			}
	   }

		private void ListPlayers()
		{
			var i = 0;
			foreach (var player in GalaxyModel.Players)
			{
				i++;
				Console.WriteLine($"{i}. {player.Name} [{player.Id}]");
			}
		}

		private void ListShips()
		{
			var i = 0;
			foreach (var ship in GalaxyModel.Ships)
			{
				i++;
				Console.WriteLine($"{i}. {ship.Name} [{ship.Id}]");
			}
		}

		private void ListProducts()
		{
			var i = 0;
			foreach (var p in GalaxyModel.Products)
			{
				i++;
				Console.WriteLine($"{i}. {p.Name} [{p.Id}]");
			}
		}

		private void ListProductGroups()
		{
			var i = 0;
			foreach (var pg in GalaxyModel.ProductGroups)
			{
				i++;
				Console.WriteLine($"{i}. {pg.Name} [{pg.Id}]");
			}
		}


		private async Task LoadPlanets()
		{
			await GalaxyModel.Load();
		}

		private async Task<string> AddNewPlanet()
		{

			Console.Write("Name des neuen Planeten: ");
			var name = Console.ReadLine();
			var position = GetPlanetPosition();

			Console.Write("Name des neuen Marktplatzes [Planetenname]: ");
			var marketPlaceName = Console.ReadLine();
			if (string.IsNullOrEmpty(marketPlaceName))
				marketPlaceName = "Markt auf " + name;

			Console.WriteLine("Produktgruppe, die benötigt wird: ");
			var pgDemands = await SelectProductGroup();

			Console.WriteLine("Produktgruppe, die verkauft wird: ");
			var pgOfferings = await SelectProductGroup();

			var offerings = new AddCatalogRequest { Id = Guid.NewGuid().ToString() };
			var demands = new AddCatalogRequest { Id = Guid.NewGuid().ToString() };

			foreach (var product in pgDemands.Products)
			{
				var price = CalculatePriceDiff(product.PricePerUnit, 15);
				demands.CatalogItems.Add(new AddCatalogItemRequest { Id = Guid.NewGuid().ToString(), ProductId = product.Id, Price = price, Title = product.Name });
			}
			foreach (var product in pgOfferings.Products)
			{
				var price = CalculatePriceDiff(product.PricePerUnit, 15);
				offerings.CatalogItems.Add(new AddCatalogItemRequest { Id = Guid.NewGuid().ToString(), ProductId = product.Id, Price = price, Title = product.Name });
			}

			var market = new AddMarketPlaceRequest { Id = Guid.NewGuid().ToString(), MarketPlaceId = Guid.NewGuid().ToString(), Offerings = offerings, Demands = demands, Name = marketPlaceName };

			var marketPlaceReply = await Factory.MarketPlacessApiClient.AddMarketPlaceAsync(market);

			var addPlanetRequest = new AddPlanetRequest { Name = name, PosX = (int)position.X, PosY = (int)position.Y, PosZ = (int)position.Z, MarketPlaceId = marketPlaceReply.Id };

			var planetReply = await Factory.PlanetsApiClient.AddPlanetAsync(addPlanetRequest);

			GalaxyModel.Planets.Add(new PlanetModel { Id = planetReply.Id, MarketId = marketPlaceReply.Id, Name = name, Sector = new Position(position.X, position.Y, position.Z) });
			
			return $"{planetReply.Id} erstellt.";

		}


		private double CalculatePriceDiff(double orgPrice, int percentValue)
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
				newPrice = orgPrice - ((orgPrice* (percentValue * -1))) / 100;
			}
			return newPrice;
		}



		private async Task<GetProductGroupReply> SelectProductGroup()
		{
			var reply = await Factory.MarketPlacessApiClient.GetProductGroupsAsync(new Empty());
			var i = 0;
			foreach (var group in reply.ProductGroups)
			{
				i++;
				Console.WriteLine($"{i}. {group.Name}");
			}
			Console.Write("Bitte Gruppe auswählen: ");
			var selectedProduct = Console.ReadLine();
			var groupId = reply.ProductGroups[int.Parse(selectedProduct) - 1].Id;
			return await Factory.MarketPlacessApiClient.GetProductGroupAsync(new GetProductGroupRequest { Id = groupId });
		}

		private Position GetPlanetPosition()
		{
			var posAdded = false;
			Position position = null;
			do
			{
				Console.Write("Position X, Y, Z: ");
				var posLine = Console.ReadLine();
				try
				{
					position = Position.GetPositionByString(posLine);
					posAdded = true;
				}
				catch (System.Exception e)
				{
					Console.WriteLine("Falsche Eingabe [" + e.Message + "]");
				}
			} while (!posAdded);
			return position;
		}
	}
}
