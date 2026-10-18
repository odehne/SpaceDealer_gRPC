# SpaceDealerModels\SpaceDealerModels.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 35
- **Number of Files with Incidents**: 2
- **Lines of Code**: 2744
- **Estimated LOC to modify**: 19+ (at least 0,7% of the project)

## Related Projects

**Depended on by (1)** — projects that reference this one:

- [C:\Git\priv\SpaceDealer\SpaceDealerService\SpaceDealerService.csproj](../projects/SpaceDealerService.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;SpaceDealerService.csproj</b><br/><small>net8.0</small>"]
        click P1 "../projects/SpaceDealerService.md"
    end
    subgraph current["SpaceDealerModels.csproj"]
        MAIN["<b>📦&nbsp;SpaceDealerModels.csproj</b><br/><small>net8.0</small>"]
        click MAIN "../projects/SpaceDealerModels.md"
    end
    P1 --> MAIN

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 19 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 2403 |  |
| ***Total APIs Analyzed*** | ***2422*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

