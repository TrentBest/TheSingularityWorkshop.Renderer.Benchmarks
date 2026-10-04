# Autonomous World Workload

This document defines the first benchmark workload for the “abnormally large world” problem.

The goal is not to claim that 100 million soldiers can already be rendered. The goal is to establish a measurable question:

> Can 100 million logically existing agents remain causally meaningful while only a small fraction require high-frequency computational responsibility?

## 1. The agent is more than a visual primitive

A synthetic soldier is represented by compact state:

- identity;
- standing order;
- general order;
- local safety/procedure state;
- FSM state;
- movement state;
- observer-relative distance band;
- event participation.

The benchmark deliberately does not create a managed object containing all of this for every logical agent.

A world may contain:

- N_logical — agents that exist in the world model;
- N_materialized — agents requiring instantiated state;
- N_observable — agents whose consequences matter to the current observer;
- N_high_frequency — agents requiring detailed repeated evaluation;
- N_event_driven — agents awaiting or responding to discrete events.

These populations are expected to differ substantially.

## 2. The first extreme-population experiment

AutonomousWorldBenchmarks uses:

- 1,000,000 logical agents;
- 10,000,000 logical agents;
- 100,000,000 logical agents.

The population is generated deterministically from an agent identity. This gives the benchmark a reproducible world without requiring hundreds of millions of heavyweight objects.

Two workloads are compared.

### Full-rate evaluation

Every logical agent evaluates:

1. standing order;
2. general order;
3. safety state;
4. FSM state;
5. movement state.

This is the deliberately expensive baseline:

    N_logical × C_full × f_full

### Observer-relative evaluation

Every logical agent still participates in the logical population traversal, but responsibility is differentiated:

- a bounded near cohort receives high-frequency work;
- a bounded observable cohort receives lower-cost semantic work;
- a sparse event-driven cohort receives event-state work;
- the remainder remains logically represented without full-rate evaluation.

The benchmark therefore tests a stronger claim than “don’t render distant soldiers.”

It tests:

> The soldier still exists. The computational representation changes.

## 3. Why the logical traversal remains

The observer-relative benchmark intentionally does not make the 100M case magically O(1).

Every logical identity is still visited so that the first experiment measures a conservative compact-population model.

This gives us a clear next wall.

If merely traversing 100M logical identities is already too expensive, the next optimization is not rendering. It is population indexing, spatial partitioning, cohort scheduling, procedural regions, and event routing.

That negative result is useful.

## 4. The next layers

This first workload is intentionally synthetic. It does not yet model:

- spatial cells;
- real FSM_API instances for every agent;
- order propagation through a hierarchy;
- collision;
- formation movement;
- visibility/occlusion;
- procedural reconstruction;
- GPU submission;
- actual soldier meshes or animation.

The intended progression is:

    compact logical population
            |
    spatial/cohort partitioning
            |
    standing/general orders
            |
    FSM state
            |
    event propagation
            |
    observer-relative promotion
            |
    procedural representation
            |
    actual rendering

## 5. Causal safety experiment

A later workload should inject an incident such as:

    tank intersects marching formation
            |
    incident detected
            |
    local safety state changes
            |
    standing/general procedure updated
            |
    affected cohort promoted
            |
    new behavior propagates
            |
    unaffected world remains at its prior cadence

The benchmark must measure the propagation cost separately from continuous simulation cost.

A competent world should be able to establish a safety procedure before the first incident if the required information is available. An unprepared world may discover the need through an incident.

The benchmark should eventually allow both paths and measure:

- event detection;
- affected population;
- state propagation;
- FSM transitions;
- promotion to higher cadence;
- return to lower cadence after the situation stabilizes.

## 6. What would falsify the hypothesis?

The strategy is not successful merely because distant entities are cheap.

It fails if:

- logical-state traversal dominates the saved work;
- cohort selection costs as much as full simulation;
- order propagation wakes most of the world unnecessarily;
- FSM scheduling becomes the bottleneck;
- promotion/demotion churn dominates;
- compact representation loses required information;
- correctness requires materializing most of the population;
- event handling becomes effectively global.

A negative result should remain in the benchmark history.

## 7. The rendering question

Only after these layers are measurable should the workload become a graphics benchmark.

For the command-center observer, the final question becomes:

    How much of the enormous world must actually become
    observable computational consequence at this instant?

For the nearby research demonstration:

    Can a small cohort become highly detailed while the
    rest of the world continues to exist at appropriate
    lower computational responsibility?

That is the connection between autonomous simulation and Event Horizons.
