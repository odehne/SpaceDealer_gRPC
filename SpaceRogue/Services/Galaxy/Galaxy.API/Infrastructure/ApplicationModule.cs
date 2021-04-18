using Autofac;
using Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Reflection;
using Cope.SpaceRogue.Infrastructure;

namespace Cope.SpaceRogue.Galaxy.API.Infrastructure
{

	public class ApplicationModule : Autofac.Module
	{
		public GalaxyDbContext Context { get; }

		public ApplicationModule(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentException("Connection string not found");

			Context = new GalaxyDbContext(connectionString);
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => new ProductGroupRepository(Context))
				.As<IProductGroupRepository>()
				.InstancePerLifetimeScope();

			builder.Register(c => new ProductRepository(Context))
				.As<IProductRepository>()
				.InstancePerLifetimeScope();

			builder.Register(c => new PlayerRepository(Context))
				.As<IPlayerRepository>()
				.InstancePerLifetimeScope();

			builder.Register(c => new MarketPlaceRepository(Context))
				.As<IMarketPlaceRepository>()
				.InstancePerLifetimeScope();

			builder.Register(c => new CatalogItemsRepository(Context))
				.As<ICatalogItemsRepository>()
				.InstancePerLifetimeScope();

			builder.Register(c => new FeatureRepository(Context))
				.As<IFeatureRepository>()
				.InstancePerLifetimeScope();

			builder.Register(c => new ShipRepository(Context))
				.As<IShipRepository>()
				.InstancePerLifetimeScope();
			
			builder.Register(c => new PlanetRepository(Context))
				.As<IPlanetRepository>()
				.InstancePerLifetimeScope();

			builder.RegisterAssemblyTypes(typeof(PlanetSpawnedIntegrationEvent).GetTypeInfo().Assembly)
		   .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
		}
	}
}
