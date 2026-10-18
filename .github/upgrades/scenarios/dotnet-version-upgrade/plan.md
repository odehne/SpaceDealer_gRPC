# .NET 10 Upgrade Plan

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Upgrade Strategy | All-at-Once | Two low-difficulty modern .NET projects with one project reference can be upgraded in a single operation. |
| Nullable Reference Types | Leave Disabled | The solution has compatibility work to address, and nullable analysis was not requested as part of this migration. |
| Test Coverage | Skip | Test generation is opt-in, and the root solution contains no test project. |

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 2 projects, both on .NET 8, with a clear single-project reference.

## Projects

- **Class library**: `SpaceDealerModels/SpaceDealerModels.csproj`
- **ASP.NET Core service**: `SpaceDealerService/SpaceDealerService.csproj`

## Tasks

### 01-upgrade-solution: Upgrade both root-solution projects to .NET 10

Upgrade `SpaceDealerModels` and `SpaceDealerService` together from `net8.0` to `net10.0`. Both projects use SDK-style project files, and the service references the model library, so this is a single atomic framework update rather than a tiered migration. Verify the .NET 10 SDK and any `global.json` constraints before changing the projects.

The assessment found 27 resolved packages and no incompatible packages; it recommends updating `Newtonsoft.Json` from 13.0.3 to 13.0.4 in both projects. It also reports 19 potential source-compatibility findings in `SpaceDealerModels/Units/DbShipFeature.cs`, all involving `System.Data.SqlClient.SqlDataReader`, although that package is assessed as compatible. Check actual restore/build results before changing those calls. Do not enable nullable reference types or modify projects outside the root solution.

**Done when**: Both root-solution projects target `net10.0`, the recommended Newtonsoft.Json update is applied, package restore succeeds, and the full solution builds with no errors.

### 02-final-validation: Validate the upgraded solution

Run final validation against `SpaceDealer.sln` after the atomic upgrade: confirm a clean restore and solution build, run any tests included in the root solution, and record any remaining warnings or validation limitations. Do not treat the legacy projects outside the root solution as part of this migration, and leave all pre-existing working-tree changes unstaged and uncommitted.

**Done when**: The root solution builds successfully, applicable tests pass (or the absence of in-scope test projects is recorded), and no unrelated worktree changes have been staged or committed.
