namespace AdventOfCodeDay6
{
    public class RaceInfo(long duration, long distanceRecord)
    {
        public long GetTotalWaysToWin() => Enumerable.Range(0, (int)duration).Where(i => ((duration - i) * (1 * i)) > distanceRecord).Count();
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<RaceInfo> task1 = [new(62, 644), new(73, 1023), new(75, 1240), new(65, 1023)];
            List<RaceInfo> task2 = [new(62737565, 644102312401023)];

            Console.WriteLine(task1.Select(r => r.GetTotalWaysToWin()).Aggregate((x, y) => x * y));
            Console.WriteLine(task2.Select(r => r.GetTotalWaysToWin()).Aggregate((x, y) => x * y));
        }
    }
}