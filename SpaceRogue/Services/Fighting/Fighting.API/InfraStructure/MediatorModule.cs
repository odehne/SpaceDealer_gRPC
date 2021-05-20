using Autofac;
using MediatR;
using System;
using System.Reflection;
using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Behaviors;
using Cope.SpaceRogue.Fighting.Application.Queries;
using Cope.SpaceRogue.Fighting.API.Repositories;

namespace Cope.SpaceRogue.Fighting.API.Infrastructure
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

			builder.RegisterAssemblyTypes(typeof(ShipsQuery).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			builder.RegisterType<ShipRepository>().As<IShipRepository>().WithParameter("context", new GalaxyDbContext(ConnectionString));
			builder.RegisterType<FightRepository>()
				.As<IFightRepository>()
				.WithParameter("context", new GalaxyDbContext(ConnectionString))
				.WithParameter("mediator", Factory.Mediator);

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
