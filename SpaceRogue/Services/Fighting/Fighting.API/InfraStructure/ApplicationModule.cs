using Autofac;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Reflection;
using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Fighting.API.Repositories;
using Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events;

namespace Cope.SpaceRogue.Fighting.API.Infrastructure
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
			builder.Register(c => new ShipRepository(Context))
				.As<IShipRepository>()
				.InstancePerLifetimeScope();
		
			builder.RegisterAssemblyTypes(typeof(AttackMissedIntegrationEvent).GetTypeInfo().Assembly)
		   .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
		}
	}
}
