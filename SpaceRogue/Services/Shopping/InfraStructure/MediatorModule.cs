using Autofac;
using MediatR;
using System;
using System.Reflection;
using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Shopping.Application.Queries;
using Cope.SpaceRogue.Shopping.API.Repositories;
using Cope.SpaceRogue.Infrastructure.Behaviors;

namespace Cope.SpaceRogue.Shopping.API.Infrastructure
{
	public class MediatorModule : Autofac.Module
	{
		public GalaxyDbContext Context { get; }
		public string ConnectionString { get; }

		public MediatorModule(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentException("Connection string not found");

			Context = new GalaxyDbContext(connectionString);
			ConnectionString = connectionString;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
				.AsImplementedInterfaces();

			builder.RegisterAssemblyTypes(typeof(PlanetsQuery).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			builder.RegisterType<PlanetRepository>().As<IPlanetRepository>().WithParameter("context", new GalaxyDbContext(ConnectionString));
			builder.RegisterType<MarketPlaceRepository>().As<IMarketPlaceRepository>().WithParameter("context", new GalaxyDbContext(ConnectionString));
			builder.RegisterType<ShipRepository>().As<IShipRepository>().WithParameter("context", new GalaxyDbContext(ConnectionString));
			builder.RegisterType<PlayerRepository>().As<IPlayerRepository>().WithParameter("context", new GalaxyDbContext(ConnectionString));

			builder.Register<ServiceFactory>(context =>
			{
				var componentContext = context.Resolve<IComponentContext>();
				return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
			});

			builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
			builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
		}
	}
}
