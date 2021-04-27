using Cope.SpaceRogue.Galaxy.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Domain;
using MediatR;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddFeatureCommand : IRequest<bool>
	{
		[DataMember]
		public string Id { get; private set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int BattleAdvantage { get; set; }
		[DataMember]
		public int BattleDisadvantage { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public int FreightCapacityAdvantage { get; set; }
		[DataMember]
		public int FreightCapacityDisadvantage { get; set; }
		[DataMember]
		public int SensorRangeAdvantage { get; set; }
	}

	public class AddFeatureCommandHandler : IRequestHandler<AddFeatureCommand, bool>
	{
		private readonly IFeatureRepository _repository;

		public AddFeatureCommandHandler(IFeatureRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<bool> Handle(AddFeatureCommand request, CancellationToken cancellationToken)
		{
			var feature = new Feature()
			{
				ID = request.Id.ToGuid(),
				Name = request.Name,
				Description = request.Description,
				BattleAdvantage = request.BattleAdvantage,
				BattleDisadvantage=request.BattleDisadvantage,
				FreightCapacityAdvantage=request.FreightCapacityAdvantage,
				FreightCapacityDisadvantage=request.FreightCapacityDisadvantage,
				SensorRangeAdvantage=request.SensorRangeAdvantage
			};
			var b = await _repository.AddItem(feature);
			return b;
		}
	}
}
