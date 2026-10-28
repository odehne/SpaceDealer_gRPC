using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Data.SQLite;
using System.Diagnostics;

namespace SpaceDealerService.Repos
{
	public class ShoppingSystem
    {
        public DbPlayer Player { get; set; }
        public DbShip Ship { get; set; }
        public DbPlanet Planet { get; set; }

        public ShoppingSystem() 
        { 
        
        }

        public ShoppingSystem(DbPlayer player, DbShip ship, DbPlanet planet)
        {
            Player = player;
            Ship = ship;
            Planet = planet;
        }

        public void BuyItem(string productId, double quantity)
        {
            var product = Planet.Industry.GeneratedProducts.GetById(productId);
            if (product == null)
            {
                throw new Exception($"Item {productId} not found in market.");
            }

            if (product.Amount < quantity)
            {
                throw new Exception($"Not enough quantity of {productId} in market.");
            }

            double totalPrice = product.PricePerTon * quantity;
            if (Player.Credits < totalPrice)
            {
                throw new Exception($"Player does not have enough credits to buy {quantity} of {productId}.");
            }

            // Deduct credits from player
            Player.Credits -= totalPrice;

            // Add item to ship's cargo
            Ship.CargoBay.AddCargo(Ship, product, quantity);

            // Deduct item from planet's market
            Planet.Industry.DeductItem(productId, quantity);
        }

    }

}
