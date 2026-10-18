# SpaceDealerService\SpaceDealerService.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 27
- **Number of Files with Incidents**: 1
- **Lines of Code**: 2686
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

## Related Projects

**Depends on (1)** — projects this one references:

- [C:\Git\priv\SpaceDealer\SpaceDealerModels\SpaceDealerModels.csproj](../projects/SpaceDealerModels.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["SpaceDealerService.csproj"]
        MAIN["<b>📦&nbsp;SpaceDealerService.csproj</b><br/><small>net8.0</small>"]
        click MAIN "../projects/SpaceDealerService.md"
    end
    subgraph downstream["Dependencies (1)"]
        P2["<b>📦&nbsp;SpaceDealerModels.csproj</b><br/><small>net8.0</small>"]
        click P2 "../projects/SpaceDealerModels.md"
    end
    MAIN --> P2

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 8203 |  |
| ***Total APIs Analyzed*** | ***8203*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

