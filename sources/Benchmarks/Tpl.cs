namespace Benchmarks
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BenchmarkDotNet;
    using JetBrains.Annotations;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Tpl
    {
        private readonly int[] _tasks = Enumerable.Range(1, 100).ToArray();

        [Benchmark]
        public int ProcessingWithDefaultSettings()
        {
            var sum = 0;

            Parallel.ForEach(_tasks, () => 0,
                (x, loopState, localSum) =>
                {
                    Thread.Sleep(x*10);
                    localSum += x;
                    return localSum;
                }, localSum => Interlocked.Add(ref sum, localSum));

            return sum;
        }

        [Benchmark]
        public int ProcessingWithDegreeOfParallelism()
        {
            var sum = 0;

            Parallel.ForEach(_tasks,
                new ParallelOptions {MaxDegreeOfParallelism = (int) (Environment.ProcessorCount*1.5d)},
                () => 0, (x, loopState, localSum) =>
                         {
                             Thread.Sleep(x*10);
                             localSum += x;
                             return localSum;
                         }, localSum => Interlocked.Add(ref sum, localSum));

            return sum;
        }

        [Benchmark]
        public int ProcessingWithDynamicPartitioner()
        {
            var sum = 0;

            Parallel.ForEach(Partitioner.Create(_tasks, true),
                () => 0, (x, loopState, localSum) =>
                {
                    Thread.Sleep(x * 10);
                    localSum += x;
                    return localSum;
                }, localSum => Interlocked.Add(ref sum, localSum));

            return sum;
        }

        [Benchmark]
        public int ProcessingWithDynamicPartitionerAndDegreeOfParallelism()
        {
            var sum = 0;

            Parallel.ForEach(Partitioner.Create(_tasks, true),
                new ParallelOptions {MaxDegreeOfParallelism = (int) (Environment.ProcessorCount*1.5d)},
                () => 0, (x, loopState, localSum) =>
                         {
                             Thread.Sleep(x*10);
                             localSum += x;
                             return localSum;
                         }, localSum => Interlocked.Add(ref sum, localSum));

            return sum;
        }
    }
}