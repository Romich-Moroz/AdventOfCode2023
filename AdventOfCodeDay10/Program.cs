namespace AdventOfCodeDay10
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var maze = Maze.Parse(File.ReadAllLines("../../../input.txt"));
            maze.GeneratePath();

            Console.WriteLine((maze.PipeLines.Max(p => p.Max(p => p.Length)) + 1) / 2);
            Console.WriteLine(maze.GetInnerRegionsCount());
        }
    }
}
