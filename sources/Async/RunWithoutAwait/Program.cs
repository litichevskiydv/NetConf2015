namespace RunWithoutAwait
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
            var task = DownloadPageAsync("http://www.microsoft.com/");
            task.ContinueWith(t => t.Exception.Handle(e =>
                                                      {
                                                          Console.WriteLine(e.Message);
                                                          return true;
                                                      }),
                TaskContinuationOptions.OnlyOnFaulted);
            Console.WriteLine("Something happening");

            Console.ReadKey();
        }
    }
}