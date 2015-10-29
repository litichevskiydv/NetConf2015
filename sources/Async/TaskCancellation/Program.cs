namespace TaskCancellation
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    static class Program
    {
        private static async Task DownloadPageAsync(string uri, CancellationToken token)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(uri, token))
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content.Length);
            }
        }

        static void Main()
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromMilliseconds(100));
                try
                {
                    var task = DownloadPageAsync("http://www.microsoft.com/", cts.Token);
                    task.Wait(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Wait cancelled");
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Downloading cancelled");
                }
            }

            Console.ReadKey();
        }
    }
}
