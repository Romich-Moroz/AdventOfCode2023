namespace AdventOfCodeDay5
{
    public class MapEntry(uint key, uint value, uint mapLength)
    {
        public Range Key { get; } = new Range(key, mapLength);
        public Range Value { get; } = new Range(value, mapLength);

        public uint GetValue(uint key) => Key.GetOffset(key) + Value.Start;
        public Range GetValue(Range key) => new(Key.GetOffset(key.Start) + Value.Start, key.Length);

        public bool CanMap(uint key) => Key.Contains(key);
        public bool CanMap(Range key) => Key.IntersectsWith(key);
    }
}
