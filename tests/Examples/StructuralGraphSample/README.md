# StructuralGraphSample

A runnable console app that exercises the public `XmiManager` API.

## Scenario
1. Bootstraps a manager and model.
2. Creates storeys, materials, points, and curve members using the helper methods.
3. Emits the resulting JSON graph via `BuildJson`.

## Run Instructions
```bash
dotnet run --project tests/Examples/StructuralGraphSample/StructuralGraphSample.csproj
```

Use this sample as a blueprint for scripting data exports or smoke-testing NuGet packages in downstream repositories.
