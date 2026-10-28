using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
    public class CargoBay : List<DbCargo>
    {

        public double TotalQuantity
        {
            get
            {
                return this.Sum(x => x.Quantity);
            }
        }

        public DbCargo GetCargoByProductId(string productId)
        {
            return this.FirstOrDefault(x => x.ProductId.Equals(productId));
        }

        public void AddCargo(DbShip ship, DbProductInStock product, double quantity)
        {
            var load = GetCargoByProductId(product.Id);
            if (load != null)
            {
                load.Quantity += quantity;
            }
            else
            {
                Add(new DbCargo(ship, product, quantity));
            }
        }

        public void RemoveCargo(string productId, double quantity)
        {
            var load = GetCargoByProductId(productId);
            if (load != null)
            {
                load.Quantity -= quantity;
                if (load.Quantity <= 0)
                {
                    Remove(load);
                }
            }
        }
    }
}
