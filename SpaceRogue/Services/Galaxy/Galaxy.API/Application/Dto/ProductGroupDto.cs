using Cope.SpaceRogue.Galaxy.API.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class ProductGroupDto
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public ICollection<ProductDto> Products { get; set; }

	}

}
