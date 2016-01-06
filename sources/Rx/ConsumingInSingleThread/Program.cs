using System;
using System.Threading.Tasks;

namespace ConsumingInSingleThread
{
    using System.Globalization;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;

    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("Main ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var queue = new Subject<string>();

            // Consuming items in single thread, for long opererations ThreadPoolScheduler creates single background thread
            queue
                .ObserveOn(ThreadPoolScheduler.Instance)
                .Subscribe(x =>
                {
                    Thread.Sleep(100);
                    Console.WriteLine("[{0}] IsThreadPool: {1}, Consuming: {2}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread, x);
                });

            var task1 = Task.Run(() =>
            {
                Console.WriteLine("Producer ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                for (var i = 0; i < 100; i++)
                    queue.OnNext((2 * i).ToString(CultureInfo.InvariantCulture));
            });
            var task2 = Task.Run(() =>
            {
                Console.WriteLine("Producer ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                for (var i = 0; i < 100; i++)
                    queue.OnNext((2 * i + 1).ToString(CultureInfo.InvariantCulture));
            });
            Task.WaitAll(task1, task2);

            Console.ReadKey();
        }
    }
}
