# Benchmark Methodology

## Principles

The benchmark suite follows five rules.

### 1. Separate theory from implementation

The first benchmarks use mathematical and synthetic workloads. No graphics API is required.

### 2. Measure the whole decision path

Eventually the benchmark must account for selection, scheduling, representation evaluation, submission, and memory.

### 3. Preserve information

A faster benchmark that changes the observable result is not an equivalent workload.

### 4. Use controlled populations

Initial population sizes are powers of ten so scaling behavior is easy to inspect.

### 5. Record uncertainty

Every published result should include the BenchmarkDotNet statistical output and the machine/runtime configuration.

## Benchmark categories

### Primitive

Measures one operation in isolation.

Useful for regression detection and cost modeling, but not sufficient for architecture claims.

### Cohort

Measures a population assigned to a known update cadence.

Useful for testing temporal detail and scheduling.

### Population

Measures complete synthetic worlds.

Useful for testing scaling, selection overhead, avoided work, and memory behavior.

### End-to-end

Measures a real observer workload through the renderer.

Useful for system-level comparison.

## Warmup and measurement

BenchmarkDotNet controls process isolation, warmup, measurement, and statistical analysis.

Do not replace BenchmarkDotNet timing with `Stopwatch` loops for final published measurements.

## CPU-only first

The first benchmark phase intentionally avoids GPU dependencies.

This isolates the theory's computational model before graphics driver, API, shader, rasterization, and GPU scheduling effects are introduced.

## Result policy

Results belong in `docs/RESULTS.md` only after the exact benchmark configuration has been recorded.

A result should never be presented without its workload definition.

## Reproducibility

The benchmark executable should be runnable from a clean checkout with:

```text
dotnet restore
dotnet run -c Release --project src/TheSingularityWorkshop.Renderer.Benchmarks
```

For a targeted benchmark:

```text
dotnet run -c Release --project src/TheSingularityWorkshop.Renderer.Benchmarks -- --filter *Parallax*
```
