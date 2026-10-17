using BenchmarkDotNet.Attributes;
using TheSingularityWorkshop.Renderer;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

/// <summary>
/// Isolates temporal responsibility from a full-world scan. The observer-relative
/// path updates only due cohorts and reads a fixed-size observer sample afterward.
/// This is a synthetic scheduling experiment, not a complete rendering benchmark.
/// </summary>
[MemoryDiagnoser]
public class TemporalResponsibilityBenchmarks
{
    private const int ObservableSampleSize = 32;
    private const int MeasurementWindowTicks = 63;

    private SyntheticEntity[] _entities = [];
    private int[][] _cohorts = [];
    private double[] _fullRateValues = [];
    private double[] _observerValues = [];
    private double[] _fidelityValues = [];
    private double _sink;
    private int _fullRateTick;
    private int _observerTick;
    private int _fidelityTick;

    [Params(1_000, 10_000, 100_000, 1_000_000)]
    public int Population { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _entities = new SyntheticEntity[Population];
        _fullRateValues = new double[Population];
        _observerValues = new double[Population];
        _fidelityValues = new double[Population];

        var cohortBuilders = new Dictionary<int, List<int>>
        {
            [1] = [],
            [4] = [],
            [16] = [],
            [64] = []
        };

        for (var i = 0; i < Population; i++)
        {
            var depth = 10.0 + ((i * 7919L) % 300_000);
            var horizontal = ((i * 104729L) % 20_000) / 100.0 - 100.0;
            var velocity = ((i * 3571L) % 101) / 10_000.0 - 0.005;
            var cadence = depth < 120 ? 1 : depth < 1_000 ? 4 : depth < 300_000 ? 16 : 64;

            _entities[i] = new SyntheticEntity(horizontal, velocity, depth, cadence);
            cohortBuilders[cadence].Add(i);

            var initial = Evaluate(_entities[i], tick: 0);
            _fullRateValues[i] = initial;
            _observerValues[i] = initial;
            _fidelityValues[i] = initial;
        }

        _cohorts = cohortBuilders.Values.Select(indices => indices.ToArray()).ToArray();
        _fullRateTick = 0;
        _observerTick = 0;
        _fidelityTick = 0;
        _sink = 0;
    }

    /// <summary>
    /// Full-rate baseline: evaluate every entity at every simulation tick.
    /// </summary>
    [Benchmark(Baseline = true)]
    public double FullRateWindow()
    {
        var checksum = 0.0;

        for (var windowTick = 0; windowTick < MeasurementWindowTicks; windowTick++)
        {
            var tick = ++_fullRateTick;

            for (var i = 0; i < _entities.Length; i++)
            {
                _fullRateValues[i] = Evaluate(_entities[i], tick);
            }
        }

        for (var sample = 0; sample < ObservableSampleSize; sample++)
        {
            checksum += _fullRateValues[SampleIndex(sample)];
        }

        _sink = checksum;
        return checksum;
    }

    /// <summary>
    /// Observer-relative path: advance only cohorts due at each tick, then read
    /// the same fixed-size sample used by the full-rate baseline.
    /// </summary>
    [Benchmark]
    public double ObserverRelativeWindow()
    {
        for (var windowTick = 0; windowTick < MeasurementWindowTicks; windowTick++)
        {
            var tick = ++_observerTick;
            UpdateDueCohorts(_observerValues, tick);
        }

        var checksum = 0.0;
        for (var sample = 0; sample < ObservableSampleSize; sample++)
        {
            checksum += _observerValues[SampleIndex(sample)];
        }

        _sink = checksum;
        return checksum;
    }

    /// <summary>
    /// Measures projection-space staleness over the same 63-tick window.
    /// The full-rate reference evaluates only the fixed observer sample every tick;
    /// the compared values are the last values written by due observer cohorts.
    /// The returned mean absolute error is a diagnostic metric, not a timing claim.
    /// </summary>
    [Benchmark]
    public double ObserverRelativeMeanAbsoluteErrorWindow()
    {
        var totalAbsoluteError = 0.0;
        var comparisons = 0;

        for (var windowTick = 0; windowTick < MeasurementWindowTicks; windowTick++)
        {
            var tick = ++_fidelityTick;
            UpdateDueCohorts(_fidelityValues, tick);

            for (var sample = 0; sample < ObservableSampleSize; sample++)
            {
                var index = SampleIndex(sample);
                var fullRateReference = Evaluate(_entities[index], tick);
                totalAbsoluteError += Math.Abs(fullRateReference - _fidelityValues[index]);
                comparisons++;
            }
        }

        var meanAbsoluteError = totalAbsoluteError / comparisons;
        _sink = meanAbsoluteError;
        return meanAbsoluteError;
    }

    private void UpdateDueCohorts(double[] values, int tick)
    {
        foreach (var cohort in _cohorts)
        {
            if (cohort.Length == 0)
            {
                continue;
            }

            var cadence = _entities[cohort[0]].Cadence;
            if (tick % cadence != 0)
            {
                continue;
            }

            foreach (var index in cohort)
            {
                values[index] = Evaluate(_entities[index], tick);
            }
        }
    }

    private int SampleIndex(int sample) =>
        (int)((long)sample * (_entities.Length - 1) / (ObservableSampleSize - 1));

    private static double Evaluate(SyntheticEntity entity, int tick)
    {
        var horizontal = entity.Horizontal + entity.Velocity * tick;
        return ParallaxModel.ProjectHorizontal(horizontal, entity.Depth, 960.0)
            + ParallaxModel.ViewAngle(horizontal, entity.Depth);
    }

    private readonly record struct SyntheticEntity(
        double Horizontal,
        double Velocity,
        double Depth,
        int Cadence);
}
