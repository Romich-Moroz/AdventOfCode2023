namespace AdventOfCodeDay5
{
    public class Almanac
    {
        public List<uint> Seeds { get; set; } = [];
        public List<Range> SeedsRange { get; set; } = [];
        public List<AlmanacMap> AlmanacMaps { get; set; } = [];

        public static Almanac Parse(string input)
        {
            var split = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            var seeds = split[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ').Select(uint.Parse).ToList();

            return new Almanac()
            {
                Seeds = seeds,
                SeedsRange = new List<Range>(Enumerable.Range(0, seeds.Count).Where(i => i % 2 == 0).Select(i => new Range(seeds[i], seeds[i + 1])).ToList()),
                AlmanacMaps = split.Skip(1).Select(l => AlmanacMap.Parse(l.Split("\r\n"))).ToList()
            };
        }

        public List<uint> GetMappedValues(List<uint> seeds)
        {
            List<uint> result = seeds;

            foreach (AlmanacMap map in AlmanacMaps)
            {
                result = result.Select(map.GetValue).ToList();
            }

            return result;
        }

        public List<Range> GetMappedValues(List<Range> seeds)
        {
            List<Range> localSeeds = seeds;

            for (var i = 0; i < AlmanacMaps.Count; i++)
            {
                localSeeds = localSeeds.Select(seed => AlmanacMaps[i].GetValue(seed)).ToList().SelectMany(r => r).ToList();
            }

            return localSeeds;
        }
    }
}
