namespace AdventOfCodeDay7
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game1 = Game.Parse(File.ReadAllLines("../../../input.txt"), false);
            var game2 = Game.Parse(File.ReadAllLines("../../../input.txt"), true);

            Console.WriteLine(game1.GetTotalWinnings());
            Console.WriteLine(game2.GetTotalWinnings());
        }
    }
}
