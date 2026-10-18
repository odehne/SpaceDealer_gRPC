# SpaceDealer Web UI

An interactive Blazor Web App targeting .NET 10. The current dashboard uses local demo data; it does not yet call the SpaceDealer API or persist trades.

## Run locally

From the repository root:

```powershell
dotnet run --project .\SpaceDealer.Web\SpaceDealer.Web.csproj --launch-profile http
```

Open the HTTP URL printed by ASP.NET Core (the default launch profile uses `http://localhost:5144`).

## Build

```powershell
dotnet build .\SpaceDealer.sln
```

## Explore the UI

- `Components/Pages/Home.razor` contains the dashboard, SVG galaxy map, and demo interactions.
- `wwwroot/app.css` contains the responsive visual styling.
- `Program.cs` enables Blazor Interactive Server. The UI and API are separate processes; API calls can be added once the API endpoint contracts are identified.
