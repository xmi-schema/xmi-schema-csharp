# Usage Examples

This folder houses runnable samples that demonstrate how downstream agents can consume the `XmiSchema.Core` library. Each project references the main library so you can explore real code, run it locally, and copy/paste snippets into your own automation.

## StructuralGraphSample
- **Location:** `tests/Examples/StructuralGraphSample`
- **Scenario:** Builds a minimal `XmiManager` instance, populates storeys, materials, structural members, and prints the resulting graph JSON.
- **Run:**
  ```bash
  dotnet run --project tests/Examples/StructuralGraphSample/StructuralGraphSample.csproj
  ```
- **Highlights:** Shows how to chain the creation helpers (`CreateMaterial`, `CreateStructuralCurveMember`, `BuildJson`) and is a good starting point for creating regression fixtures or docs snippets.
