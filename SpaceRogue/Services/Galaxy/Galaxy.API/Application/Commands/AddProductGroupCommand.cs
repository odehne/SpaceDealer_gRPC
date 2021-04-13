using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.InfraStructure;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddProductGroupCommand : IRequest<ProductGroupDto>
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

	public class AddProductGroupCommandHandler : IRequestHandler<AddProductGroupCommand, ProductGroupDto>
    {
        private readonly IProductGroupRepository _repository;

        public AddProductGroupCommandHandler(IProductGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProductGroupDto> Handle(AddProductGroupCommand request, CancellationToken cancellationToken)
        {
            var grp = AutoMap.Mapper.Map<ProductGroup>(request);

            grp = await _repository.UpdateItem(grp);

			return AutoMap.Mapper.Map<ProductGroupDto>(grp);
        }
    }

}
