using BenchmarkDotNet.Attributes;
using TheSingularityWorkshop.Renderer;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

[MemoryDiagnoser]
public class RendererMathBenchmarks
{
    private readonly EventHorizon[] _horizons =
    [
        new("Near", 10, 120),
        new("Distant", 1_000, 20),
        new("Landscape", 300_000, 1),
        new("Semantic", 1_000_000, 0.25)
    ];

    [Params(10, 1_000, 300_000)]
    public double Depth { get; set; }

    [Benchmark(Baseline = true)]
    public double ProjectHorizontal()
        => ParallaxModel.ProjectHorizontal(123.0, Depth, 960.0);

    [Benchmark]
    public double LateralParallax()
        => ParallaxModel.LateralImageDisplacement(10.0, Depth, 960.0);

    [Benchmark]
    public double ViewAngle()
        => ParallaxModel.ViewAngle(123.0, Depth);

    [Benchmark]
    public EventHorizon SelectEventHorizon()
        => ObserverDistance.Select(Depth, _horizons);
}
