using BenchmarkDotNet.Attributes;

namespace TheSingularityWorkshop.Renderer.Benchmarks;

[MemoryDiagnoser]
public class AutonomousWorldBenchmarks
{
    private const int HighFrequencyPopulation = 1_000;
    private const int ObservablePopulation = 10_000;
    private const int EventPopulation = 128;
    private ulong _sink;

    [Params(1_000_000, 10_000_000, 100_000_000)]
    public int LogicalPopulation { get; set; }

    [Benchmark(Baseline = true)]
    public ulong FullRateAgentEvaluation()
    {
        var result = 0UL;
        for (var id = 0; id < LogicalPopulation; id++)
        {
            var state = SyntheticAgentState.FromId((uint)id);
            result ^= EvaluateFullAgent(state);
        }
        _sink = result;
        return result;
    }

    [Benchmark]
    public ulong ObserverRelativeAgentEvaluation()
    {
        var result = 0UL;
        var highFrequency = 0;
        var observable = 0;
        var events = 0;

        for (var id = 0; id < LogicalPopulation; id++)
        {
            var state = SyntheticAgentState.FromId((uint)id);

            if (state.ObserverDistanceBand == DistanceBand.Near)
            {
                if (highFrequency < HighFrequencyPopulation)
                {
                    result ^= EvaluateHighFrequencyAgent(state);
                    highFrequency++;
                }
            }
            else if (state.ObserverDistanceBand == DistanceBand.Observable)
            {
                if (observable < ObservablePopulation)
                {
                    result ^= EvaluateObservableAgent(state);
                    observable++;
                }
            }

            if (state.EventBucket < EventPopulation)
            {
                result ^= EvaluateEventDrivenState(state);
                events++;
            }
        }

        result ^= (ulong)LogicalPopulation;
        result ^= (ulong)highFrequency << 32;
        result ^= (ulong)observable << 16;
        result ^= (ulong)events;

        _sink = result;
        return result;
    }

    private static ulong EvaluateFullAgent(SyntheticAgentState state)
    {
        var result = state.Identity;
        result ^= (ulong)state.StandingOrder * 0x9E3779B9UL;
        result ^= (ulong)state.GeneralOrder * 0x85EBCA6BUL;
        result ^= (ulong)state.SafetyState * 0xC2B2AE35UL;
        result ^= (ulong)state.FsmState * 0x27D4EB2FUL;
        result ^= (ulong)state.MovementState * 0x165667B1UL;
        return Mix(result);
    }

    private static ulong EvaluateHighFrequencyAgent(SyntheticAgentState state)
    {
        var result = state.Identity;
        result ^= (ulong)state.FsmState;
        result ^= (ulong)state.MovementState << 16;
        result ^= (ulong)state.SafetyState << 32;
        return Mix(result);
    }

    private static ulong EvaluateObservableAgent(SyntheticAgentState state)
        => Mix(state.Identity ^ ((ulong)state.GeneralOrder << 24));

    private static ulong EvaluateEventDrivenState(SyntheticAgentState state)
        => Mix(state.Identity ^ ((ulong)state.SafetyState << 40));

    private static ulong Mix(ulong value)
    {
        value ^= value >> 30;
        value *= 0xBF58476D1CE4E5B9UL;
        value ^= value >> 27;
        value *= 0x94D049BB133111EBUL;
        return value ^ (value >> 31);
    }

    private readonly record struct SyntheticAgentState(
        ulong Identity,
        ushort StandingOrder,
        ushort GeneralOrder,
        byte SafetyState,
        byte FsmState,
        byte MovementState,
        DistanceBand ObserverDistanceBand,
        ushort EventBucket)
    {
        public static SyntheticAgentState FromId(uint id)
        {
            var identity = Mix(id + 0x9E3779B9U);
            return new SyntheticAgentState(
                identity,
                (ushort)(identity % 64),
                (ushort)((identity >> 8) % 128),
                (byte)((identity >> 16) % 16),
                (byte)((identity >> 24) % 32),
                (byte)((identity >> 32) % 16),
                (DistanceBand)((identity >> 40) % 3),
                (ushort)(identity % 100_000));
        }
    }

    private enum DistanceBand : byte
    {
        Near,
        Observable,
        Distant
    }
}
