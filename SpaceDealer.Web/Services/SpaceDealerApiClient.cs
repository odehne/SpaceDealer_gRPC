using System.Net.Http.Json;

namespace SpaceDealer.Web.Services;

/// <summary>Typed HttpClient for the SpaceDealerService REST API.</summary>
public sealed class SpaceDealerApiClient(HttpClient http)
{
    private static string Q(string value) => Uri.EscapeDataString(value);

    public Task<ApiResult<PlayerDto>> LoginAsync(string playerName, CancellationToken ct = default)
        => SendAsync<PlayerDto>(HttpMethod.Post, $"players/new?playerName={Q(playerName)}", ct);

    public Task<ApiResult<PlayerDto>> GetPlayerAsync(string playerId, CancellationToken ct = default)
        => SendAsync<PlayerDto>(HttpMethod.Get, $"players/{Q(playerId)}", ct);

    public async Task<ApiResult<Dictionary<string, PlayerDto>>> GetPlayersAsync(CancellationToken ct = default)
    {
        var result = await SendAsync<Dictionary<string, PlayerDto>>(HttpMethod.Get, "players", ct);
        // The service answers 404 while no player exists yet.
        return result.Ok ? result : new(new Dictionary<string, PlayerDto>());
    }

    public async Task<ApiResult<int>> GetPlanetCountAsync(CancellationToken ct = default)
    {
        var result = await SendAsync<Dictionary<string, object>>(HttpMethod.Get, "planets", ct);
        return result.Ok ? new(result.Value?.Count ?? 0) : new(0, result.Error);
    }

    public Task<ApiResult<ShipDto>> NewShipAsync(string playerId, string shipName, CancellationToken ct = default)
        => SendAsync<ShipDto>(HttpMethod.Post, $"players/{Q(playerId)}/ships/new?shipName={Q(shipName)}", ct);

    public Task<ApiResult<bool>> StartTravelAsync(string playerId, string shipId, string destinationPlanetId, CancellationToken ct = default)
        => SendAsync<bool>(HttpMethod.Post, $"players/{Q(playerId)}/ships/{Q(shipId)}/travel/to?destinationPlanetUID={Q(destinationPlanetId)}", ct);

    public Task<ApiResult<BattleResultDto>> AttackAsync(string playerId, string shipId, CancellationToken ct = default)
        => SendAsync<BattleResultDto>(HttpMethod.Post, $"players/{Q(playerId)}/ships/{Q(shipId)}/attack", ct);

    public Task<ApiResult<BattleResultDto>> DefendAsync(string playerId, string shipId, CancellationToken ct = default)
        => SendAsync<BattleResultDto>(HttpMethod.Post, $"players/{Q(playerId)}/ships/{Q(shipId)}/defend", ct);

    // TradingController combines [Route("trading")] on the class with "trading/..." on each action,
    // so the effective path really is /trading/trading/...
    public Task<ApiResult<double>> BuyAsync(string playerId, string shipId, string planetId, string productId, int quantity, CancellationToken ct = default)
        => SendAsync<double>(HttpMethod.Get, $"trading/trading/{Q(playerId)}/{Q(shipId)}/{Q(planetId)}/{Q(productId)}/buy?quantity={quantity}", ct);

    public Task<ApiResult<double>> SellAsync(string playerId, string shipId, string planetId, string productId, int quantity, CancellationToken ct = default)
        => SendAsync<double>(HttpMethod.Get, $"trading/trading/{Q(playerId)}/{Q(shipId)}/{Q(planetId)}/{Q(productId)}/sell?quantity={quantity}", ct);

    // tradeType "Buy" = the best planet that NEEDS the product (highest price for the player selling there).
    public Task<ApiResult<PlanetDto>> BrowseBestBuyerAsync(string playerId, string shipId, string productId, CancellationToken ct = default)
        => SendAsync<PlanetDto>(HttpMethod.Get, $"trading/trading/{Q(playerId)}/{Q(shipId)}/browse?productUID={Q(productId)}&tradeType=Buy", ct);

    // Same double prefix as above: GameController is [Route("game")] with "game/save" on the action.
    public Task<ApiResult<bool>> SaveGameAsync(CancellationToken ct = default)
        => SendAsync<bool>(HttpMethod.Put, "game/game/save", ct);

    private async Task<ApiResult<T>> SendAsync<T>(HttpMethod method, string url, CancellationToken ct)
    {
        try
        {
            using var request = new HttpRequestMessage(method, url);
            using var response = await http.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
                return new(default, await ReadErrorAsync(response, ct));

            var value = await response.Content.ReadFromJsonAsync<T>(ct);
            return new(value);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return new(default, "Der SpaceDealer-Dienst ist nicht erreichbar.");
        }
        catch (Exception ex) when (ex is TaskCanceledException or System.Text.Json.JsonException)
        {
            return new(default, "Unerwartete Antwort vom SpaceDealer-Dienst.");
        }
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        var body = await response.Content.ReadAsStringAsync(ct);
        // Unhandled service exceptions come back as a stack trace; only show real messages.
        var firstLine = body.Split('\n', 2)[0].Trim().Trim('"');
        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError
            || string.IsNullOrWhiteSpace(firstLine)
            || firstLine.StartsWith("System.", StringComparison.Ordinal))
        {
            return $"Der Dienst meldet einen Fehler ({(int)response.StatusCode}).";
        }
        return firstLine;
    }
}
