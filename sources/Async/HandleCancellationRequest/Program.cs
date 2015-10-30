namespace HandleCancellationRequest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static void Main()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(500);
                var token = cts.Token;

                try
                {
                    Task.Run(() =>
                    {
                        for (var i = 0; i < 10; i++)
                        {
                            Task.Delay(200).Wait();
                            token.ThrowIfCancellationRequested();
                        }
                    }, token)
                    .Wait();
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Calculations cancelled");
                }
            }

            Console.ReadKey();
        }
    }
}