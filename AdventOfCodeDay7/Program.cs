namespace AdventOfCodeDay7
{
    public enum Card
    {
        Undefined,
        Joker,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum CardCombination
    {
        Undefined,
        HighCard,
        OnePair,
        TwoPair,
        ThreeCards,
        FullHouse,
        FourCards,
        FiveCards
    }

    public class Hand(List<Card> cards, int bet, bool jokerMode) : IComparable<Hand>
    {
        public static readonly Dictionary<char, Card> CardsDictionary = new()
        {
            { '!', Card.Joker },
            { '2', Card.Two },
            { '3', Card.Three },
            { '4', Card.Four },
            { '5', Card.Five },
            { '6', Card.Six },
            { '7', Card.Seven },
            { '8', Card.Eight },
            { '9', Card.Nine },
            { 'T', Card.Ten },
            { 'J', Card.Jack },
            { 'Q', Card.Queen },
            { 'K', Card.King },
            { 'A', Card.Ace },
        };

        public List<Card> Cards { get; } = cards;
        public int Bet { get; } = bet;
        public bool JokerMode { get; } = jokerMode;

        public CardCombination CardCombination { get; } = GetCardCombination(cards, jokerMode);

        public static Hand Parse(string input, bool jokerMode)
        {
            var split = input.Split(' ');
            split[0] = jokerMode ? split[0].Replace('J', '!') : split[0];
            return new Hand(split[0].Select(c => CardsDictionary[c]).ToList(), int.Parse(split[1]), jokerMode);
        }

        private static CardCombination GetCardCombination(List<Card> cards, bool jokerMode)
        {
            var countedCards = cards.GroupBy(c => c).Select(g => new Tuple<int, Card>(g.Count(), g.Key)).ToList();
            var totalDifferentCards = countedCards.Count;
            var maxSingleCardCount = countedCards.Where(cc => cc.Item2 != Card.Joker).DefaultIfEmpty(new Tuple<int, Card>(5, Card.Joker)).Max(cc => cc.Item1);
            var hasCardsBesidesJokers = countedCards.Any(cc => cc.Item2 != Card.Joker);
            if (jokerMode && hasCardsBesidesJokers)
            {
                var jokers = cards.Count(c => c == Card.Joker);
                if (jokers != 0)
                {
                    Console.Write($"Hand {GetCardCombination(totalDifferentCards, maxSingleCardCount)}: ");
                    cards.ForEach(c => Console.Write(CardsDictionary.Single(p => p.Value == c).Key));
                    totalDifferentCards--;
                    maxSingleCardCount += jokers;
                    Console.WriteLine($" changed to: {GetCardCombination(totalDifferentCards, maxSingleCardCount)}");
                }
            }

            return GetCardCombination(totalDifferentCards, maxSingleCardCount);
        }

        private static CardCombination GetCardCombination(int totalDifferentCards, int maxSingleCardCount) => totalDifferentCards == 1
                ? CardCombination.FiveCards
                : totalDifferentCards == 2
                ? maxSingleCardCount == 4 ? CardCombination.FourCards : CardCombination.FullHouse
                : totalDifferentCards == 3
                ? maxSingleCardCount == 3 ? CardCombination.ThreeCards : CardCombination.TwoPair
                : totalDifferentCards == 4 ? CardCombination.OnePair : CardCombination.HighCard;

        public int CompareTo(Hand? other)
        {
            if (other is null)
            {
                return 1;
            }
            else
            {
                if (CardCombination != other.CardCombination)
                {
                    return CardCombination - other.CardCombination;
                }

                for (var i = 0; i < Cards.Count; i++)
                {
                    if (Cards[i] != other.Cards[i])
                    {
                        return Cards[i] - other.Cards[i];
                    }
                }

                return 0;
            }
        }
    }

    public class Game(List<Hand> hands)
    {
        public List<Hand> Hands { get; } = hands;

        public static Game Parse(string[] input, bool jokerMode) => new(input.Select(line => Hand.Parse(line, jokerMode)).ToList());

        public int GetTotalWinnings() => Hands.OrderBy(h => h).Select((o, i) => (i + 1) * o.Bet).Sum();
    }

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
