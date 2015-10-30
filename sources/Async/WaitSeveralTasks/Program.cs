namespace WaitSeveralTasks
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static async Task DownloadPageAsync(string uri)
        {
            using (var webclient = new WebClient())
            {
                var content = await webclient.DownloadStringTaskAsync(uri);
                Console.WriteLine(content.Length);
                throw new InvalidOperationException("Something go wrong");
            }
        }

        private static void Main()
        {
            try
            {
                var complexTask = Task.WhenAll(DownloadPageAsync("http://dotnetconf.ru"), DownloadPageAsync("http://microsoft.com"));
                complexTask.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var inner in ex.InnerExceptions)
                    Console.WriteLine(inner.Message);
            }
        }
    }
}