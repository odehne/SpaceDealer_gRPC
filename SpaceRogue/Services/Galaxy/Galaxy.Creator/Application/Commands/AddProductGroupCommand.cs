using MediatR;
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
	}

}
