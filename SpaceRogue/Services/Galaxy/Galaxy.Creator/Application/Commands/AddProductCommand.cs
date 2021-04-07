using Cope.SpaceRogue.Galaxy.API.Model;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{

	public class AddProductCommand : IRequest<ProductDTO>
    {
		[DataMember]
		public string ProductId { get; private set; }

		[DataMember]
        public string ProductName { get; private set; }

        [DataMember]
        public string ProductGroupId { get; private set; }

        [DataMember]
        public double PricePerUnit { get; private set; }

		[DataMember]
		public double Capacity { get; private set; }

		[DataMember]
		public int Rarity { get; private set; }

		public AddProductCommand(string productId, string productName, string productGroupId, double pricePerUnit, double capacity, int rarity)
		{
			ProductId = productId;
			ProductName = productName;
			ProductGroupId = productGroupId;
			PricePerUnit = pricePerUnit;
			Capacity = capacity;
			Rarity = rarity;
		}
	}

	public class ProductDTO
	{
		public string ProductId { get; private set; }
		public string ProductGroupId { get; private set; }
		public string Name { get; private set; }
		public double PricePerUnit { get; private set; }
		public double Capacity { get; private set; }
		public int Rarity { get; private set; }

		public ProductDTO(string productId, string productGroupId, string name, double pricePerUnit, double capacity, int rarity)
		{
			ProductId = productId;
			ProductGroupId = productGroupId;
			Name = name;
			PricePerUnit = pricePerUnit;
			Capacity = capacity;
			Rarity = rarity;
		}
	}
}
