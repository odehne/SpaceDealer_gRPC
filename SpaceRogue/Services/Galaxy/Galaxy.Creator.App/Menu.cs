using System;
using System.Linq;
using System.Threading.Tasks;
using Cope.SpaceRogue.Galaxy.API.Proto;
using Galaxy.Creator.App;
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
				Console.WriteLine("6.  Planeten löschen");
				Console.WriteLine("7.  Produktgruppen anzeigen");
				Console.WriteLine("8.  Produkte anzeigen");
				Console.WriteLine("9.  Produktgruppe löschen");
				Console.WriteLine("10. Produkt löschen");
				Console.WriteLine("11. Produkt aktualisieren");
                Console.WriteLine("12. Raumschiff erstellen");
                Console.WriteLine("13. Beenden");

				Console.Write("Wähle zwischen 1-13: ");
				var selection = Console.ReadLine();
				if(selection=="13")
					exitRecieved=true;
		    	if(selection=="1")
					result = await AddNewPlanet();
               if(selection=="3")
					await AddProductGroup();
			    if(selection=="4")
					await AddProduct();
			    if(selection=="5")
					await ListPlanets();
			    if(selection=="6")
					await DeletePlanet();
			    if(selection=="7")
					await ListProductGroups();
			    if(selection=="8")
					await ListProducts();
			    if(selection=="9")
					await DeleteProductGroup();
			    if(selection=="10")
					await DeleteProduct();
			    if(selection=="11")
					await UpdateProduct();
				if(selection=="12")
					result = AddNewShip();
       		Console.WriteLine(result);
			} while (!exitRecieved);
       }

       private string AddNewShip()
       {
           throw new NotImplementedException();
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

        private async Task ListProducts()
       {
            var reply = await Factory.MarketPlacessApiClient.GetProductsAsync(new Empty());
            var i = 0;
            foreach (var prod in reply.Products)
            {
                i++;
                Console.WriteLine($"{i}. [{prod.Id}] {prod.Name}");
            }
            
        }

        private async Task ListProductGroups()
        {
            var reply = await Factory.MarketPlacessApiClient.GetProductGroupsAsync(new Empty());
            reply.ProductGroups.ToList().ForEach(x => Console.WriteLine($"[{x.Id}] {x.Name}"));
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

       private async Task ListPlanets()
       {
			var reply = await Factory.PlanetsApiClient.GetPlanetsAsync(new PlanetsEmpty());

			var i = 0;
			foreach (var planet in reply.Planets)
			{
				i++;
				Console.WriteLine($"{i}. {planet.Name} [{planet.Id}]");
			}
			i++;
			Console.WriteLine($"{i}. Zurück");
			Console.Write("Details oder zurück: ");
			var line = Console.ReadLine();
			var selected = int.Parse(line);
			if (selected >= i)
				return;
			//ShowPlanetDetails(reply.Planets[selected-1].Id);
	   }

		//private void ShowPlanetDetails(string planetId)
		//{
		//	var reply = await Factory.MarketPlacessApiClient.Get(new PlanetsEmpty());

		//	Console.WriteLine($"Planet {planet.Id}");
		//	Console.WriteLine($"Name: {planet.Name}");
		//	Console.WriteLine($"Markt: {planet.Market.Name}");
		//	Console.WriteLine($"Angebot: {planet.Market.ProductOfferings.ToString()}");
		//	Console.WriteLine($"Nachfrage: {planet.Market.ProductDemands.ToString()}");
		//}

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

			var offerings = new AddCatalogRequest();
			var demands = new AddCatalogRequest();

			foreach (var product in pgDemands.Products)
			{
				demands.CatalogItems.Add(new AddCatalogItemRequest { Id = Guid.NewGuid().ToString(), ProductId = product.Id, PricePercentDelta = 15, Title = product.Name });
			}
			foreach (var product in pgOfferings.Products)
			{
				offerings.CatalogItems.Add(new AddCatalogItemRequest { Id = Guid.NewGuid().ToString(), ProductId = product.Id, PricePercentDelta = -5, Title = product.Name });
			}

			var market = new AddMarketPlaceRequest { Id = Guid.NewGuid().ToString(), MarketPlaceId = Guid.NewGuid().ToString(), Offerings = offerings, Demands = demands, Name = marketPlaceName };

			//var ret = 

			//await Factory.MarketPlaceRepository.AddItem(market);
			//var planet = new Planet(market, name, "Neuer Planet entdeckt!", (int)position.X, (int)position.Y, (int)position.Z);
			//await Factory.PlanetRepository.AddItem(planet);
			//return $"{planet.Name} erstellt.";

			return "OK";
			
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
