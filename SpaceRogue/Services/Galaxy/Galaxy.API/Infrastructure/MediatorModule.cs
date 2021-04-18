using Autofac;
using Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers;
using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using FluentValidation;
using Galaxy.Creator.Application.Behaviors;
using Galaxy.Creator.Application.Commands;
using Galaxy.Creator.Application.Validations;
using MediatR;
using System;
using System.Linq;
using System.Reflection;
using Cope.SpaceRogue.Infrastructure;

namespace Cope.SpaceRogue.Galaxy.API.Infrastructure
{
	public class MediatorModule : Autofac.Module
	{
		public GalaxyDbContext Context { get; }

		public MediatorModule(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentException("Connection string not found");

			Context = new GalaxyDbContext(connectionString);
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
				.AsImplementedInterfaces();
			
			// Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
			builder.RegisterAssemblyTypes(typeof(SpawnPlanetCommand).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			builder.RegisterAssemblyTypes(typeof(AddProductGroupCommand).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			builder.RegisterAssemblyTypes(typeof(AddProductCommand).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			builder.RegisterAssemblyTypes(typeof(ProductsQuery).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			// Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
			builder.RegisterAssemblyTypes(typeof(ValidateOrAddPlanetWhenPlanetSpawnedDomainEventHandler).GetTypeInfo().Assembly)
				.AsClosedTypesOf(typeof(INotificationHandler<>));

			// Register the Command's Validators (Validators based on FluentValidation library)
			builder
				.RegisterAssemblyTypes(typeof(CreatePlanetValidator).GetTypeInfo().Assembly)
				.Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
				.AsImplementedInterfaces();


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
