using System.Text.RegularExpressions;

namespace AdventOfCodeDay4
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
}
