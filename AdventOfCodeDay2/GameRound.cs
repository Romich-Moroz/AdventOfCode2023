namespace AdventOfCodeDay2
{
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
}