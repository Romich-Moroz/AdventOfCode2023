namespace AdventOfCodeDay4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");
            var gameRounds = lines.Select(GameCard.Parse).ToList();

            Console.WriteLine(Task1(gameRounds));
            Console.WriteLine(Task2(gameRounds));
        }

        private static int Task1(List<GameCard> gameRounds) =>
            gameRounds.Select(gr => gr.GetWinningCardsCount()).
                       Select(n => n == 0 ? 0 : n == 1 ? 1 : (1 << (n - 1))).
                       Sum();

        private static int Task2(List<GameCard> gameRounds)
        {
            var result = 0;

            for (var i = 0; i < gameRounds.Count; result += gameRounds[i].TimesToProcess, i++)
            {
                var winningCardsCount = gameRounds[i].GetWinningCardsCount();
                for (var j = 1; j <= winningCardsCount && i + j < gameRounds.Count; gameRounds[i + j].TimesToProcess += gameRounds[i].TimesToProcess, j++) ;
            }

            return result;
        }
    }
}
