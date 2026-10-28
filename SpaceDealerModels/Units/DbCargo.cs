using System.Xml.Serialization;

namespace SpaceDealerModels.Units
{

    public class DbCargo : BaseUnit
	{
		public string ShipId { get; set; }
		public string ProductId { get; set; }
        public double Quantity { get; set; } // in tons
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public DbShip Parent { get; set; }
        public DbProductInStock Product { get; set; }

        public DbCargo() { }

        public DbCargo( DbShip parent, DbProductInStock product, double quantity) : base($"{parent.Id}-{product.Id}")
        {
            Parent = parent;
            Product = product;

            ShipId = parent.Id;
            ProductId = product.Id;
            Quantity = quantity;
        }

        public double GetTotalWeight()
        {
            return Quantity * Product.Weight;
        }


        public double GetTotalPrice()
        {
            return Quantity * Product.Weight * Product.PricePerTon;
        }

        public override void Update()
        {
            // Update the product reference from the parent's cargo bay
            // This ensures that the product information is always current
            // Maybe the products can expire or change in some way, so we want to make sure we have the latest info
            base.Update();
        }

    }
}
