namespace AdventOfCodeDay5
{
    public class AlmanacMap
    {
        private List<MapEntry> MapEntries { get; set; } = [];

        public static AlmanacMap Parse(string[] input)
        {
            var result = new AlmanacMap();

            foreach (var line in input.Skip(1))
            {
                var split = line.Split(' ');
                result.MapEntries.Add(new MapEntry(
                    uint.Parse(split[1]),
                    uint.Parse(split[0]),
                    uint.Parse(split[2])
                ));
            }

            return result;
        }

        public uint GetValue(uint key) => MapEntries.SingleOrDefault(md => md.CanMap(key))?.GetValue(key) ?? key;

        public List<Range> GetValue(Range key)
        {
            List<Range> unmappedKeyParts = [];
            List<Range> mappedKeyParts = [];
            List<Range> rangesToExclude = [];

            foreach (MapEntry mapEntry in MapEntries)
            {
                if (mapEntry.CanMap(key))
                {
                    var intersection = new RangesIntersection(mapEntry.Key, key);

                    if (intersection.RangeBeforeIntersection is not null && !rangesToExclude.Contains(intersection.RangeBeforeIntersection))
                    {
                        var unmappedLeft = new RangesIntersection(intersection.RangeBeforeIntersection, key);
                        if (unmappedLeft is not null && unmappedLeft.IntersectionRange is not null)
                        {
                            unmappedKeyParts.Add(unmappedLeft.IntersectionRange);
                        }
                    }

                    if (intersection.IntersectionRange is not null)
                    {
                        rangesToExclude.Add(intersection.IntersectionRange);
                        mappedKeyParts.Add(mapEntry.GetValue(intersection.IntersectionRange));
                    }

                    if (intersection.RangeAfterIntersection is not null && !rangesToExclude.Contains(intersection.RangeAfterIntersection))
                    {
                        var unmappedRight = new RangesIntersection(intersection.RangeAfterIntersection, key);
                        if (unmappedRight is not null && unmappedRight.IntersectionRange is not null)
                        {
                            unmappedKeyParts.Add(unmappedRight.IntersectionRange);
                        }
                    }
                }

                var unmappedToRemove = new List<Range>();
                var unmappedToAdd = new List<Range>();
                foreach (Range unmapped in unmappedKeyParts)
                {
                    if (mapEntry.CanMap(unmapped))
                    {
                        unmappedToRemove.Add(unmapped);

                        var intersection = new RangesIntersection(mapEntry.Key, unmapped);

                        if (intersection.RangeBeforeIntersection is not null)
                        {
                            var unmappedLeft = new RangesIntersection(intersection.RangeBeforeIntersection, unmapped);
                            if (unmappedLeft is not null && unmappedLeft.IntersectionRange is not null)
                            {
                                unmappedToAdd.Add(unmappedLeft.IntersectionRange);
                            }
                        }

                        if (intersection.IntersectionRange is not null && !rangesToExclude.Contains(intersection.IntersectionRange))
                        {
                            mappedKeyParts.Add(mapEntry.GetValue(intersection.IntersectionRange));
                        }

                        if (intersection.RangeAfterIntersection is not null)
                        {
                            var unmappedRight = new RangesIntersection(intersection.RangeAfterIntersection, unmapped);
                            if (unmappedRight is not null && unmappedRight.IntersectionRange is not null)
                            {
                                unmappedToAdd.Add(unmappedRight.IntersectionRange);
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
        }
    }
}
