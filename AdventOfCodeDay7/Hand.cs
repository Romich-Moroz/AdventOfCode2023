namespace AdventOfCodeDay7
{
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
        public CardCombination CardCombination { get; } = GetCardCombination(cards, jokerMode);

        public static Hand Parse(string input, bool jokerMode)
        {
            var split = (jokerMode ? input.Replace('J', '!') : input).Split(' ');
            return new Hand(split[0].Select(c => CardsDictionary[c]).ToList(), int.Parse(split[1]), jokerMode);
        }

        public int CompareTo(Hand? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (CardCombination != obj.CardCombination)
            {
                return CardCombination - obj.CardCombination;
            }

            var diffId = Enumerable.Range(0, Cards.Count).FirstOrDefault(i => Cards[i] != obj.Cards[i]);
            return Cards[diffId] - obj.Cards[diffId];
        }

        private static CardCombination GetCardCombination(List<Card> cards, bool jokerMode)
        {
            var groupedCards = cards.GroupBy(c => c).Select(g => new Tuple<int, Card>(g.Count(), g.Key)).ToList();

            var totalDifferentCards = groupedCards.Count;
            var anyCardsBesidesJokers = groupedCards.Any(cc => cc.Item2 != Card.Joker);
            var maxCardCountWithoutJoker = groupedCards.Where(cc => cc.Item2 != Card.Joker).DefaultIfEmpty(new(5, Card.Joker)).Max(cc => cc.Item1);

            if (jokerMode && anyCardsBesidesJokers)
            {
                var jokers = cards.Count(c => c == Card.Joker);
                if (jokers != 0)
                {
                    totalDifferentCards--;
                    maxCardCountWithoutJoker += jokers;
                }
            }

            return GetCardCombination(totalDifferentCards, maxCardCountWithoutJoker);
        }

        private static CardCombination GetCardCombination(int totalDifferentCards, int maxSingleCardCount) =>
            totalDifferentCards == 1 ? CardCombination.FiveCards : totalDifferentCards == 2
                ? maxSingleCardCount == 4 ? CardCombination.FourCards : CardCombination.FullHouse
                : totalDifferentCards == 3
                    ? maxSingleCardCount == 3 ? CardCombination.ThreeCards : CardCombination.TwoPair
                    : totalDifferentCards == 4 ? CardCombination.OnePair : CardCombination.HighCard;
    }
}