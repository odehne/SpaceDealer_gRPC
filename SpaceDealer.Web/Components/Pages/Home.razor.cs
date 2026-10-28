using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SpaceDealer.Web.Services;

namespace SpaceDealer.Web.Components.Pages;

public partial class Home : IDisposable
{
    private const string StorageKey = "spacedealer.player";
    private static readonly CultureInfo GermanCulture = CultureInfo.GetCultureInfo("de-DE");
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(2);
    private const int UniverseRefreshEveryNTicks = 8;
    private const int MaxLogEntries = 30;

    [Inject] private SpaceDealerApiClient Api { get; set; } = default!;
    [Inject] private ProtectedLocalStorage Storage { get; set; } = default!;

    private readonly CancellationTokenSource _cts = new();
    private readonly Dictionary<string, ShipSnapshot> _snapshots = [];
    private bool _pollingStarted;

    // session
    private PlayerDto? Player;
    private string? SelectedShipId;
    private bool Restoring = true;
    private bool Connected = true;
    private bool Busy;

    // login form
    private string LoginName = "";
    private string? LoginError;

    // ui state
    private string? _toast;
    private int _toastVersion;
    private string? ToastMessage
    {
        get => _toast;
        set
        {
            _toast = value;
            var version = ++_toastVersion;
            if (value is not null) _ = ExpireToastAsync(version);
        }
    }
    private bool ShowLog;
    private bool ShowShipyard;
    private List<FeatureDto> ShipyardFeatures = [];
    public const double NewShipCosts = 150_000;
    private const double DumpPriceFactor = 0.6;
    private double Quantity = 1;
    private string? SelectedDestinationId;
    private string NewShipName = "";
    private readonly List<LogEntry> LogEntries = [];

    // universe
    private int PlanetCount;
    private int ShipCount;
    private List<LeaderEntry> Leaders = [];

    private ShipDto? SelectedShip
        => Player is not null && SelectedShipId is not null && Player.Fleet.TryGetValue(SelectedShipId, out var ship) ? ship : null;
    private string? InspectedPlanetId;
    private PlanetDto? InspectedPlanet
        => InspectedPlanetId is not null && Player?.DiscoveredPlanets.TryGetValue(InspectedPlanetId, out var planet) == true ? planet : null;
    private void InspectPlanet(string? planetId) => InspectedPlanetId = planetId == InspectedPlanetId ? null : planetId;
    private PlanetDto? DockedPlanet => SelectedShip?.DockedPlanet;
    private bool IsTravelling => SelectedShip?.IsTravelling ?? false;
    private IEnumerable<ProbeDto> FlyingProbes
        => Player?.Fleet.Values
            .SelectMany(s => s.Probes?.Values ?? (IEnumerable<ProbeDto>)[])
            .Where(p => p.State == "Travelling" && p.CurrentSector is not null) ?? [];
    // Ships of other pilots inside the scan range of the selected ship.
    private List<NearbyShipDto> NearbyShips = [];
    private EnemyShipDto? Enemy => SelectedShip?.Cruise?.EnemyBattleShip;
    private IEnumerable<PlanetDto> Destinations
        => (Player?.DiscoveredPlanets.Values ?? Enumerable.Empty<PlanetDto>())
            .Where(p => p.Id != DockedPlanet?.Id).OrderBy(p => p.Name);

