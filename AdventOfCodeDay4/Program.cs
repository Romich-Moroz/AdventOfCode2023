using System.Text.RegularExpressions;

namespace AdventOfCodeDay4
{
    internal class Program
    {

        public class GameCard(int gameCardId, List<int> winningScratchCards, List<int> playerScratchCards)
        {
            public int GameCardId { get; set; } = gameCardId;
            public List<int> WinningScratchCards { get; set; } = winningScratchCards;
            public List<int> PlayerScratchCards { get; set; } = playerScratchCards;
            public int TimesToProcess { get; set; } = 1;

            public static GameCard Parse(string input)
            {
                var gameCardSplit = new Regex("[ ]{2,}", RegexOptions.None).Replace(input, " ").Split(':', StringSplitOptions.TrimEntries);
                var scratchCardSidesSplit = gameCardSplit[1].Split('|', StringSplitOptions.TrimEntries);

                return new(
                    int.Parse(gameCardSplit[0].Split(' ')[1]),
                    scratchCardSidesSplit[0].Split(' ').Select(int.Parse).ToList(),
                    scratchCardSidesSplit[1].Split(' ').Select(int.Parse).ToList()
               );
            }

            public int GetWinningCardsCount() => PlayerScratchCards.Where(WinningScratchCards.Contains).Count();
        }

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
