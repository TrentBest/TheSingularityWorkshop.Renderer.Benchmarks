using BenchmarkDotNet.Attributes;
using TheSingularityWorkshop.Renderer;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

/// <summary>
/// Measures Event Horizon selection as horizon count increases.
/// The allocation is intentionally measured rather than hidden.
/// </summary>
[MemoryDiagnoser]
public class EventHorizonSelectionBenchmarks
{
    private EventHorizon[] _horizons = [];

    [Params(4, 16, 64, 256)]
    public int HorizonCount { get; set; }

    [Params(10, 300_000)]
    public double Distance { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _horizons = new EventHorizon[HorizonCount];

        for (var i = 0; i < HorizonCount; i++)
        {
            _horizons[i] = new EventHorizon(
                $"Horizon-{i}",
                (i + 1) * 1_000.0,
                Math.Max(0.25, 120.0 / (i + 1)));
        }
    }

    [Benchmark]
    public EventHorizon Select()
        => ObserverDistance.Select(Distance, _horizons);
}
