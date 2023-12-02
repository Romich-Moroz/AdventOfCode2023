namespace AdventOfCodeDay2
{
    public class Program
    {
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