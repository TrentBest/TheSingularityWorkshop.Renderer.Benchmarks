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
