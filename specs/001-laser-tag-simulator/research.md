# Research: Laser Tag Simulator

**Date**: 2025-12-09
**Input**: [Implementation Plan](plan.md)

## Summary

This document confirms the technology choices for the Laser Tag Simulator. All technical decisions were predefined by the project constitution and the detailed feature specification, so no external research was required.

---

### Decision 1: Technology Stack

*   **Decision**: Use C# with .NET 9 and Windows Forms (WinForms).
*   **Rationale**: This is explicitly required by the project constitution and the feature specification. It provides the necessary tools to build a native Windows desktop application.
*   **Alternatives Considered**: None. The stack was non-negotiable.

### Decision 2: JSON Serialization

*   **Decision**: Use `System.Text.Json` for all JSON operations. Configure it with `JsonNamingPolicy.CamelCase`.
*   **Rationale**: This is the modern, built-in .NET library for JSON manipulation. The camelCase policy is required to match the data contract of the receiving application (`GOTF-laser-tag`).
*   **Alternatives Considered**: `Newtonsoft.Json`. Rejected because `System.Text.Json` is the current standard and sufficient for this project's needs, avoiding an external dependency.

### Decision 3: HTTP Communication

*   **Decision**: Use `HttpClient` for sending HTTP POST requests.
*   **Rationale**: `HttpClient` is the standard and recommended way to perform HTTP requests in .NET. It is flexible, efficient, and well-supported.
*   **Alternatives Considered**: None. `HttpClient` is the clear choice for this task.
