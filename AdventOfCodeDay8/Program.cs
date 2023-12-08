namespace AdventOfCodeDay8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var net = NodeNetwork.Parse(File.ReadAllText("../../../input.txt").Replace("(", null).Replace(")", null).Split("\r\n", StringSplitOptions.TrimEntries));

            Console.WriteLine(net.FindPathLength());
            Console.WriteLine(net.FindGhostPathLegnth());
        }
    }
}
