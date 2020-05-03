using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceDealerModels;
using SpaceDealerModels.Units;

namespace SpaceDealerService
{
	public static class ProtoBufConverter
	{
		public static ProductInStock ConvertToProductInStock(SpaceDealerModels.Units.DbProductInStock uPis)
		{
			return new ProductInStock
			{
				ProductName = uPis.Name,
				Amount = uPis.Amount,
				AmountGeneratedPerRound = uPis.AmountGeneratedPerRound,
				PricePerTon = uPis.PricePerTon,
				TotalPrice = uPis.GetTotalPrice(),
				TotalWeight = uPis.GetTotalWeight(),
				PicturePath = uPis.PicturePath
			};
		}

		public static Industry ConverToIndustry(SpaceDealerModels.Units.DbIndustry ui)
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

	
		public static Planet ConvertToPlanet(SpaceDealerModels.Units.DbPlanet uP)
		{
			var ret = new Planet
			{
				PlanetName = uP.Name,
				Sector = ConvertToCoordinates(uP.Sector),
				PicturePath = uP.PicturePath
			};

			ret.Industries.Add(ConverToIndustry(uP.Industry));
			return ret;
			
		}

		public static Journey ConvertToJourney(SpaceDealerModels.Units.Journey uJourny)
		{
			var ret = new Journey
			{
				CurrentDistance = uJourny.CurrentDistanceToDestination,
				CurrentSector = ConvertToCoordinates(uJourny.CurrentSector),
				Departure = ConvertToPlanet(uJourny.Departure),
				Destination = ConvertToPlanet(uJourny.Destination)
			};

			if (uJourny.NewlyDiscoveredPlanet != null)
			{
				ret.NewPlanetDiscovered = ConvertToPlanet(uJourny.NewlyDiscoveredPlanet);
			}

			if (uJourny.EnemyBattleShip != null)
			{
				ret.EnemyBattleShip = ConvertToPirateShip(uJourny.EnemyBattleShip);
			}

			return ret;
		}

		public static BattleReply ConvertToBattleReply(BattleResult result)
		{
			return new BattleReply
			{
				CriticalHit = result.CriticalHit,
				DefenderWasHit = result.DefenderWasHit,
				Message = result.Message,
				Value = result.Value,
				Treasure = result.Treasure,
				Defeaded = result.Defeaded
			};

		}

		public static UpdateInfo ConvertToUpdateInfo(SpaceDealerModels.Units.UpdateInfo uInfo)
		{
			var ret = new UpdateInfo();
			ret.Ship = ConvertToShip(uInfo.TheShip);
			ret.UpdateState = (UpdateStates)uInfo.UpdateState;
			return ret;
		}

		public static PirateShip ConvertToPirateShip(SpaceDealerModels.Units.PirateShip pShip)
		{
			var ret = new PirateShip
			{
				ShipName = pShip.Name,
				Shields = pShip.Shields,
				Hull = pShip.Hull,
				PicturePath = pShip.PicturePath
			};
			return ret;
		}

		public static Ship ConvertToShip(SpaceDealerModels.Units.DbShip uShip)
		{
			var ret = new Ship
			{
				ShipName = uShip.Name,
				CargoSize = uShip.CargoSize,
				Shields = uShip.Shields,
				Hull = uShip.Hull,
				PicturePath = uShip.PicturePath
			};

			if (uShip.CurrentPlanet != null)
			{
				ret.CurrentPlanet = ConvertToPlanet(uShip.CurrentPlanet);
			}

			if(uShip.Cruise!=null)
			{
				ret.Cruise = ConvertToJourney(uShip.Cruise);
			}

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

		internal static Player ConvertToPlayer(SpaceDealerModels.Units.DbPlayer uP)
		{
			var player = new Player
			{
				Name = uP.Name,
				Credits = uP.Credits,
				HomePlanet = uP.HomePlanet.Name,
				PicturePath = uP.PicturePath
			};
			foreach (var sh in uP.Fleet)
			{
				player.Ships.Add(ConvertToShip(sh));
			}
			return player;
		}
	}
}
