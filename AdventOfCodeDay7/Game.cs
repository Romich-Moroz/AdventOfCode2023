namespace AdventOfCodeDay7
{
    public class Game(List<Hand> hands)
    {
        public List<Hand> Hands { get; } = hands;

        public static Game Parse(string[] input, bool jokerMode) => new(input.Select(line => Hand.Parse(line, jokerMode)).ToList());
        public int GetTotalWinnings() => Hands.OrderBy(h => h).Select((o, i) => (i + 1) * o.Bet).Sum();
    }
}
