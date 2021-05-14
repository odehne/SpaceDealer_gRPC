using System;

namespace Cope.SpaceRogue.Shopping.API.Models
{

	public class PayloadModel
	{
		public Guid ID { get; set; }
		public Guid ProductId { get; set; }
		public string ProductName {get; set;}
		public double Quantity { get; set; }

	}

}