using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;


namespace Cope.SpaceRogue.Shopping.Application.Queries
{
	public class PlayersQuery : IRequest<List<PlayerModel>> { }

    public class PlayersQueryHandler : IRequestHandler<PlayersQuery, List<PlayerModel>>
    {
        private readonly IPlayerRepository _repository;

        public PlayersQueryHandler(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<PlayerModel>> Handle(PlayersQuery request, CancellationToken cancellationToken)
        {
            {
                var players = await _repository.GetItems();
                var model = new List<PlayerModel>();

                foreach (var player in players)
                {
                   var playerModel = new PlayerModel
                    {
                        ID = player.ID,
                        Name = player.Name,
                        Credits = player.Credits,
                        PlayerType = player.PlayerType
                    };

					foreach (var ship in player.Fleet)
					{
                        var shipModel = new ShipModel
                        {
                            ID = ship.Id,
                            Name = ship.Name,
                            PlayerId = player.ID,
                            Capacity = ship.LoadedCapacity
                        };
						foreach (var cargo in ship.Cargo)
						{
                            shipModel.Cargo.Add(new PayloadModel { ID = cargo.ID, ProductId = cargo.Product.ID, ProductName = cargo.Product.Name, Quantity = cargo.Quantity });
						}
                        foreach (var ft in ship.Features)
                        {
                            shipModel.Features.Add(new FeatureModel { ID = ft.ID, Name = ft.Name, Description = ft.Description, FreightCapacityAdvantage = ft.FreightCapacityAdvantage, FreightCapacityDisadvantage = ft.FreightCapacityDisadvantage });
                        }

                        playerModel.Fleet.Add(shipModel);
                    }

                    model.Add(playerModel);
                }

                return model;
            }
        }
    }
}
