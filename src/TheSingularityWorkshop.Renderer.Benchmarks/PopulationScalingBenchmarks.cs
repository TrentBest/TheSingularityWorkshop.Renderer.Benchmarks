using BenchmarkDotNet.Attributes;
using TheSingularityWorkshop.Renderer;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

/// <summary>
/// CPU-only synthetic experiment comparing full-rate representation evaluation
/// with observer-relative temporal responsibility.
/// </summary>
[MemoryDiagnoser]
public class PopulationScalingBenchmarks
{
    private SyntheticEntity[] _entities = [];
    private double _sink;

    [Params(1_000, 10_000, 100_000, 1_000_000)]
    public int Population { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _entities = new SyntheticEntity[Population];

        for (var i = 0; i < _entities.Length; i++)
        {
            // Deterministic spatial distribution. The benchmark is deliberately
            // not a visibility/culling benchmark yet.
            var depth = 10.0 + ((i * 7919L) % 300_000);
            var horizontal = ((i * 104729L) % 20_000) / 100.0 - 100.0;

            _entities[i] = new SyntheticEntity(horizontal, depth);
        }
    }

    [Benchmark(Baseline = true)]
    public double NaiveFullRate()
    {
        var result = 0.0;

        foreach (var entity in _entities)
        {
            result += ParallaxModel.ProjectHorizontal(entity.Horizontal, entity.Depth, 960.0);
            result += ParallaxModel.ViewAngle(entity.Horizontal, entity.Depth);
        }

        _sink = result;
        return result;
    }

    [Benchmark]
    public double ObserverRelativeCadence()
    {
        var result = 0.0;

        foreach (var entity in _entities)
        {
            // The benchmark models temporal responsibility only. It does not
            // pretend that distance selection is free.
            var horizon = ObserverDistance.Select(entity.Depth, Horizons);

            switch (horizon.Name)
            {
                case "Near":
                    result += ParallaxModel.ProjectHorizontal(entity.Horizontal, entity.Depth, 960.0);
                    result += ParallaxModel.ViewAngle(entity.Horizontal, entity.Depth);
                    break;

                case "Distant":
                    result += ParallaxModel.ProjectHorizontal(entity.Horizontal, entity.Depth, 960.0);
                    break;

                case "Landscape":
                    // The mountain remains represented, but its full positional
                    // evaluation is not performed every scheduler tick.
                    result += entity.Depth * 0.000001;
                    break;

                default:
                    result += entity.Depth * 0.0000001;
                    break;
            }
        }

        _sink = result;
        return result;
    }

    private static readonly EventHorizon[] Horizons =
    [
        new("Near", 10, 120),
        new("Distant", 1_000, 20),
        new("Landscape", 300_000, 1),
        new("Semantic", 1_000_000, 0.25)
    ];

    private readonly record struct SyntheticEntity(double Horizontal, double Depth);
}
