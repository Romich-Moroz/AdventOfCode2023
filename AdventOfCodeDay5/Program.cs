using System.Diagnostics;

namespace AdventOfCodeDay5
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var almanac = Almanac.Parse(File.ReadAllText("../../../input.txt"));

            Console.WriteLine(Task1(almanac));

            Console.WriteLine(Task2(almanac));
        }

        public static uint Task1(Almanac almanac) => almanac.GetMappedValues(almanac.Seeds).Min();

        public static uint Task2(Almanac almanac) => almanac.GetMappedValues(almanac.SeedsRange).Min(s => s.Start);
    }
}
