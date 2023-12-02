namespace AdventOfCodeDay2
{
    public class Program
    {
        public class GameSet
        {
            private static readonly string[] Keywords = ["red", "blue", "green"];
            public CubesSet CubesSet { get; set; } = new CubesSet();

            public static GameSet Parse(string input)
            {
                var result = new GameSet();

                foreach (var line in input.Split(',', StringSplitOptions.TrimEntries))
                {
                    if (line.Contains(Keywords[0]))
                    {
                        result.CubesSet.RedCubesCount = int.Parse(line.Split(' ')[0]);
                    }
                    else if (line.Contains(Keywords[1]))
                    {
                        result.CubesSet.BlueCubesCount = int.Parse(line.Split(' ')[0]);
                    }
                    else if (line.Contains(Keywords[2]))
                    {
                        result.CubesSet.GreenCubesCount = int.Parse(line.Split(' ')[0]);
                    }
                }

                return result;
            }

            public bool IsPossibleSet(CubesSet gameRoundCondition) =>
                CubesSet.GreenCubesCount <= gameRoundCondition.GreenCubesCount
                && CubesSet.RedCubesCount <= gameRoundCondition.RedCubesCount
                && CubesSet.BlueCubesCount <= gameRoundCondition.BlueCubesCount;
        }

        public class GameRound
        {
            public int Id { get; set; }
            public List<GameSet> GameSets { get; set; } = [];

            public static GameRound Parse(string input)
            {
                var split = input.Split(':');
                return new GameRound
                {
                    Id = int.Parse(split[0].Split(' ')[1]),
                    GameSets = split[1].Split(';').Select(gameSetLine => GameSet.Parse(gameSetLine.Trim())).ToList()
                };
            }

            public CubesSet FindSuccessfullRoundCondition() => new()
            {
                RedCubesCount = GameSets.Max(gs => gs.CubesSet.RedCubesCount),
                GreenCubesCount = GameSets.Max(gs => gs.CubesSet.GreenCubesCount),
                BlueCubesCount = GameSets.Max(gs => gs.CubesSet.BlueCubesCount)
            };
        }

        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");

            var GameRounds = lines.Select(GameRound.Parse).ToList();
            var gameRoundCondtion = new CubesSet() { RedCubesCount = 12, GreenCubesCount = 13, BlueCubesCount = 14 };

            Console.WriteLine(Task1(GameRounds, gameRoundCondtion));
            Console.WriteLine(Task2(GameRounds));
        }

        private static int Task1(List<GameRound> GameRounds, CubesSet gameRoundCondition) =>
            GameRounds.Where(gr => gr.GameSets.All(gs => gs.IsPossibleSet(gameRoundCondition))).Sum(gr => gr.Id);

        private static int Task2(List<GameRound> GameRounds) => GameRounds.Select(gr => gr.FindSuccessfullRoundCondition()).
                                                                           Sum(grc => grc.RedCubesCount * grc.GreenCubesCount * grc.BlueCubesCount);
    }
}