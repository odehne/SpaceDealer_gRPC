using Cope.SpaceRogue.Galaxy.API.Domain;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class ProductDto
	{
		public string ID { get; set; }
		public string GroupId { get; set; }
		public string GroupName { get; set; }
		public string Name { get; set; }
		public double PricePerUnit { get; set; }
		public double Capacity { get; set; }
		public double Rarity { get; set; }

	}
}
