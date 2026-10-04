## Current measured baseline

The first successful math run establishes the following machine-specific baseline:

- projection: ~6.2–6.3 ns, 0 B;
- lateral parallax: ~2.8 ns, 0 B;
- view angle: ~14–20 ns, 0 B;
- Event Horizon selection: ~35–38 ns, 64 B.

Depth from 10 m to 300 km did not materially increase the cost of the mathematical primitives. Event Horizon selection is the first identified allocation hotspot.

The next benchmark pass must expand from primitive timing into population and scheduling scaling. The benchmark matrix therefore treats **selection cost, scheduling cost, and representation work as separate charges** rather than assuming observer-relative work is free.

# Benchmark Matrix

| Layer | Benchmark | Variable | First values | Question |
|---|---|---|---|---|
| Math | Projection | depth | 10m, 1km, 300km | Is projection cheap enough to apply broadly? |
| Math | Parallax | depth | 10m → 300km | Does observable motion fall as predicted? |
| Selection | Horizon | distance | 0 → far | What does representation selection cost? |
| Semantic | Anchor lookup | anchors | 2 → 10,000 | How does semantic lookup scale? |
| Scheduler | Cohorts | population | 1k → 10M | What does cadence-aware work cost? |
| Population | Naive | population | 1k → 10M | Full-rate baseline |
| Population | Observer-relative | population | 1k → 10M | Reduced computational responsibility |
| Transition | Promotion/demotion | crossings | low → high | What is horizon churn cost? |
| Memory | Population | population | 1k → 10M | What does resident state cost? |
| Agent simulation | Standing orders | population | 1k → 1M+ | Can autonomous behavior remain inexpensive outside the observer's high-detail responsibility? |
| Agent simulation | General orders | population | 1k → 1M+ | What is the cost of propagating shared intent without evaluating every agent at full rate? |
| Agent simulation | Safety/procedure state | events | sparse → dense | Can event-driven state changes replace continuous evaluation where appropriate? |
| Extreme population | Command-center window | logical population | 1M → 100M+ | How much world can remain observable from one bounded observer context? |
| Extreme population | Research demonstration | nearby cohort | 1k → 100k+ | Can a local high-detail experience coexist with a huge low-detail world? |

## Required comparison

The first decisive comparison is:

`Naive: N × C_full × f_full`

versus:

`Observer-relative: Σ N_i × C_i × f_i`

The benchmark must report both absolute work and ratio.

## Later GPU matrix

When a backend exists:

- CPU selection time;
- CPU scheduling time;
- render-thread submission;
- draw/dispatch count;
- visible triangles/primitives;
- pixels/fragments;
- GPU frame time;
- GPU memory;
- streaming bandwidth;
- frame-time variance;
- latency.

These are separate measurements, not a single invented "performance score."


## Extreme-world workload

The benchmark suite must deliberately include workloads much larger than a conventional scene stress test.

A useful target is a command-center observer looking across a populated installation or battlefield. Millions of soldiers may exist in the world while only a fraction are close enough, visible enough, semantically important enough, or temporally active enough to require high-frequency work.

A second target is the same world viewed from inside a research building. The observer should be able to watch a nearby demonstration containing many autonomous agents while the rest of the world continues to exist at lower computational cadence.

These workloads are not intended to reduce soldiers to anonymous dots. The eventual agent model should preserve meaningful state such as identity, standing orders, general orders, local procedures and safety state, FSM state, movement, role, and observer-visible activity.

### Logical population versus instantiated population

For extreme populations, distinguish:

`N_logical` — entities that exist in the simulated world

from:

`N_materialized` — entities that currently require instantiated computational state

and:

`N_observable` — entities whose consequences are relevant to the current observer.

This distinction is central to the experiment.

A 100-million-agent scenario should not automatically mean 100 million heavyweight managed objects. If compact procedural, cohort, semantic, or event-driven state can represent the population faithfully, that is part of what the benchmark measures.

## Required comparison

The first decisive comparison is:

`Naive: N × C_full × f_full`

versus:

`Observer-relative: C_selection + C_scheduling + Σ N_i × C_i × f_i`

The observer-relative measurement must charge itself for selection and scheduling.

## Information preservation

Extreme-world benchmarks must record more than elapsed time.

At minimum track logical, materialized, observable, semantically active, high-frequency, low-frequency, and event-driven populations, representation transitions, scheduling work, memory, and an explicit observable-information/error measure.
