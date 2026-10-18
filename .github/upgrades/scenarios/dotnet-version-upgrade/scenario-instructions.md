# Scenario Instructions

## Confirmed parameters

- Scenario: `dotnet-version-upgrade`
- Scope: the two projects in `SpaceDealer.sln` (`SpaceDealerService` and `SpaceDealerModels`)
- Assessment input mode: `solution`
- Assessment paths: `C:\Git\priv\SpaceDealer\SpaceDealer.sln`
- Target framework: `net10.0`
- Execution mode: automatic

## Source Control

- Repository: Git
- Working branch: `master`
- Commit strategy: do not commit (explicitly confirmed; preserve existing unrelated changes)

## Upgrade Options

### Strategy
- Upgrade Strategy: All-at-Once

### Modernization
- Nullable Reference Types: Leave Disabled

### Reliability
- Test Coverage: Skip

## Execution constraints

- Preserve all pre-existing working-tree changes. Do not revert, stage, or commit them.
- Apply framework changes only to the two projects in the root solution.
- Validate the root solution build and relevant tests after implementation.

## Strategy

**Selected**: All-at-Once
**Rationale**: The assessment found two low-difficulty modern .NET projects with one project reference and no incompatible packages.

### Execution Constraints

- Upgrade both projects in one atomic pass; do not introduce project-tier phases.
- Verify .NET SDK and `global.json` compatibility before editing project files.
- Update target frameworks and the recommended package version, then restore and build the full root solution.
- Resolve only compatibility issues demonstrated by restore, compilation, or tests; the flagged SqlClient APIs belong to a package assessed as compatible.
- Keep all pre-existing changes unstaged and uncommitted.