    // ---------------------------------------------------------------- lifecycle

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        try
        {
            var stored = await Storage.GetAsync<string>(StorageKey);
            if (stored.Success && !string.IsNullOrWhiteSpace(stored.Value))
                await LoginAsync(stored.Value, remember: false);
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.Security.Cryptography.CryptographicException)
        {
            // stored value unreadable (e.g. data-protection keys changed): fall back to the login form
        }
        finally
        {
            Restoring = false;
            StateHasChanged();
        }
    }

    private async Task ExpireToastAsync(int version)
    {
        try { await Task.Delay(TimeSpan.FromSeconds(6), _cts.Token); }
        catch (OperationCanceledException) { return; }

        if (version != _toastVersion) return;
        _toast = null;
        await InvokeAsync(StateHasChanged);
    }

    private async Task PollLoopAsync()
    {
        using var timer = new PeriodicTimer(PollInterval);
        var tick = 0;
        try
        {
            while (await timer.WaitForNextTickAsync(_cts.Token))
            {
                await InvokeAsync(async () =>
                {
                    if (Player is null) return;
                    await RefreshPlayerAsync();
                    if (++tick % UniverseRefreshEveryNTicks == 0)
                        await RefreshUniverseAsync();
                    StateHasChanged();
                });
            }
        }
        catch (OperationCanceledException)
        {
            // component disposed
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    // ---------------------------------------------------------------- session actions

    private async Task LoginAsync(string name, bool remember = true)
    {
        name = name.Trim();
        if (name.Length == 0)
        {
            LoginError = "Bitte gib einen Namen ein.";
            return;
        }

        Busy = true;
        LoginError = null;
        var result = await Api.LoginAsync(name, _cts.Token);
        Busy = false;

        if (!result.Ok || result.Value is null)
        {
            Connected = !(result.Error?.Contains("nicht erreichbar") ?? false);
            LoginError = result.Error ?? "Anmeldung fehlgeschlagen.";
            return;
        }

        Connected = true;
        _snapshots.Clear();
        LogEntries.Clear();
        ApplyPlayer(result.Value, detectChanges: false);
        if (remember)
            await Storage.SetAsync(StorageKey, result.Value.Name);

        await RefreshUniverseAsync();
        AddLog($"Willkommen an Bord, Kapitän {Player!.Name}.");

        if (!_pollingStarted)
        {
            _pollingStarted = true;
            _ = PollLoopAsync();
        }
    }

    private async Task LogoutAsync()
    {
        Player = null;
        SelectedShipId = null;
        LoginName = "";
        ShowLog = false;
        _snapshots.Clear();
        await Storage.DeleteAsync(StorageKey);
    }

    private async Task SaveGameAsync()
    {
        var result = await Api.SaveGameAsync(_cts.Token);
        ToastMessage = result.Ok ? "Spielstand gespeichert." : result.Error;
    }

    // ---------------------------------------------------------------- refresh

    private async Task RefreshPlayerAsync()
    {
        if (Player is null) return;

        var result = await Api.GetPlayerAsync(Player.Id, _cts.Token);
        Connected = !(result.Error?.Contains("nicht erreichbar") ?? false);
        if (result.Ok && result.Value is not null)
            ApplyPlayer(result.Value, detectChanges: true);
        else if (Connected)
            ToastMessage = "Der Spieler wurde im Dienst nicht mehr gefunden. Bitte neu anmelden.";

        await RefreshNearbyShipsAsync();
    }

    private async Task RefreshNearbyShipsAsync()
    {
        if (Player is null || SelectedShip is null)
        {
            NearbyShips = [];
            return;
        }

        var nearby = await Api.GetNearbyShipsAsync(Player.Id, SelectedShip.Id, _cts.Token);
        NearbyShips = nearby.Ok && nearby.Value is not null ? nearby.Value : [];
    }

    private async Task RefreshUniverseAsync()
    {
        var players = await Api.GetPlayersAsync(_cts.Token);
        if (players.Ok && players.Value is not null)
        {
            ShipCount = players.Value.Values.Sum(p => p.Fleet.Count);
            Leaders = players.Value.Values
                .OrderByDescending(p => p.Credits)
                .Select(p => new LeaderEntry(p.Name, p.Credits, p.Id == Player?.Id))
                .ToList();
        }

        var planets = await Api.GetPlanetCountAsync(_cts.Token);
        if (planets.Ok) PlanetCount = planets.Value;
    }

    private void ApplyPlayer(PlayerDto fresh, bool detectChanges)
    {
        var previousDiscovered = Player?.DiscoveredPlanets.Count ?? fresh.DiscoveredPlanets.Count;

        if (detectChanges)
        {
            foreach (var ship in fresh.Fleet.Values)
                DetectShipChanges(ship);
            if (fresh.DiscoveredPlanets.Count > previousDiscovered)
            {
                var known = Player?.DiscoveredPlanets;
                var newPlanets = fresh.DiscoveredPlanets
                    .Where(p => known is null || !known.ContainsKey(p.Key))
                    .Select(p => p.Value)
                    .ToList();
                AddLog($"{newPlanets.Count} neue(r) Planet(en) entdeckt: {string.Join(", ", newPlanets)}", toast: true);
            }
        }
        else
        {
            foreach (var ship in fresh.Fleet.Values)
                _snapshots[ship.Id] = ShipSnapshot.Of(ship);
        }

        Player = fresh;
        if (SelectedShipId is null || !fresh.Fleet.ContainsKey(SelectedShipId))
            SelectedShipId = fresh.Fleet.Keys.FirstOrDefault();

        if (SelectedDestinationId is null || SelectedDestinationId == DockedPlanet?.Id
            || !fresh.DiscoveredPlanets.ContainsKey(SelectedDestinationId))
            SelectedDestinationId = Destinations.FirstOrDefault()?.Id;
    }

    private void DetectShipChanges(ShipDto ship)
    {
        var current = ShipSnapshot.Of(ship);
        if (_snapshots.TryGetValue(ship.Id, out var old))
        {
            if (old.Travelling && !current.Travelling)
                AddLog($"{ship.Name} ist in {ship.DockedPlanet?.Name ?? "unbekanntem Ziel"} angekommen.", toast: true);
            if (old.EnemyId is null && current.EnemyId is not null)
                AddLog($"Piratenangriff! {ship.Cruise?.EnemyBattleShip?.Name} stellt {ship.Name}.", toast: true);
            if (old.EnemyId is not null && current.EnemyId is null)
                AddLog($"Der Kontakt zu {ship.Name}s Gegner ist abgerissen.");
            if (current.Hull < old.Hull)
                AddLog($"{ship.Name} hat Rumpfschaden erlitten ({current.Hull} verbleibend).");
        }
        _snapshots[ship.Id] = current;
    }

    private void AddLog(string message, bool toast = false)
    {
        LogEntries.Insert(0, new LogEntry(DateTime.Now, message));
        if (LogEntries.Count > MaxLogEntries)
            LogEntries.RemoveAt(LogEntries.Count - 1);
        if (toast) ToastMessage = message;
    }

    // ---------------------------------------------------------------- game actions

    private async Task StartTravelAsync()
    {
        if (SelectedShip is null || Player is null || SelectedDestinationId is null) return;
        if (IsTravelling)
        {
            ToastMessage = "Das Schiff ist bereits unterwegs.";
            return;
        }

        var destination = Player.DiscoveredPlanets.GetValueOrDefault(SelectedDestinationId);
        var result = await Api.StartTravelAsync(Player.Id, SelectedShip.Id, SelectedDestinationId, _cts.Token);
        if (result.Ok)
            AddLog($"{SelectedShip.Name} startet nach {destination?.Name}.", toast: true);
        else
            ToastMessage = result.Error;

        await RefreshPlayerAsync();
    }

    private async Task BuyAsync(GoodRow good)
    {
        if (SelectedShip is null || Player is null || DockedPlanet is null) return;
        var quantity = Math.Max(0.1, Quantity);

        if (good.Stock is null) return;
        if (quantity > good.Stock)
        {
            ToastMessage = $"Es sind nur {FormatAmount(good.Stock.Value)} t {good.Name} verfügbar.";
            return;
        }
        if (Player.Credits < good.BuyPrice * quantity)
        {
            ToastMessage = "Nicht genügend Credits für diesen Handel.";
            return;
        }
        if (SelectedShip.CargoUsed + quantity > SelectedShip.CargoSize)
        {
            ToastMessage = "Im Laderaum ist nicht genug Platz.";
            return;
        }

        var result = await Api.BuyAsync(Player.Id, SelectedShip.Id, DockedPlanet.Id, good.ProductId, quantity, _cts.Token);
        if (result.Ok)
            AddLog($"{FormatAmount(quantity)} t {good.Name} für {FormatCredits(result.Value)} CR gekauft.", toast: true);
        else
            ToastMessage = result.Error;

        await RefreshPlayerAsync();
    }

    private async Task SellAsync(GoodRow good)
    {
        if (SelectedShip is null || Player is null || DockedPlanet is null) return;
        var quantity = Math.Max(0.1, Quantity);

        var held = CargoOf(good.ProductId);
        if (held < quantity)
        {
            ToastMessage = held > 0
                ? $"Du hast nur {FormatAmount(held)} t {good.Name} geladen."
                : $"Keine Ladung von {good.Name} zum Verkaufen vorhanden.";
            return;
        }

        var result = await Api.SellAsync(Player.Id, SelectedShip.Id, DockedPlanet.Id, good.ProductId, quantity, _cts.Token);
        if (result.Ok)
            AddLog($"{FormatAmount(quantity)} t {good.Name} für {FormatCredits(result.Value)} CR verkauft.", toast: true);
        else
            ToastMessage = result.Error;

        await RefreshPlayerAsync();
    }

    private async Task NewShipAsync()
    {
        if (Player is null) return;
        var name = NewShipName.Trim();
        if (name.Length == 0)
        {
            ToastMessage = "Bitte gib dem neuen Schiff einen Namen.";
            return;
        }

        var result = await Api.NewShipAsync(Player.Id, name, _cts.Token);
        if (result.Ok)
        {
            NewShipName = "";
            AddLog($"Neues Schiff „{name}“ ist einsatzbereit.", toast: true);
            await RefreshPlayerAsync();
            if (result.Value is not null && Player.Fleet.ContainsKey(result.Value.Id))
                SelectedShipId = result.Value.Id;
        }
        else
        {
            ToastMessage = result.Error;
        }
    }

    private async Task ToggleShipyardAsync()
    {
        ShowShipyard = !ShowShipyard;
        if (!ShowShipyard) return;

        var result = await Api.GetShipyardFeaturesAsync(_cts.Token);
        if (result.Ok && result.Value is not null)
            ShipyardFeatures = result.Value;
        else
            ToastMessage = result.Error;
    }

    private bool HasFeature(FeatureDto feature)
        => feature.Name != "AdditionalProbe" && SelectedShip?.Features.ContainsKey(feature.Id) == true;

    private async Task BuyFeatureAsync(FeatureDto feature)
    {
        if (SelectedShip is null || Player is null) return;

        var result = await Api.BuyFeatureAsync(Player.Id, SelectedShip.Id, feature.Id, _cts.Token);
        if (result.Ok)
            AddLog($"{feature.Description} für {SelectedShip.Name} gekauft ({FormatCredits(feature.Costs)} CR).", toast: true);
        else
            ToastMessage = result.Error;

        await RefreshPlayerAsync();
    }

    private async Task SendProbeAsync()
    {
        if (SelectedShip is null || Player is null) return;

        var result = await Api.SendProbeAsync(Player.Id, SelectedShip.Id, _cts.Token);
        if (result.Ok)
            AddLog($"{SelectedShip.Name} hat eine Sonde ausgesandt.", toast: true);
        else
            ToastMessage = result.Error;

        await RefreshPlayerAsync();
    }

    private async Task BattleAsync(bool attack)
    {
        if (SelectedShip is null || Player is null) return;

        var result = attack
            ? await Api.AttackAsync(Player.Id, SelectedShip.Id, _cts.Token)
            : await Api.DefendAsync(Player.Id, SelectedShip.Id, _cts.Token);

        if (result.Ok && result.Value is not null)
        {
            var message = result.Value.Message ?? (attack ? "Angriff ausgeführt." : "Verteidigung ausgeführt.");
            if (result.Value.Defeaded)
                message += $" Beute: {FormatCredits(result.Value.Treasure)} CR.";
            AddLog(message, toast: true);
        }
        else
        {
            ToastMessage = result.Error;
        }

        await RefreshPlayerAsync();
    }

    private void SwitchShip()
    {
        if (Player is null || Player.Fleet.Count < 2) return;
        var ids = Player.Fleet.Keys.ToList();
        var next = (ids.IndexOf(SelectedShipId ?? "") + 1) % ids.Count;
        SelectedShipId = ids[next];
        SelectedDestinationId = Destinations.FirstOrDefault()?.Id;
        ToastMessage = $"Aktives Schiff: {SelectedShip?.Name}.";
    }

    // ---------------------------------------------------------------- view helpers

    private IEnumerable<GoodRow> MarketRows()
    {
        var industry = DockedPlanet?.Industry;
        if (industry is null) return [];

        var rows = new Dictionary<string, GoodRow>();
        foreach (var p in industry.GeneratedProducts)
            rows[p.Id] = new GoodRow(p.Id, p.Name) { Stock = p.Amount, BuyPrice = p.PricePerTon };
        foreach (var p in industry.ProductsNeeded)
        {
            if (!rows.TryGetValue(p.Id, out var row))
                rows[p.Id] = row = new GoodRow(p.Id, p.Name);
            row.Demand = p.Amount;
            row.SellPrice = p.PricePerTon;
        }

        // Cargo nobody here needs can still be dumped at a discount (mirrors TradingController.DUMP_PRICE_FACTOR).
        foreach (var cargo in SelectedShip?.Cargo.Where(c => c.Quantity > 0) ?? [])
        {
            if (!rows.TryGetValue(cargo.ProductId, out var row))
                rows[cargo.ProductId] = row = new GoodRow(cargo.ProductId, cargo.Product?.Name ?? cargo.Name);
            if (row.Demand is null)
            {
                row.IsDump = true;
                row.SellPrice = (cargo.Product?.PricePerTon ?? 0) * DumpPriceFactor;
            }
        }
        return rows.Values.OrderBy(r => r.Name);
    }

    private double CargoOf(string productId)
        => SelectedShip?.Cargo.Where(c => c.ProductId == productId).Sum(c => c.Quantity) ?? 0;

    private double JourneyProgress()
    {
        var cruise = SelectedShip?.Cruise;
        if (cruise?.Departure is null || cruise.Destination is null) return 1;
        var total = CoordinatesDto.Distance(cruise.Departure.Sector, cruise.Destination.Sector);
        return total <= 0 ? 1 : Math.Clamp(1 - cruise.CurrentDistanceToDestination / total, 0, 1);
    }

    // Sector coordinates run 0..UniverseSize on each axis (see DbCoordinates.UniverseSize); the map ignores Z.
    private const double UniverseSize = 30;
    private static double MapX(CoordinatesDto c) => 50 + c.X / UniverseSize * 660;
    private static double MapY(CoordinatesDto c) => 45 + c.Y / UniverseSize * 300;
    private static string Svg(double value) => value.ToString("0.#", CultureInfo.InvariantCulture);

    private static string FormatCredits(double value)
        => value.ToString(Math.Abs(value) < 100 ? "N2" : "N0", GermanCulture);
    private static string FormatAmount(double value) => value.ToString("N1", GermanCulture);
    private static string Initials(string name)
        => string.Concat(name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => char.ToUpperInvariant(w[0])));

    private sealed class GoodRow(string productId, string name)
    {
        public string ProductId { get; } = productId;
        public string Name { get; } = name;
        public double? Stock { get; set; }
        public double BuyPrice { get; set; }
        public double? Demand { get; set; }
        public double SellPrice { get; set; }
        public bool IsDump { get; set; }
    }

    private sealed record BestBuyer(PlanetDto Planet, double PricePerTon, double Demand, double? Distance);

    private readonly Dictionary<string, BestBuyer?> BestBuyers = [];

    // Asks TradingController.BrowseBestPrice for the known planet that pays most for this cargo.
    private async Task FindBuyerAsync(string productId)
    {
        if (SelectedShip is null || Player is null) return;

        var result = await Api.BrowseBestBuyerAsync(Player.Id, SelectedShip.Id, productId, _cts.Token);
        if (!result.Ok || result.Value is null)
        {
            BestBuyers[productId] = null;
            return;
        }

        var planet = result.Value;
        var need = planet.Industry?.ProductsNeeded.FirstOrDefault(n => n.Id == productId);
        var here = DockedPlanet?.Sector ?? SelectedShip.Cruise?.Destination?.Sector;
        BestBuyers[productId] = new BestBuyer(planet, need?.PricePerTon ?? 0, need?.Amount ?? 0,
            here is null ? null : CoordinatesDto.Distance(here, planet.Sector));
    }

    private void SelectDestination(string planetId)
    {
        SelectedDestinationId = planetId;
        ToastMessage = $"Ziel gewählt: {Player?.DiscoveredPlanets.GetValueOrDefault(planetId)?.Name}.";
    }

    private sealed record LogEntry(DateTime Time, string Message);
    private sealed record LeaderEntry(string Name, double Credits, bool IsYou);

    private sealed record ShipSnapshot(bool Travelling, string? EnemyId, int Hull)
    {
        public static ShipSnapshot Of(ShipDto ship) => new(ship.IsTravelling, ship.Cruise?.EnemyBattleShip?.Id, ship.Hull);
    }
}
