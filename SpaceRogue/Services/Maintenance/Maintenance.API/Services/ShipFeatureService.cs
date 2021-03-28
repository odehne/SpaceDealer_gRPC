using Cope.SpaceRogue.Maintenance.API;
using Cope.SpaceRogue.Maintenance.API.Proto;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Services.Maintenance.API.Services
{

	public class ShipFeatureService : ShipFeaturesService.ShipFeaturesServiceBase
	{
		public override Task<AddedFeatureReply> AddFeature(FeatureRequest request, ServerCallContext context)
		{
			return base.AddFeature(request, context);
		}

		public override Task<AddedFeatureReply> AddFeatureToShip(FeatureToShipRequest request, ServerCallContext context)
		{
			return base.AddFeatureToShip(request, context);
		}

		public override Task<FeatureReply> GetFeature(FeatureIdRequest request, ServerCallContext context)
		{
			return base.GetFeature(request, context);
		}

		public override Task<FeaturesReply> GetFeatures(GetFeaturesRequest request, ServerCallContext context)
		{
			return base.GetFeatures(request, context);
		}
	}
}
