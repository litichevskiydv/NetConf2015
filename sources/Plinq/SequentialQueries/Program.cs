namespace SequentialQueries
{
    using System;
    using System.Linq;
    using System.Threading;

    internal static class Program
    {
        private static int GetNumber(int position, int element)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            var result = element;
            if (position%2 != 0)
                result *= 2;
            if (position % 3 != 0)
                result *= 3;
            return result;
        }

        private static bool SelectNumber(int position, int element)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            return position%3 == 0;
        }

        private static void Main()
        {
            var rnd = new Random();

            var firstExample = Enumerable.Repeat(0, 50)
                .Select(x => rnd.Next(10000))
                .AsParallel()
                .Where(x => x % 5 != 0)
                .Select((x, i) => GetNumber(i, x))
                .ToArray();
            Console.WriteLine(firstExample.First());

            var secondExample = Enumerable.Repeat(0, 50)
                .Select(x => rnd.Next(10000))
                .AsParallel()
                .Where(x => x % 5 != 0)
                .Where((x, i) => SelectNumber(i, x))
                .ToArray();
            Console.WriteLine(secondExample.First());

            var thirdExample = Enumerable.Repeat(0, 50)
                .Select(x => rnd.Next(10000))
                .AsParallel()
                .Skip(1)
                .SkipWhile((x, i) => SelectNumber(i, x))
                .ToArray();
            Console.WriteLine(thirdExample.First());

            var fourthExample = Enumerable.Repeat(0, 50)
                .Select(x => rnd.Next(10000))
                .AsParallel()
                .Skip(1)
                .TakeWhile((x, i) => SelectNumber(i, x))
                .ToArray();
            Console.WriteLine(fourthExample.First());

            Console.ReadKey();
        }
    }
}