using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        var artifactsPath = Path.Combine(
            Path.GetTempPath(),
            "TheSingularityWorkshop.Renderer.Benchmarks");

        var config = DefaultConfig.Instance
            .WithArtifactsPath(artifactsPath);

        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, config);
    }
}
