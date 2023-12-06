namespace AdventOfCodeDay5
{
    public class Range(uint start, uint length)
    {
        public uint Start { get; } = start;
        public uint Length { get; } = length;

        public uint End => Start + Length - 1;

        public bool Contains(uint value) => Start <= value && value <= End;
        public bool IntersectsWith(Range value) => new RangesIntersection(this, value).IntersectionRange is not null;
        public uint GetOffset(uint value) => value - Start;

        public override bool Equals(object? obj) => obj is Range r && Start == r.Start && Length == r.Length;
        public override int GetHashCode() => (Start + Length).GetHashCode();
    }
}
