namespace Galaxy.Creator.App.Model
{
	public class ProductModel
	{
		public string Id { get; set; }
		public string GroupId { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public double PricePerUnit { get; set; }
		public double Rarity { get; internal set; }
		public double Size { get; internal set; }
	}

}
