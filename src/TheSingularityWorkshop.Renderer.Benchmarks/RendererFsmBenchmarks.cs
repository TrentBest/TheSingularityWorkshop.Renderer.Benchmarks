using BenchmarkDotNet.Attributes;
using TheSingularityWorkshop.Renderer;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

/// <summary>
/// Measures the actual Renderer FSM integration using the published FSM_API package.
/// This isolates scheduling overhead from graphics work.
/// </summary>
[MemoryDiagnoser]
public sealed class RendererFsmBenchmarks
{
    private RendererComputationMachine[] _machines = [];

    [Params(1, 10, 50, 100)]
    public int Population { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _machines = new RendererComputationMachine[Population];

        for (var i = 0; i < Population; i++)
        {
            var context = new RendererComputationContext($"BenchmarkEntity-{i}");
            _machines[i] = new RendererComputationMachine(context);
        }
    }

    [Benchmark]
    public void UpdateRendererGroup()
    {
        _machines[0].Advance();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        foreach (var machine in _machines)
        {
            machine.Dispose();
        }
    }
}
