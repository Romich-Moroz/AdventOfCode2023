namespace AdventOfCodeDay2
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
                var number = int.Parse(line.Split(' ')[0]);
                result.CubesSet.RedCubesCount = line.Contains(Keywords[0]) ? number : result.CubesSet.RedCubesCount;
                result.CubesSet.BlueCubesCount = line.Contains(Keywords[1]) ? number : result.CubesSet.BlueCubesCount;
                result.CubesSet.GreenCubesCount = line.Contains(Keywords[2]) ? number : result.CubesSet.GreenCubesCount;
            }

            return result;
        }

        public bool IsPossibleSet(CubesSet gameRoundCondition) =>
            CubesSet.GreenCubesCount <= gameRoundCondition.GreenCubesCount
            && CubesSet.RedCubesCount <= gameRoundCondition.RedCubesCount
            && CubesSet.BlueCubesCount <= gameRoundCondition.BlueCubesCount;
    }
}