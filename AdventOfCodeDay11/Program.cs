namespace AdventOfCodeDay11
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var galaxy = GalacticSpace.Parse(File.ReadAllLines("../../../input.txt"));

            galaxy.Expand(2);
            Console.WriteLine(galaxy.GetShortestPath());

            galaxy.Expand(1000000);
            Console.WriteLine(galaxy.GetShortestPath());
        }
    }
}