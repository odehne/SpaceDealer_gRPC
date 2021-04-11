using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Creator.Application.Validations
{
	public class CreatePlanetValidator : AbstractValidator<SpawnPlanetCommand>
	{
		public CreatePlanetValidator(ILogger<CreatePlanetValidator> logger)
		{
			RuleFor(command => command.PlanetId).NotEmpty();
			RuleFor(command => command.Name).NotEmpty();
			RuleFor(command => command.PosX).NotEmpty().Must(GreaterOrEqualZero).WithMessage("Invalid X position");
			RuleFor(command => command.PosY).NotEmpty().Must(GreaterOrEqualZero).WithMessage("Invalid Y position");
			RuleFor(command => command.PosZ).NotEmpty().Must(GreaterOrEqualZero).WithMessage("Invalid Z position");

			logger.LogTrace("----- PLANET SPAWNED - {ClassName}", GetType().Name);
		}

		private bool GreaterOrEqualZero(int pos)
		{
			return pos >= 0;
		}
	}
}
