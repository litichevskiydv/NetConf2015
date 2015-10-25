namespace Benchmarks
{
    using BenchmarkDotNet;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var competitionSwitch = new BenchmarkCompetitionSwitch(new[]
                                                                   {
                                                                       typeof (Plinq),
                                                                       typeof (Tpl)
                                                                   });
            competitionSwitch.Run(args);
        }
    }
}