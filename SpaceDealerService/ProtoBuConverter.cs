using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceDealerModels;

namespace SpaceDealerService
{
	public static class ProtoBuConverter
	{
		public static ProductInStock ConvertToProductInStock(SpaceDealerModels.Units.ProductInStock uPis)
		{
			return new ProductInStock
			{
				ProductName = uPis.Name,
				Amount = uPis.Amount,
				AmountGeneratedPerRound = uPis.AmountGeneratedPerRound,
				PricePerTon = uPis.PricePerTon,
				TotalPrice = uPis.GetTotalPrice(),
				TotalWeight = uPis.GetTotalWeight()
			};
		}

		public static Industry ConverToIndustry(SpaceDealerModels.Units.Industry ui)
		{
			var ret = new Industry
			{
				IndustryName = ui.Name
			};

			foreach (var needed in ui.ProductsNeeded)
			{
				ret.ProductsNeeded.Add(ConvertToProductInStock(needed));
			}
			foreach (var generated in ui.GeneratedProducts)
			{
				ret.GeneratedProducts.Add(ConvertToProductInStock(generated));
			}

			return ret;
		}

		public static Coordinates ConvertToCoordinates(SpaceDealerModels.Units.Coordinates uCoords)
		{
			return new Coordinates { X = uCoords.X, Y = uCoords.Y, Z = uCoords.Z };
		}

		public static Planet ConvertToPlanet(SpaceDealerModels.Units.Planet uP)
		{
			var ret = new Planet
			{
				PlanetName = uP.Name,
				Sector = ConvertToCoordinates(uP.Sector)
			};

			foreach (var industry in uP.Industries)
			{
				ret.Industries.Add(ConverToIndustry(industry));
			}
			return ret;
			
		}

		public static Journey ConvertToJourney(SpaceDealerModels.Units.Journey uJourny)
		{
			return new Journey
			{
				CurrentDistance = uJourny.CurrentDistanceToDestination,
				CurrentSector = ConvertToCoordinates(uJourny.CurrentSector),
				Departure = ConvertToPlanet(uJourny.Depature),
				Destination = ConvertToPlanet(uJourny.Destination)
			};
		}
		public static Ship ConvertToShip(SpaceDealerModels.Units.Ship uShip)
		{
			var ret = new Ship
			{
				ShipName = uShip.Name,
				Cruise = ConvertToJourney(uShip.Cruise),
				CargoSize = uShip.CargoSize
			};
			var pload = new Load();
			if (uShip.CurrentLoad.Any())
			{
				foreach (var upis in uShip.CurrentLoad)
				{
					pload.LoadedProducts.Add(ConvertToProductInStock(upis));
				}
			}
			ret.CargoLoad = pload;
			return ret;
		}

		internal static Player ConvertToPlayer(SpaceDealerModels.Units.Player uP)
		{
			var ret = new Player();
			ret.Name = uP.Name;
			foreach (var sh in uP.Fleet)
			{
				ret.Ships.Add(ConvertToShip(sh));
			}
			return ret;
		}
	}
}
