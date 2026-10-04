# Results

The successful first run was performed on the user's Windows 10 22H2 machine with an Intel Core i5-10400F, .NET host 8.0.31, using BenchmarkDotNet 0.16.0-preview.2 and Renderer 0.1.0-alpha.1. These values are machine-specific and are not a cross-platform performance claim.

## Renderer math baseline

| Method | Depth | Mean | Allocation |
|---|---:|---:|---:|
| ProjectHorizontal | 10 m | 6.308 ns | 0 B |
| ProjectHorizontal | 1 km | 6.228 ns | 0 B |
| ProjectHorizontal | 300 km | 6.310 ns | 0 B |
| LateralParallax | 10 m | 2.852 ns | 0 B |
| LateralParallax | 1 km | 2.851 ns | 0 B |
| LateralParallax | 300 km | 2.812 ns | 0 B |
| ViewAngle | 10 m | 20.371 ns | 0 B |
| ViewAngle | 1 km | 19.420 ns | 0 B |
| ViewAngle | 300 km | 14.240 ns | 0 B |
| SelectEventHorizon | 10 m | 35.264 ns | 64 B |
| SelectEventHorizon | 1 km | 35.404 ns | 64 B |
| SelectEventHorizon | 300 km | 38.400 ns | 64 B |

### Interpretation

The projection and parallax primitives are effectively constant-cost across the tested depth range. This supports the narrower mathematical premise that an object's distance changes the observable result without inherently increasing the cost of calculating that result.

Event Horizon selection is also tens of nanoseconds, but currently allocates 64 B per call. That allocation is an explicit optimization target and must remain visible in future comparisons.

This is **not** evidence that the complete Renderer is faster than a production renderer. It is calibration evidence for the next experiments: population scaling, scheduler responsibility, semantic work, and autonomous-world workloads.

## What comes next

The benchmark suite now moves toward the falsifiable claim:

`Naive: N_world × C_full × f_full`

versus

`Observer-relative: C_selection + C_scheduling + Σ(N_i × C_i × f_i)`

The critical measurements are absolute work, scaling slope, allocation, and information preservation—not a flattering single ratio.
No renderer performance conclusions are published here yet.

The repository has just been established as the controlled experimental environment.

Results will be added only after:

1. the benchmark implementation is reviewed for correctness;
2. the workload is defined;
3. measurements are repeated;
4. machine/runtime configuration is captured;
5. statistical output is preserved;
6. the result is compared against the stated hypothesis.

**Current status:** instrumentation and benchmark design phase.
