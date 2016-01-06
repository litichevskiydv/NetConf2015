namespace ParallelConsuming
{
    using System;
    using System.Globalization;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("Main ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

            var queue = new Subject<string>();
            queue
                .Subscribe(x =>
                           {
                               Console.WriteLine("[{0}] Consuming: {1}", Thread.CurrentThread.ManagedThreadId, x);
                           });

            // Consuming items in generation threads
            var task1 = Task.Run(() =>
                                 {
                                     Console.WriteLine("Producer ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                                     for (var i = 0; i < 100; i++)
                                         queue.OnNext((2*i).ToString(CultureInfo.InvariantCulture));
                                 });
            var task2 = Task.Run(() =>
                                 {
                                     Console.WriteLine("Producer ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
                                     for (var i = 0; i < 100; i++)
                                         queue.OnNext((2*i + 1).ToString(CultureInfo.InvariantCulture));
                                 });
            Task.WaitAll(task1, task2);

            Console.ReadKey();
        }
    }
}
