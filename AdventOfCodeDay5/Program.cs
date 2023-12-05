namespace AdventOfCodeDay5
{
    internal class Program
    {
        public class Intersection
        {
            public Range? BeforeIntersectionRange { get; }
            public Range? IntersectingRange { get; }
            public Range? AfterIntersectionRange { get; }

            public Intersection(Range r1, Range r2)
            {
                if (r1.Start <= r2.Start && r2.Start <= r1.End)
                {
                    var end = r1.End < r2.End ? r1.End : r2.End;
                    IntersectingRange = new Range(r2.Start, end - r2.Start + 1);

                }
                else if (r1.Start <= r2.End && r2.End <= r1.End)
                {
                    var start = r1.Start > r2.Start ? r1.Start : r2.Start;
                    IntersectingRange = new Range(r2.End, r2.End - start + 1);
                }

                var minStart = r1.Start < r2.Start ? r1.Start : r2.Start;
                if (minStart < IntersectingRange?.Start)
                {
                    BeforeIntersectionRange = new Range(minStart, IntersectingRange.Start - minStart);
                }

                var maxEnd = r1.End > r2.End ? r1.End : r2.End;
                if (maxEnd > IntersectingRange?.End)
                {
                    AfterIntersectionRange = new Range(IntersectingRange.End + 1, maxEnd - IntersectingRange.End);
                }
            }
        }
        public class Range(uint start, uint length)
        {
            public uint Start { get; } = start;
            public uint Length { get; } = length;

            public uint End => Start + Length - 1;

            public bool Contains(uint value) => Start <= value && value <= End;
            public bool Contains(Range value) => Start < value.Start && value.End <= End;
            public bool IntersectsWith(Range value) => (Start <= value.Start && value.Start <= End) || (Start <= value.End && value.End <= End) || Contains(value) || value.Contains(this);
            public uint GetOffset(uint value) => value - Start;
        }

        public class MappedValue
        {
            public required Range AppliedMap { get; set; }
            public List<Range> PendingMap { get; set; } = [];
        }

        public class MapEntry(uint key, uint value, uint mapLength)
        {
            public Range Key { get; } = new Range(key, mapLength);
            public Range Value { get; } = new Range(value, mapLength);

            public uint MapLength { get; } = mapLength;

            public uint GetValue(uint key) => Key.GetOffset(key) + Value.Start;

            public MappedValue GetValue(Range key)
            {
                var result = new MappedValue() { AppliedMap = key };
                var intersection = new Intersection(Key, key);
                if (intersection.IntersectingRange is not null)
                {
                    if (intersection.BeforeIntersectionRange is not null && key.IntersectsWith(intersection.BeforeIntersectionRange))
                    {
                        result.PendingMap.Add(intersection.BeforeIntersectionRange);
                    }

                    result.AppliedMap = MapValues(intersection.IntersectingRange);
                    if (intersection.AfterIntersectionRange is not null && key.IntersectsWith(intersection.AfterIntersectionRange))
                    {
                        result.PendingMap.Add(intersection.AfterIntersectionRange);
                    }

                    return result;
                }

                return result;
            }

            public bool CanMap(uint key) => Key.Contains(key);
            public bool CanMap(Range key) => Key.IntersectsWith(key);

            private Range MapValues(Range range)
            {
                var offset = Key.GetOffset(range.Start);
                return new Range(offset + Value.Start, range.Length);
            }

            public bool IntersectsWith(Range range) => Key.IntersectsWith(range);
        }

        public class Almanac
        {
            public List<uint> Seeds { get; set; } = [];
            public List<Range> SeedsRange { get; set; } = [];
            public List<AlmanacMap> AlmanacMaps { get; set; } = [];

            public static Almanac Parse(string input)
            {
                var result = new Almanac();

                var split = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

                result.Seeds = split[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(' ').Select(uint.Parse).ToList();

                for (var i = 0; i < result.Seeds.Count; i += 2)
                {
                    result.SeedsRange.Add(new Range(result.Seeds[i], result.Seeds[i + 1]));
                }

                result.AlmanacMaps = split.Skip(1).Select(l => AlmanacMap.Parse(l.Split("\r\n"))).ToList();

                return result;
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
                List<Range> result = seeds;

                for (var i = 0; i < AlmanacMaps.Count; i++)
                {
                    AlmanacMap? map = AlmanacMaps[i];
                    IEnumerable<List<Range>> test = result.Select(map.GetValue);
                    var tmp = test.ToList();
                    result = test.SelectMany(r => r).ToList();
                }

                return result;
            }
        }

        public class AlmanacMap
        {
            private List<MapEntry> MapEntries { get; set; } = [];
            public static AlmanacMap Parse(string[] input)
            {
                var result = new AlmanacMap();

                foreach (var line in input.Skip(1))
                {
                    var split = line.Split(' ');
                    var dest = uint.Parse(split[0]);
                    var src = uint.Parse(split[1]);
                    var length = uint.Parse(split[2]);

                    result.MapEntries.Add(new MapEntry(src, dest, length));
                }

                return result;
            }

            public uint GetValue(uint key) => MapEntries.SingleOrDefault(md => md.CanMap(key))?.GetValue(key) ?? key;
            public List<Range> GetValue(Range key)
            {
                //Returns duplicates thats why its not working
                IEnumerable<MapEntry> entries = MapEntries.Where(md => md.CanMap(key));
                if (entries.Any())
                {
                    IEnumerable<MappedValue> values = entries.Select(md => md.GetValue(key));
                    var result = new List<Range>();
                    foreach (MappedValue? value in values)
                    {
                        var itemsToRemove = value.PendingMap.Where(pm => entries.Any(e => e.IntersectsWith(pm))).ToList();
                        itemsToRemove.ForEach(pm => value.PendingMap.Remove(pm));

                        result.Add(value.AppliedMap);
                        result.AddRange(value.PendingMap);
                    }

                    return result;
                }

                return [key];
            }
        }

        private static void Main(string[] args)
        {
            var lines = File.ReadAllText("../../../input.txt");

            var almanac = Almanac.Parse(lines);

            Console.WriteLine(Task1(almanac));

            Console.WriteLine(Task2(almanac));
        }

        public static uint Task1(Almanac almanac) => almanac.GetMappedValues(almanac.Seeds).Min();

        public static uint Task2(Almanac almanac)
        {
            List<Range> test = almanac.GetMappedValues(almanac.SeedsRange);
            return test.Min(s => s.Start);
        }
    }
}
