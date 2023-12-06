using System.Diagnostics;

namespace AdventOfCodeDay5
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var almanac = Almanac.Parse(File.ReadAllText("../../../input.txt"));

            Console.WriteLine(Task1(almanac));
            var task = Task2(almanac);
            Debug.Assert(task == 1240035);
            Console.WriteLine(task);
        }

        public static uint Task1(Almanac almanac) => almanac.GetMappedValues(almanac.Seeds).Min();

        public static uint Task2(Almanac almanac) => almanac.GetMappedValues(almanac.SeedsRange).Min(s => s.Start);
    }
}
