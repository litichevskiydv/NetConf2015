namespace Benchmarks
{
    using BenchmarkDotNet;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var competitionSwitch = new BenchmarkCompetitionSwitch(new[]
                                                                   {
                                                                       typeof (Plinq)
                                                                   });
            competitionSwitch.Run(args);
        }
    }
}