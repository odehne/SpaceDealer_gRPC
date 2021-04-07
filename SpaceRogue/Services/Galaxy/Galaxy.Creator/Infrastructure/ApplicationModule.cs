using Autofac;
using Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers;
using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using System.Reflection;

namespace Cope.SpaceRogue.Galaxy.Creator.Infrastructure
{

	public class ApplicationModule : Autofac.Module
	{
		public GalaxyDbContext Context { get; }

		public ApplicationModule(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				connectionString = "/Users/oliverde/Documents/SpaceRogue.db";

			Context = new GalaxyDbContext(connectionString);
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new ProductGroupRepository(Context))
				.As<IRepository<ProductGroup>>()
				.InstancePerLifetimeScope();

			builder.Register(c => new ProductRepository(Context))
				.As<IRepository<Product>>()
				.InstancePerLifetimeScope();

			builder.Register(c => new PlayerRepository(Context))
				.As<IRepository<Player>>()
				.InstancePerLifetimeScope();

			builder.Register(c => new MarketPlaceRepository(Context))
				.As<IRepository<MarketPlace>>()
				.InstancePerLifetimeScope();

			builder.Register(c => new CatalogItemsRepository(Context))
				.As<IRepository<CatalogItem>>()
				.InstancePerLifetimeScope();

			builder.Register(c => new FeatureRepository(Context))
				.As<IRepository<Feature>>()
				.InstancePerLifetimeScope();

			builder.Register(c => new ShipRepository(Context))
				.As<IRepository<Ship>>()
				.InstancePerLifetimeScope();

			builder.RegisterAssemblyTypes(typeof(PlanetSpawnedIntegrationEvent).GetTypeInfo().Assembly)
		   .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
		}
	}
}
