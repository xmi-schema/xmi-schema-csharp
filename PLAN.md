Plan: Make cross-section/material optional for structural curve members

1) Map coupling points (code scan)
- Locate factory/manager signatures and relationship wiring that force cross-section/material on `XmiStructuralCurveMember`, `XmiBeam`, `XmiColumn`.

2) Design changes
- Decide nullable parameters and safe relationship creation (skip when null); keep equality/serialization untouched.
- Update interface contracts (IXmiManager/XmiManager/XmiModel) and any helpers/tests/docs to match optional cross-section/material behavior.

3) Implement
- Update factories to accept null cross-section/material and guard `XmiHasCrossSection` creation; adjust beams/columns linkage if needed.
- Add/adjust tests for creating curve members without cross-section/material and ensure existing cases still pass.

4) Validate
- Run `dotnet build` (and tests if/when present) to confirm compile and behaviors.
