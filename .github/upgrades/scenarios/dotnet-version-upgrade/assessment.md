# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

Detailed findings live alongside this file in `assessment/`. This page is the index: read it first, then open only the documents you need.

## Table of Contents

- [Executive Summary](#executive-summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Detailed Reports](#detailed-reports)
  - [Projects Relationship Graph](assessment/project-graph.md)
  - [Aggregate NuGet packages details](assessment/nuget/aggregate-packages.md)
  - [Most Frequent API Issues (complete list)](assessment/api-issues/most-frequent-api-issues.md)
  - [Project Details](#project-details)

## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 2 | All require upgrade |
| Total NuGet Packages | 27 | 1 need upgrade |
| Total Code Files | 57 |  |
| Total Code Files with Incidents | 3 |  |
| Total Lines of Code | 5430 |  |
| Total Number of Issues | 23 |  |
| Proposed Target Framework | net10.0 |  |
| Estimated LOC to modify | 19+ | at least 0,3% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Test Coverage | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [SpaceDealerModels\SpaceDealerModels.csproj](assessment/projects/SpaceDealerModels.md) | net8.0 | 🟢 Low | 🧪 Recommended | 1 | 19 | 0 | 19+ | ClassLibrary, Sdk Style = True |
| [SpaceDealerService\SpaceDealerService.csproj](assessment/projects/SpaceDealerService.md) | net8.0 | 🟢 Low | — | 1 | 0 | 0 |  | AspNetCore, Sdk Style = True |

🧪 **Test Coverage** — projects risky enough to add behavior-locking tests before upgrading, to catch regressions the upgrade may introduce. Requires the **dotnet-test** plugin.

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 26 | 96,3% |
| ⚠️ Incompatible | 0 | 0,0% |
| 🔄 Upgrade Recommended | 1 | 3,7% |
| ***Total NuGet Packages*** | ***27*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 19 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 10606 |  |
| ***Total APIs Analyzed*** | ***10625*** |  |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| M:System.Data.SqlClient.SqlDataReader.GetOrdinal(System.String) | 9 | 47,4% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetBoolean(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetDateTime(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetGuid(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetInt32(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetInt64(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetSqlMoney(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetSqlXml(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.GetString(System.Int32) | 1 | 5,3% | Source Incompatible |
| M:System.Data.SqlClient.SqlDataReader.IsDBNull(System.Int32) | 1 | 5,3% | Source Incompatible |

The table above is the top 10. See [the complete list](assessment/api-issues/most-frequent-api-issues.md) for every affected API.

## Detailed Reports

- [Projects Relationship Graph](assessment/project-graph.md)
- [Aggregate NuGet packages details](assessment/nuget/aggregate-packages.md)
- [Most Frequent API Issues (complete list)](assessment/api-issues/most-frequent-api-issues.md)

### Project Details

- [SpaceDealerModels\SpaceDealerModels.csproj](assessment/projects/SpaceDealerModels.md)
- [SpaceDealerService\SpaceDealerService.csproj](assessment/projects/SpaceDealerService.md)


