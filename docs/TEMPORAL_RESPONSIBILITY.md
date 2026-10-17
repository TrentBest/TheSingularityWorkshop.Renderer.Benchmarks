# Temporal Responsibility Experiment

## Question

Can a world keep a large logical population while updating only the cohorts whose representation is due at the current tick—and what projection-space error does that cadence introduce?

This experiment isolates evaluation cadence. It does not attempt to measure a complete Renderer → Visualize → GUI.SVG or GPU pipeline.

## Workload

```text
Population:       1,000 / 10,000 / 100,000 / 1,000,000 entities
Measurement span: 63 ticks per benchmark invocation
Observer sample:  32 deterministic entity indices
Cohorts:          cadence 1 / 4 / 16 / 64 ticks
```

Synthetic entities have a stable depth, a small deterministic lateral velocity, and a cadence selected by depth. The full-rate method evaluates every entity at every tick. The observer-relative method updates only cohorts due on that tick and reads the same fixed-size sample at the end. The 63-tick window is intentionally not divisible by all cohort cadences; a 64-tick window would refresh every cohort at its final tick and make the boundary sample artificially flattering.

The entity state and arrays are allocated during `GlobalSetup`. MemoryDiagnoser records benchmark allocations; setup allocations are not the per-invocation cost.

## Measurements

The suite includes three distinct workloads:

- **`FullRateWindow`:** evaluate all entities on every tick, then read the fixed observer sample.
- **`ObserverRelativeWindow`:** update only cohorts due on each tick, then read the same fixed observer sample.
- **`ObserverRelativeMeanAbsoluteErrorWindow`:** on every tick, update due cohorts, evaluate the full-rate reference for the same 32 sample entities, and accumulate the mean absolute difference between reference and last observer-relative values.

The error metric is measured in the combined output space of `ParallaxModel.ProjectHorizontal` plus `ParallaxModel.ViewAngle`. It is a synthetic projection-space error, **not** a pixel error, perceptual quality metric, world-space position error, or universal fidelity guarantee. It provides a first observable stale-value measure under this particular motion/cadence model.

The observer-relative update still loops over the small set of cohorts each tick. It does not scan every entity to decide whether that entity is due. The sample readout is fixed at 32 entities rather than proportional to population.

## What it does not prove

This is a synthetic CPU scheduling experiment, not a production simulation or full rendering benchmark. It does not yet measure:

- error by each individual cadence/distance band as a separately reported result;
- selection and cohort-transition cost in a changing world;
- the cost of writing a complete visible frame;
- causal/event propagation through FSM_API;
- CPU parallelism, GPU dispatch, or device memory;
- browser frame time or user-visible latency;
- whether this projection-space error is acceptable for a real scene.

The error benchmark does extra reference work for 32 sampled entities every tick. Its elapsed time is therefore **not** a fair timing comparison against either scheduling method; use its returned value as the error metric, not as a performance result. BenchmarkDotNet timing for the two scheduling methods remains the performance comparison. No result should be recorded until the suite has been run on a controlled machine and the output reviewed.

## Run

```powershell
dotnet run --project .\src\TheSingularityWorkshop.Renderer.Benchmarks\TheSingularityWorkshop.Renderer.Benchmarks.csproj --configuration Release --filter "*TemporalResponsibilityBenchmarks*"
```

Run the complete suite on a controlled machine and retain the BenchmarkDotNet artifacts before recording any result in `RESULTS.md`. Do not infer speedup from the model or from operation counts alone.

## Decision rule

The useful result is not merely that fewer evaluations occur. The benchmark must show whether the avoided evaluation cost exceeds cohort scheduling overhead as population scales. The fidelity result must then show what information is lost at the tested cadences. If the total cost does not improve, or the measured error exceeds a subsequently defined scene-specific budget, the hypothesis is weakened.

Before making an information-preservation claim, add cadence-specific error reporting and a declared acceptable error budget tied to an actual representation/use case. The present aggregate sampled error is a diagnostic baseline only.
