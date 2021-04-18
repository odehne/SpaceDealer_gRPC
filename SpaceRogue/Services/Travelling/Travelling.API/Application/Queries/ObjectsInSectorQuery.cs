using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Traveling.API.Controllers
{
    public class ObjectsInSectorQuery : IRequest<IEnumerable<ObjectsInSectorModel>> { }

	public class ObjectsInSectorQueryHandler : IRequestHandler<ObjectsInSectorQuery, IEnumerable<ObjectsInSectorModel>>
	{
		//private readonly IObjectRepository _repository;

		//public ProductGroupsQueryHandler(IProductGroupRepository repository)
		//{
		//    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
		//}

		//public async Task<IEnumerable<ProductGroupDto>> Handle(ProductGroupsQuery request, CancellationToken cancellationToken)
		//{
		//    {
		//        var itms = await _repository.GetItems();
		//        var dtos = new List<ProductGroupDto>();

		//        foreach (var itm in itms)
		//        {
		//            dtos.Add(AutoMap.Mapper.Map<ProductGroupDto>(itm));
		//        }

		//        return dtos;
		//    }
		//}
		public Task<IEnumerable<ObjectsInSectorModel>> Handle(ObjectsInSectorQuery request, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}