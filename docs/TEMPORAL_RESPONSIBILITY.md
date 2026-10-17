# Temporal Responsibility Experiment

## Question

Can a world keep a large logical population while updating only the cohorts whose representation is due at the current tick?

This experiment isolates one variable: **evaluation cadence**. It does not attempt to measure a complete Renderer → Visualize → GUI.SVG or GPU pipeline.

## Workload

```text
Population:       1,000 / 10,000 / 100,000 / 1,000,000 entities
Measurement span: 64 ticks per benchmark invocation
Observer sample:  32 deterministic entity indices
Cohorts:          cadence 1 / 4 / 16 / 64 ticks
```

Synthetic entities have a stable depth, a small deterministic lateral velocity, and a cadence selected by depth. Both methods evaluate the same projection and view-angle functions. The full-rate method evaluates every entity at every tick. The observer-relative method updates only cohorts due on that tick and reads the same fixed-size sample at the end. The 63-tick window is intentionally not divisible by all cohort cadences; a 64-tick window would refresh every cohort at its final tick and make the boundary sample artificially flattering.

The entity state and arrays are allocated during `GlobalSetup`. MemoryDiagnoser records benchmark allocations; setup allocations are not the per-invocation cost.

## What it measures

- full-rate evaluation time across a fixed 63-tick window;
- cadence-aware cohort update time across the same window;
- per-invocation allocations;
- scaling as the logical population grows.

The observer-relative method still loops over the small set of cohorts each tick. It does not scan every entity to decide whether that entity is due. The sample readout is fixed at 32 entities rather than proportional to population.

## What it does not yet prove

This is a synthetic CPU scheduling experiment, not a production simulation or full rendering benchmark. It does not yet measure:

- selection and cohort-transition cost in a changing world;
- the cost of writing a complete visible frame;
- error between stale and current representation values;
- causal/event propagation through FSM_API;
- CPU parallelism, GPU dispatch, or device memory;
- browser frame time or user-visible latency.

The sampled output is a workload sink to keep evaluated values observable to the benchmark; it is **not** a fidelity or error metric. A follow-up correctness experiment must compare observer-relative samples against a full-rate reference and publish a defined error budget before making information-preservation claims.

## Run

```powershell
dotnet run --project .\src\TheSingularityWorkshop.Renderer.Benchmarks\TheSingularityWorkshop.Renderer.Benchmarks.csproj --configuration Release --filter "*TemporalResponsibilityBenchmarks*"
```

Run the complete suite on a controlled machine and retain the BenchmarkDotNet artifacts before recording any result in `RESULTS.md`. Do not infer speedup from the model or from operation counts alone.

## Decision rule

The useful result is not merely that fewer evaluations occur. The benchmark must show whether the avoided evaluation cost exceeds cohort scheduling overhead as population scales. A later fidelity/error experiment must show what information is lost, if any, at each cadence. If the total cost does not improve, or the error exceeds the defined budget, the hypothesis is weakened.
