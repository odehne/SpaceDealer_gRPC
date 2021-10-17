using System.Collections.Generic;

namespace Galaxy.Creator.App.Model
{
	public class ProductGroupModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public List<ProductModel> Products { get; set; }
	}

}
