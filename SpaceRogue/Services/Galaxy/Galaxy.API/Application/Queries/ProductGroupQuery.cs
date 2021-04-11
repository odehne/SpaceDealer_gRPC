using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Galaxy.Creator.Application.Commands
{
    public class ProductGroupQuery : IRequest<ProductGroupDTO> 
    { 
        [DataMember]
        public string GroupId { get; private set; }

		public ProductGroupQuery(string groupId)
		{
			GroupId = groupId;
		}
	}

    public class ProductGroupQueryHandler : IRequestHandler<ProductGroupQuery, ProductGroupDTO>
    {
        private readonly IProductGroupRepository _repository;

        public ProductGroupQueryHandler(IProductGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProductGroupDTO> Handle(ProductGroupQuery request, CancellationToken cancellationToken)
        {
            var itm = await _repository.GetItem(request.GroupId.ToGuid());
            return ProductGroupDTO.MapToDto(itm);
        }
    }
}
