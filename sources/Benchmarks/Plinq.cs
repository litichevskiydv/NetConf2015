namespace Benchmarks
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using BenchmarkDotNet;
    using JetBrains.Annotations;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Plinq
    {
        private readonly int[] _tasks = Enumerable.Range(1, 100).ToArray();
        private readonly int[] _shortTasks = Enumerable.Range(1, 10000000).ToArray();

        [Benchmark]
        public int[] ProcessingWithDefaultSettings()
        {
            return _tasks.AsParallel()
                .Select(x =>
                        {
                            Thread.Sleep(x * 10);
                            return x;
                        })
                .ToArray();
        }

        [Benchmark]
        public int[] ProcessingWitDegreeOfParallelism()
        {
            return _tasks.AsParallel()
                .WithDegreeOfParallelism((int)(Environment.ProcessorCount * 1.5d))
                .Select(x =>
                        {
                            Thread.Sleep(x * 10);
                            return x;
                        })
                .ToArray();
        }

        [Benchmark]
        public int[] ProcessingWitDynamicPartitioner()
        {
            return Partitioner.Create(_tasks, true).AsParallel()
                .Select(x =>
                        {
                            Thread.Sleep(x * 10);
                            return x;
                        })
                .ToArray();
        }

        [Benchmark]
        public int[] ProcessingWitDynamicPartitionerAndDegreeOfParallelism()
        {
            return Partitioner.Create(_tasks, true).AsParallel()
                .WithDegreeOfParallelism((int)(Environment.ProcessorCount * 1.5d))
                .Select(x =>
                {
                    Thread.Sleep(x * 10);
                    return x;
                })
                .ToArray();
        }

        [Benchmark]
        public List<int> IterationWithDefaultSettings()
        {
            var squares = new List<int>(_shortTasks.Length);

            var query = _shortTasks.AsParallel()
                .WithDegreeOfParallelism((int) (Environment.ProcessorCount*1.5d))
                .Select(x => x);

            foreach (var i in query)
                squares.Add(2*i);
            return squares;
        }

        [Benchmark]
        public List<int> IterationWithNotBuffered()
        {
            var squares = new List<int>(_shortTasks.Length);

            var query = _shortTasks.AsParallel()
                .WithDegreeOfParallelism((int) (Environment.ProcessorCount*1.5d))
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .Select(x => x);

            foreach (var i in query)
                squares.Add(2 * i);
            return squares;
        }

        [Benchmark]
        public List<int> IterationWithFullyBuffered()
        {
            var squares = new List<int>(_shortTasks.Length);

            var query = _shortTasks.AsParallel()
                .WithDegreeOfParallelism((int) (Environment.ProcessorCount*1.5d))
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                .Select(x => x);

            foreach (var i in query)
                squares.Add(2 * i);
            return squares;
        }
    }
}