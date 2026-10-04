# Performance Hypothesis

## Primary hypothesis

The renderer should spend computational work in proportion to **observable consequence**, not raw world population.

For a population divided into computational cohorts:

`C_total/s ≈ Σ N_i × C_i × f_i`

where:

- `N_i` = population in cohort `i`;
- `C_i` = cost of evaluating its representation;
- `f_i` = evaluation frequency.

A conventional full-rate baseline is approximately:

`C_naive/s ≈ N_world × C_full × f_full`

The theory predicts that the observer-relative curve should grow substantially more slowly when most of the world has low observable consequence.

## Important constraint

The observer-relative strategy cannot simply ignore the cost of deciding what matters.

Therefore the experiment must eventually measure:

`C_observer = C_selection + C_scheduling + C_representation + C_submission`

A strategy that saves representation work but spends equivalent or greater work selecting entities has not demonstrated an advantage.

## Mountain case

A distant landmark must remain visible.

For a pinhole camera:

`u = fX/Z`

Lateral observer movement produces:

`|Δu| = f|ΔC|/Z`

For two depths:

`|Δu_relative| = f|ΔC||1/Z_near - 1/Z_far|`

Therefore a distant object can remain geometrically meaningful while its apparent positional change becomes very small.

The benchmark should verify that this allows temporal work to decrease without making visibility itself a distance cutoff.

## Falsification criteria

The theory is weakened if:

1. observer-relative scheduling does not reduce total work;
2. selection cost dominates the avoided representation work;
3. horizon transitions cause excessive churn;
4. memory overhead overwhelms CPU savings;
5. visible information cannot be preserved within a defined error budget;
6. scaling remains effectively proportional to full-fidelity world population.

A negative result is valuable. It tells us where the model is wrong.

## What success would establish

A successful first experiment would not prove that the Renderer beats a production engine.

It would establish that:

- observer-relative representation is computationally measurable;
- temporal frequency is a useful independent optimization dimension;
- distant visible objects can remain represented at lower computational cadence;
- total work can scale with observable workload rather than full world population under controlled conditions.


## Autonomous-world extension

The extreme-world experiment extends the hypothesis from visible geometry to meaningful simulated agents.

A soldier can have standing orders, general orders, local procedure/safety state, movement state, role, and event-driven responses. Those responsibilities do not all need the same evaluation frequency.

The benchmark therefore introduces three distinct populations:

- `N_logical`: agents that exist in the world;
- `N_materialized`: agents requiring active instantiated state;
- `N_observable`: agents whose current consequences matter to the observer.

The stronger hypothesis is:

> A large world can remain semantically and causally alive without every entity requiring full-rate, full-fidelity computation.

This is deliberately testable.

A command-center view should preserve large-scale formations and meaningful activity. A nearby research demonstration may promote a much smaller cohort into high-frequency simulation and rendering. When the observer leaves, those agents can demote without ceasing to exist.

This also creates an important simulation test: state changes caused by events should propagate through the FSM system without forcing unrelated agents into high-frequency evaluation.

For example, a safety procedure may become relevant after an incident. The benchmark should measure the cost of propagating that changed order/procedure state separately from the cost of continuously evaluating every soldier in anticipation of the incident.

The benchmark must not assume that this strategy wins. If maintaining causal correctness, selection, scheduling, or state propagation costs as much as full-rate simulation, that is a falsification result.
