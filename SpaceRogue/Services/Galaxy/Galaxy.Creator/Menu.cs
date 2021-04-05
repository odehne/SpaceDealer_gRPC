using Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using System;
using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.API.Model;

namespace Cope.SpaceRogue.Galaxy.Creator
{
    public class Menu
	{
		public GalaxyDbContext Context {get; set; }
		public Menu(GalaxyDbContext context) 
		{
			Context = context;

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
				Console.WriteLine("12. Beenden");

				Console.Write("Wähle zwischen 1-12: ");
				var selection = Console.ReadLine();
				if(selection=="12")
					exitRecieved=true;
				if(selection=="1")
					result = AddNewPlanet();
                if(selection=="3")
					AddProductGroup();
			    if(selection=="4")
					AddProduct();
			    if(selection=="5")
					ListPlanets();
			    if(selection=="6")
					DeletePlanet();
			    if(selection=="7")
					ListProductGroups();
			    if(selection=="8")
					ListProducts();
			    if(selection=="9")
					DeleteProductGroup();
			    if(selection=="10")
					DeleteProduct();
			    if(selection=="11")
					UpdateProduct();
			    
				Console.WriteLine(result);
			} while (!exitRecieved);

		}

        private void UpdateProduct()
        {
            Console.Write("Name des zu aktualisierenden Produktes: ");
            var line = Console.ReadLine();
            var repo = new ProductRepository(Context);
            var prod = repo.GetItemByName(line);
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
            repo.UpdateItem(prod);
        }

        private void DeleteProduct()
        {
            Console.Write("Name des zu löschenden Produktes: ");
            var line = Console.ReadLine();
            var repo = new ProductRepository(Context);
            var prod = repo.GetItemByName(line);
            if(prod==null)
            {
                Console.WriteLine(prod + " nicht gefunden.");
                return;
            }
            repo.DeleteItem(prod);
        }

        private void DeleteProductGroup()
        {
             Console.Write("Name der zu löschenden Produktgruppe: ");
            var line = Console.ReadLine();
            var repo = new ProductGroupRepository(Context);
            var prod = repo.GetItemByName(line);
            if(prod==null)
            {
                Console.WriteLine(prod + " nicht gefunden.");
                return;
            }
            repo.DeleteItem(prod);
        }

        private void ListProducts()
        {
            var productRepo = new ProductRepository(Context);
            var lst = productRepo.GetItems();
            var i=0;
            foreach (var prod in lst)
            {
                i++;
                Console.WriteLine($"{i}. {prod.Name} [t/unit: {prod.SizeInUnits}, Selten: {prod.Rarity}, Preis: {prod.PricePerUnit}]");
            }
         }

        private void ListProductGroups()
        {
            var prodGroupRepo = new ProductGroupRepository(Context);
            var lst = prodGroupRepo.GetItems();
            var i=0;
            foreach (var group in lst)
            {
                i++;
                Console.WriteLine($"{i}. {group.Name}");
            }
        }

        private void AddProductGroup()
        {
            Console.Write("Name des neuen Produktgruppe: ");
            var line = Console.ReadLine();
            var repo = new ProductGroupRepository(Context);
            var group = new ProductGroup(line);
            repo.AddItem(group);
        }

        private void AddProduct()
        {
            Console.Write("Name der zugehörigen Produktgruppe: ");
            var groupName = Console.ReadLine();
            var repo = new ProductGroupRepository(Context);
            var group = repo.GetItemByName(groupName);
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
            var prodRepo = new ProductRepository(Context);
            prodRepo.AddItem(product);
        }

        private void DeletePlanet()
        {
            Console.Write("Name des zu löschenden Planeten: ");
            var line = Console.ReadLine();
            var repo = new PlanetRepository(Context);
            var planet = repo.GetItemByName(line);
            if(planet==null)
            {
                Console.WriteLine(planet + " nicht gefunden.");
                return;
            }
            repo.DeleteItem(planet);
        }

        private void ListPlanets()
        {
            var planetRepo = new PlanetRepository(Context);
            var lst = planetRepo.GetItems();
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
            if( selected == i)
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

        private string AddNewPlanet()
        {
			Console.Write("Name des neuen Planeten: ");
			var name = Console.ReadLine();
			var position = GetPlanetPosition();
			
			Console.Write("Name des neuen Marktplatzes [Planetenname]: ");
			var marketPlaceName = Console.ReadLine();
			if(string.IsNullOrEmpty(marketPlaceName))
				marketPlaceName = "Markt auf " + name;

			Console.WriteLine("Produktgruppe, die benötigt wird: ");
			var pgDemands = SelectProductGroup();
			
			Console.WriteLine("Produktgruppe, die verkauft wird: ");
			var pgOfferings = SelectProductGroup();

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

			var marketRepo = new MarketPlaceRepository(Context);
			var market = new MarketPlace(marketPlaceName, offerings, demands);
			marketRepo.AddItem(market);
			var planetRepo = new PlanetRepository(Context);
			var planet = new Planet(market, name, "Neuer Planet entdeckt!", (int)position.X, (int)position.Y, (int)position.Z);
			planetRepo.AddItem(planet); 
			
			return $"{planet.Name} erstellt.";
		}

        private ProductGroup SelectProductGroup()
        {
            var groupRepo = new ProductGroupRepository(Context);
			var lst = groupRepo.GetItems();
			var i = 0;
			foreach (var group in lst)
			{
				i++;
				Console.WriteLine($"{i}. {group.Name}");
			}
			Console.Write("Bitte Gruppe auswählen: ");
            var selectedProduct = Console.ReadLine();
			return lst[Int32.Parse(selectedProduct)-1];
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
