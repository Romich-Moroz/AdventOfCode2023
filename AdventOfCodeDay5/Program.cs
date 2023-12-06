using System.Diagnostics;

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
                    if (r1.End < r2.End)
                    {
                        IntersectingRange = new Range(r2.Start, r1.End - r2.Start + 1);
                    }
                    else
                    {
                        IntersectingRange = new Range(r2.Start, r2.End - r2.Start + 1);
                    }
                }
                else if (r1.Start <= r2.End && r2.End <= r1.End)
                {
                    if (r1.End < r2.End)
                    {
                        IntersectingRange = new Range(r1.Start, r1.End - r1.Start + 1);
                    }
                    else
                    {
                        IntersectingRange = new Range(r1.Start, r2.End - r1.Start + 1);
                    }                   
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
            public bool IntersectsWith(Range value) => new Intersection(this, value).IntersectingRange is not null;
            public uint GetOffset(uint value) => value - Start;

            public override bool Equals(object? obj)
            {
                if (obj is Range r)
                {
                    return Start == r.Start && Length == r.Length;
                }

                return false;
            }
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

            public Range GetValue(Range key) => new(Key.GetOffset(key.Start) + Value.Start, key.Length);

            public bool CanMap(uint key) => Key.Contains(key);
            public bool CanMap(Range key) => Key.IntersectsWith(key);


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
                    var tmp = new List<List<Range>>();
                    foreach(var seed in result)
                    {
                        var val = map.GetValue(seed);
                        tmp.Add(val);
                    }
                    var newSeeds = tmp.SelectMany(r => r).ToList();
                    result = newSeeds;
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
                List<Range> unmappedKeyParts = new List<Range>();
                List<Range> mappedKeyParts = new List<Range>();
                List<Range> rangesToExclude = new List<Range>();
                foreach(var mapEntry in MapEntries)
                {
                    if (mapEntry.CanMap(key))
                    {
                        Intersection intersection = new Intersection(mapEntry.Key, key);

                        if (intersection.BeforeIntersectionRange is not null && !rangesToExclude.Contains(intersection.BeforeIntersectionRange))
                        {
                            Intersection unmappedLeft = new Intersection(intersection.BeforeIntersectionRange, key);
                            if (unmappedLeft is not null && unmappedLeft.IntersectingRange is not null)
                            {
                                unmappedKeyParts.Add(unmappedLeft.IntersectingRange);
                            }
                        }

                        if (intersection.IntersectingRange is not null)
                        {
                            rangesToExclude.Add(intersection.IntersectingRange);
                            mappedKeyParts.Add(mapEntry.GetValue(intersection.IntersectingRange));
                        }

                        if (intersection.AfterIntersectionRange is not null && !rangesToExclude.Contains(intersection.AfterIntersectionRange))
                        {
                            var unmappedRight = new Intersection(intersection.AfterIntersectionRange, key);
                            if (unmappedRight is not null && unmappedRight.IntersectingRange is not null)
                            {
                                unmappedKeyParts.Add(unmappedRight.IntersectingRange);
                            }
                        }
                    }

                    var unmappedToRemove = new List<Range>();
                    var unmappedToAdd = new List<Range>();
                    foreach(var unmapped in unmappedKeyParts)
                    {
                        if (mapEntry.CanMap(unmapped))
                        {
                            unmappedToRemove.Add(unmapped);

                            Intersection intersection = new Intersection(mapEntry.Key, unmapped);

                            if (intersection.BeforeIntersectionRange is not null)
                            {
                                Intersection unmappedLeft = new Intersection(intersection.BeforeIntersectionRange, unmapped);
                                if (unmappedLeft is not null && unmappedLeft.IntersectingRange is not null)
                                {
                                    unmappedToAdd.Add(unmappedLeft.IntersectingRange);
                                }
                            }

                            if (intersection.IntersectingRange is not null && !rangesToExclude.Contains(intersection.IntersectingRange))
                            {
                                mappedKeyParts.Add(mapEntry.GetValue(intersection.IntersectingRange));
                            }

                            if (intersection.AfterIntersectionRange is not null)
                            {
                                var unmappedRight = new Intersection(intersection.AfterIntersectionRange, unmapped);
                                if (unmappedRight is not null && unmappedRight.IntersectingRange is not null)
                                {
                                    unmappedToAdd.Add(unmappedRight.IntersectingRange);
                                }
                            }
                        }
                    }
                    unmappedKeyParts.AddRange(unmappedToAdd);

                    unmappedToRemove.ForEach(u => unmappedKeyParts.Remove(u));

                }

                if (mappedKeyParts.Count == 0)
                {
                    mappedKeyParts.Add(key);
                }

                mappedKeyParts.AddRange(unmappedKeyParts);
                return mappedKeyParts;


                //Returns duplicates thats why its not working
                //IEnumerable<MapEntry> entries = MapEntries.Where(md => md.CanMap(key));
                //if (entries.Any())
                //{
                //    List<MappedValue> values = new List<MappedValue>();

                //    foreach (var entry in entries)
                //    {
                //        var value = entry.GetValue(key);
                //        List<Range> ranges = new List<Range>();
                //        foreach (var en in value.PendingMap)
                //        {
                //            if (entry.CanMap(en))
                //            {
                //                values.Add(entry.GetValue(en));
                //                ranges.Add(en);
                //            }
                //        }
                //        value.PendingMap.RemoveAll(m => ranges.Contains(m));
                        
                //        values.Add(value);
                //    }

                //    var result = new List<Range>();
                //    foreach (MappedValue? value in values)
                //    {
                //        var itemsToRemove = value.PendingMap.Where(pm => entries.Any(e => e.IntersectsWith(pm))).ToList();
                //        itemsToRemove.ForEach(pm => value.PendingMap.Remove(pm));

                //        result.Add(value.AppliedMap);
                //        result.AddRange(value.PendingMap);
                //    }

                //    return result;
                //}

                //return [key];
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
