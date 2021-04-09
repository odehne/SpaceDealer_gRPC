using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using System;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Galaxy.API.Model;
using System.Threading.Tasks;
using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Cope.SpaceRogue.Galaxy.Creator
{
	public class Menu
	{
        private readonly IMediator _mediator;
        private readonly ILogger<Menu> _logger;
        private readonly IShipRepository _shipRepo;
        private readonly GalaxyDbContext _context;

		public Menu(GalaxyDbContext context, IShipRepository shipRepo) 
		{
            _shipRepo = shipRepo;
            _context = context;
		}

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
            Console.Write("Name des zu aktualisierenden Produktes: ");
            var line = Console.ReadLine();
            var prod = await Factory.ProductRepository.GetItemByName(line);
            if(prod==null)
            {
                Console.WriteLine(prod + " nicht gefunden.");
                return;
            }
            Console.Write($"Neuer Name [{prod.Name}]: ");
            var newName = Console.ReadLine();
            if(!string.IsNullOrEmpty(newName))
                prod.Name = newName;
            
            Console.Write($"Neuer Preis pro Einheit [{prod.PricePerUnit}]: ");
            var newPricePerUnit = Console.ReadLine();
            if(!string.IsNullOrEmpty(newPricePerUnit))
                prod.PricePerUnit = double.Parse(newPricePerUnit);
            
            Console.Write($"Neue Seltenheit [{prod.Rarity}]: ");
            var newRarity = Console.ReadLine();
            if(!string.IsNullOrEmpty(newRarity))
                prod.Rarity = double.Parse(newRarity);
            
            Console.Write($"Neue Größe pro Einheit [{prod.SizeInUnits}]: ");
            var newSize = Console.ReadLine();
            if(!string.IsNullOrEmpty(newSize))
                prod.SizeInUnits = double.Parse(newSize);
            var result = await Factory.ProductRepository.UpdateItem(prod);
        }

        private async Task DeleteProduct()
        {
            Console.Write("Name des zu löschenden Produktes: ");
            var line = Console.ReadLine();
            var prod = await Factory.ProductRepository.GetItemByName(line);
            if(prod==null)
            {
                Console.WriteLine(prod + " nicht gefunden.");
                return;
            }
            var result = await Factory.ProductRepository.DeleteItem(prod);
        }

        private async Task DeleteProductGroup()
        {
             Console.Write("Name der zu löschenden Produktgruppe: ");
            var line = Console.ReadLine();
           var prod = await Factory.ProductGroupRepository.GetItemByName(line);
            if(prod==null)
            {
                Console.WriteLine(prod + " nicht gefunden.");
                return;
            }
            var result = await Factory.ProductGroupRepository.DeleteItem(prod);
        }

        private async Task ListProducts()
        {
            var lst = await Factory.ProductRepository.GetItems();
            var i=0;
            foreach (var prod in lst)
            {
                i++;
                Console.WriteLine($"{i}. {prod.Name} [t/unit: {prod.SizeInUnits}, Selten: {prod.Rarity}, Preis: {prod.PricePerUnit}]");
            }
         }

        private async Task<bool> ListProductGroups()
        {
             var lst = await Factory.ProductGroupRepository.GetItems();
            var i=0;
            foreach (var group in lst)
            {
                i++;
                Console.WriteLine($"{i}. {group.Name}");
            }
            return true;
        }

        private async Task AddProductGroup()
        {
            Console.Write("Name des neuen Produktgruppe: ");
            var line = Console.ReadLine();
            var group = new ProductGroup(line);
            var command = new AddProductGroupCommand(group.ID.ToString(), group.Name);
            await _mediator.Send(command);
        }

        private async Task AddProduct()
        {
            Console.Write("Name der zugehörigen Produktgruppe: ");
            var groupName = Console.ReadLine();
            var group = await Factory.ProductGroupRepository.GetItemByName(groupName);
            if(group==null){
                Console.WriteLine($"Gruppe {groupName} nicht gefunden.");
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
            var product = new Product(prodName, group.ID, double.Parse(tonsPerUnit), double.Parse(rarity), double.Parse(price));
             var result = await Factory.ProductRepository.AddItem(product);
        }

        private async Task DeletePlanet()
        {
            Console.Write("Name des zu löschenden Planeten: ");
            var line = Console.ReadLine();
             var planet = await Factory.PlanetRepository.GetItemByName(line);
            if(planet==null)
            {
                Console.WriteLine(planet + " nicht gefunden.");
                return;
            }
            var result = await Factory.PlanetRepository.DeleteItem(planet);
        }

        private async Task ListPlanets()
        {
            var lst = await Factory.PlanetRepository.GetItems();
            var i=0;
            foreach (var planet in lst)
            {
                i++;
                Console.WriteLine($"{i}. {planet.Name} [{planet.ID}]");
            }
            i++;
            Console.WriteLine($"{i}. Zurück");
            Console.Write("Details oder zurück: ");
            var line = Console.ReadLine();
            var selected = int.Parse(line);
            if( selected >= i)
                return;
             ShowPlanetDetails(lst[selected-1]);
        }

        private void ShowPlanetDetails(Planet planet)
        {
            Console.WriteLine($"Planet {planet.ID}");
            Console.WriteLine($"Name: {planet.Name}");
            Console.WriteLine($"Markt: {planet.Market.Name}");
            Console.WriteLine($"Angebot: {planet.Market.ProductOfferings.ToString()}");
            Console.WriteLine($"Nachfrage: {planet.Market.ProductDemands.ToString()}");
        }

        private async Task<string> AddNewPlanet()
        {
			Console.Write("Name des neuen Planeten: ");
			var name = Console.ReadLine();
			var position = GetPlanetPosition();
			
			Console.Write("Name des neuen Marktplatzes [Planetenname]: ");
			var marketPlaceName = Console.ReadLine();
			if(string.IsNullOrEmpty(marketPlaceName))
				marketPlaceName = "Markt auf " + name;

			Console.WriteLine("Produktgruppe, die benötigt wird: ");
			var pgDemands = await SelectProductGroup();
			
			Console.WriteLine("Produktgruppe, die verkauft wird: ");
			var pgOfferings = await SelectProductGroup();

			var offerings = new Catalog();
			var demands = new Catalog();
			
			foreach (var product in pgDemands.Products)
			{
				demands.AddCatalogItem(product, product.Name, 15);
			}
			foreach (var product in pgOfferings.Products)
			{
				offerings.AddCatalogItem(product, product.Name, -5);
			}

			var market = new MarketPlace(marketPlaceName, offerings, demands);
			await Factory.MarketPlaceRepository.AddItem(market);
			var planet = new Planet(market, name, "Neuer Planet entdeckt!", (int)position.X, (int)position.Y, (int)position.Z);
			await Factory.PlanetRepository.AddItem(planet); 
			
			return $"{planet.Name} erstellt.";
		}

        private async Task<ProductGroup> SelectProductGroup()
        {
            var lst = await Factory.ProductGroupRepository.GetItems();
			var i = 0;
			foreach (var group in lst)
			{
				i++;
				Console.WriteLine($"{i}. {group.Name}");
			}
			Console.Write("Bitte Gruppe auswählen: ");
            var selectedProduct = Console.ReadLine();
			return lst[int.Parse(selectedProduct)-1];
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
