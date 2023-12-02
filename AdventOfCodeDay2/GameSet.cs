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
}
