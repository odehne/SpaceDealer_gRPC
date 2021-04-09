using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.InfraStructure;
using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class AddProductGroupCommand : IRequest<ProductGroupDTO>
	{
		[DataMember]
		public string ProductGroupId { get; private set; }

		[DataMember]
		public string ProductGroupName { get; private set; }

		public AddProductGroupCommand(string productGroupId, string productGroupName)
		{
			ProductGroupId = productGroupId;
			ProductGroupName = productGroupName;
		}
	}

	public class ProductGroupDTO
	{
		public string ProductGroupId { get; private set; }
		public string ProductGroupName { get; private set; }

		public ProductGroupDTO(string productGroupId, string productGroupName)
		{
			ProductGroupId = productGroupId;
			ProductGroupName = productGroupName;
		}

		internal static ProductGroupDTO MapToDto(ProductGroup itm)
		{
			return new ProductGroupDTO(itm.ID.ToString(), itm.Name);
		}

	}

}
