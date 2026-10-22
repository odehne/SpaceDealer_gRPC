using System.Text.Json.Serialization;

namespace SpaceDealer.Web.Services;

// Client-side view of the JSON served by SpaceDealerService (camelCase, enums as strings).
// Kept separate from SpaceDealerModels because those types have no deserializable constructors.

public sealed class CoordinatesDto
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public static double Distance(CoordinatesDto a, CoordinatesDto b)
        => Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2) + Math.Pow(b.Z - a.Z, 2));
}

public sealed class ProductDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public double Amount { get; set; }
    public double Weight { get; set; }
    public double AmountGeneratedPerRound { get; set; }
    public double PricePerTon { get; set; }
}

public sealed class IndustryDto
{
    public List<ProductDto> GeneratedProducts { get; set; } = [];
    public List<ProductDto> ProductsNeeded { get; set; } = [];
}

public sealed class PlanetDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public CoordinatesDto Sector { get; set; } = new();
    [JsonPropertyName("industries")]
    public IndustryDto? Industry { get; set; }
}

public sealed class CargoDto
{
    public string ProductId { get; set; } = "";
    public string Name { get; set; } = "";
    public double Quantity { get; set; }
    public ProductDto? Product { get; set; }
}

public sealed class EnemyShipDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Shields { get; set; }
    public int Hull { get; set; }
    public double Credits { get; set; }
}

public sealed class JourneyDto
{
    public PlanetDto? Departure { get; set; }
    // The service serializes this property as "detination" (sic).
    [JsonPropertyName("detination")]
    public PlanetDto? Destination { get; set; }
    public CoordinatesDto CurrentSector { get; set; } = new();
    public EnemyShipDto? EnemyBattleShip { get; set; }
    public double CurrentDistanceToDestination { get; set; }
}

public sealed class FeatureDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

public sealed class ShipDto
{
    public List<FeatureDto> Features { get; set; } = [];
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public double CargoSize { get; set; }
    public string State { get; set; } = "";
    public int Shields { get; set; }
    public int Hull { get; set; }
    public int RangeBonus { get; set; }
    public JourneyDto? Cruise { get; set; }
    public List<CargoDto> Cargo { get; set; } = [];
    public PlanetDto? CurrentPlanet { get; set; }

    public double CargoUsed => Cargo.Sum(c => c.Quantity);
    public bool IsTravelling => Cruise is { CurrentDistanceToDestination: > 0 };
    /// <summary>The planet the ship is docked at, or null while in transit.</summary>
    public PlanetDto? DockedPlanet => IsTravelling ? null : Cruise?.Destination ?? CurrentPlanet;
}

public sealed class PlayerDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public double Credits { get; set; }
    public PlanetDto? HomePlanet { get; set; }
    public Dictionary<string, ShipDto> Fleet { get; set; } = [];
    public Dictionary<string, PlanetDto> DiscoveredPlanets { get; set; } = [];
}

public sealed class BattleResultDto
{
    public string? Message { get; set; }
    public bool DefenderWasHit { get; set; }
    public bool CriticalHit { get; set; }
    public double Treasure { get; set; }
    public bool Defeaded { get; set; }
}

/// <summary>Outcome of an API call: either a value or a human-readable error.</summary>
public sealed record ApiResult<T>(T? Value, string? Error = null)
{
    public bool Ok => Error is null;
}
