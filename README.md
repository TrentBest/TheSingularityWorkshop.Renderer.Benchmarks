# TheSingularityWorkshop.Renderer.Benchmarks

![Status](https://img.shields.io/badge/status-experimental-blue)
![.NET](https://img.shields.io/badge/.NET-8-512BD4)

Performance laboratory for **The Singularity Workshop Renderer**.

This repository exists to answer a specific engineering question with measurements:

> **As world population increases, can observer-relative computation preserve the information an observer needs while doing substantially less work than evaluating every entity at full fidelity and full frequency?**

This is not a benchmark intended to produce a flattering number. It is intended to **falsify the Renderer theory if the measurements disagree with it**.

## The theory under test

The Renderer is built around Event Horizons and observer-relative computational detail.

The working hypothesis is:

> **The amount of stuff in the world should not determine the amount of computation required to show the world. Observable consequence should.**

Or, more compactly:

> **Render less by understanding less.**

This does **not** mean hiding distant objects, using fog to make geometry disappear, or pretending that an object does not exist.

A distant mountain can remain visible and mathematically represented while requiring dramatically less positional evaluation because its observer-relative consequence changes slowly.

The same principle should apply to populated worlds.

A command-center window should be able to look across a world containing an enormous number of soldiers without requiring every soldier to behave, simulate, animate, and render at the same frequency as the soldier standing ten meters from the observer.

That nearby soldier may need detailed animation, interaction, collision, equipment state, orders, and safety behavior. A distant formation may instead need semantic identity, aggregate state, low-frequency movement, occasional order propagation, event-driven state changes, and enough visual consequence to remain believable.

The soldier has not ceased to exist. The computational representation has changed.

The benchmark suite therefore separates:

- world population,
- observable population,
- representation cost,
- evaluation frequency,
- semantic importance,
- event-driven work,
- scheduling cost,
- projection/parallax consequence,
- memory allocation,
- and eventually rendering submission/GPU work.

## Running the benchmarks locally

The benchmark project consumes the Renderer as source rather than as a published NuGet package. The Renderer repository is therefore tracked as a **Git submodule** at `Renderer/`.

### Existing clone

If you already cloned this repository, from the repository root run:

```powershell
git pull
git submodule update --init --recursive
```

### New clone

Clone with the submodule in one step:

```powershell
git clone --recurse-submodules https://github.com/TrentBest/TheSingularityWorkshop.Renderer.Benchmarks.git
cd TheSingularityWorkshop.Renderer.Benchmarks
```

Then restore and build:

```powershell
dotnet restore .\TheSingularityWorkshop.Renderer.Benchmarks.slnx
dotnet build .\TheSingularityWorkshop.Renderer.Benchmarks.slnx --configuration Release
```

Run the benchmark launcher:

```powershell
dotnet run --project .\src\TheSingularityWorkshop.Renderer.Benchmarks\TheSingularityWorkshop.Renderer.Benchmarks.csproj --configuration Release
```

BenchmarkDotNet will present the available benchmark classes and filters. **Do not start with the 100-million-agent experiment unless you deliberately want the extreme workload.** Begin with the mathematical/FSM benchmarks and then move into population scaling.

The exact Renderer commit used by a benchmark checkout is pinned by the submodule. This makes a recorded result reproducible against a known Renderer revision rather than silently measuring whatever happens to be at Renderer `main` later.

### Why the source is a submodule

The Renderer is intentionally **not published to NuGet yet**. The benchmark lab needs to measure the actual Renderer implementation without forcing a premature package release.

The GitHub Actions build initializes the same submodule, so CI and local source composition use the same repository relationship.

## The theory under test

The Renderer is built around Event Horizons and observer-relative computational detail.

The working hypothesis is:

> **The amount of stuff in the world should not determine the amount of computation required to show the world. Observable consequence should.**

Or, more compactly:

> **Render less by understanding less.**

This does **not** mean hiding distant objects, using fog to make geometry disappear, or pretending that an object does not exist.

A distant mountain can remain visible and mathematically represented while requiring dramatically less positional evaluation because its observer-relative consequence changes slowly.

The same principle should apply to populated worlds.

A command-center window should be able to look across a world containing an enormous number of soldiers without requiring every soldier to behave, simulate, animate, and render at the same frequency as the soldier standing ten meters from the observer.

That nearby soldier may need detailed animation, interaction, collision, equipment state, orders, and safety behavior. A distant formation may instead need semantic identity, aggregate state, low-frequency movement, occasional order propagation, event-driven state changes, and enough visual consequence to remain believable.

The soldier has not ceased to exist. The computational representation has changed.

The benchmark suite therefore separates:

- world population,
- observable population,
- representation cost,
- evaluation frequency,
- semantic importance,
- event-driven work,
- scheduling cost,
- projection/parallax consequence,
- memory allocation,
- and eventually rendering submission/GPU work.

## What this repository is not

This is not yet a claim that the Singularity Workshop Renderer is faster than Unreal Nanite, Unity, or another production renderer.

Those systems solve much larger problems and use highly optimized GPU pipelines.

Our first job is smaller and more important:

1. measure the mathematical primitives;
2. measure the scheduling substrate;
3. measure population scaling;
4. compare naive full-rate evaluation with observer-relative work;
5. establish confidence intervals and error;
6. deliberately push into abnormally large logical populations;
7. model autonomous agent responsibilities;
8. only then introduce a rendering backend;
9. compare complete systems only when the workloads are genuinely comparable.

## Benchmark layers

### Layer 1 — Mathematical primitives

Backend-independent calculations:

- perspective projection;
- lateral parallax;
- relative parallax;
- view angle;
- Event Horizon selection.

### Layer 2 — Semantic representation

Measure the cost of semantic identity and spatial semantic lookup without putting domain vocabulary inside the Renderer.

### Layer 3 — FSM scheduling

Measure:

- cohort update cost;
- processing-group cadence;
- promotion/demotion;
- migration between compatible processing groups;
- scheduler overhead.

The published FSM_API benchmark currently gives a calibration point of approximately:

| Workload | Update cost | Allocation |
|---|---:|---:|
| 1 processing group | 305.1 ns | 360 B |
| 50 processing groups | 15,736.6 ns | 18,000 B |

These numbers are calibration data, not assumptions about the Renderer.

### Layer 4 — Population scaling

The first population experiment will compare:

**Naive**

`N_world × C_full × f_full`

against:

**Observer-relative**

`Σ(N_i × C_i × f_i)`

where each cohort has a representation cost and update frequency appropriate to its observable consequence.

Initial populations:

- 1,000
- 10,000
- 100,000
- 1,000,000
- 10,000,000

The first pass is CPU-only and intentionally backend-independent.

### Layer 5 — Complete rendering

Later experiments may add:

- representation construction;
- geometry submission;
- draw/dispatch counts;
- visible primitives;
- pixel/fragment workload;
- GPU frame time;
- streaming;
- resident/transient memory;
- frame-time variance;
- latency.

Only then does comparison with production rendering systems become meaningful.

## Experimental discipline

Every benchmark result should record:

- commit SHA;
- benchmark version;
- Renderer submodule commit;
- .NET/runtime version;
- operating system;
- CPU;
- architecture;
- configuration;
- workload parameters;
- iteration count;
- mean;
- standard deviation/error;
- allocation;
- and any environmental caveats.

Do not compare numbers from different machines as though they were identical hardware measurements.

Do not treat a mathematical microbenchmark as evidence of complete renderer performance.

Do not optimize the benchmark to make the theory win.

## Repository structure

```
/Renderer                 Renderer source submodule
/src
  TheSingularityWorkshop.Renderer.Benchmarks

/docs
  METHODOLOGY.md
  BENCHMARK_MATRIX.md
  PERFORMANCE_HYPOTHESIS.md
  RESULTS.md
```

## First questions

The initial benchmark program is designed to answer:

1. How expensive is observer-relative projection/parallax mathematics?
2. How expensive is Event Horizon selection?
3. How expensive is semantic lookup?
4. What is the cost of scheduling work at multiple temporal frequencies?
5. How does total CPU work change as world population increases?
6. At what point does selecting relevant work become more expensive than the work it avoids?
7. Does observer-relative scheduling actually produce a better scaling curve?

That last question is the one that matters.

## Relationship to the Renderer

The renderer architecture and theory live in:

**TheSingularityWorkshop.Renderer**

This repository is the empirical laboratory for those claims.

The Renderer says what we believe.

This repository measures whether we are right.

---

MIT licensed. Copyright © 2026 The Singularity Workshop.
