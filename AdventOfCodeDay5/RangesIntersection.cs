namespace AdventOfCodeDay5
{
    public class RangesIntersection
    {
        public Range? RangeBeforeIntersection { get; }
        public Range? IntersectionRange { get; }
        public Range? RangeAfterIntersection { get; }

        public RangesIntersection(Range r1, Range r2)
        {
            var minStart = Math.Min(r1.Start, r2.Start);
            var minEnd = Math.Min(r1.End, r2.End);
            var maxEnd = Math.Max(r1.End, r2.End);

            if (r1.Start <= r2.Start && r2.Start <= r1.End)
            {
                IntersectionRange = new Range(r2.Start, minEnd - r2.Start + 1);
            }
            else if (r1.Start <= r2.End && r2.End <= r1.End)
            {
                IntersectionRange = new Range(r1.Start, minEnd - r1.Start + 1);
            }

            if (minStart < IntersectionRange?.Start)
            {
                RangeBeforeIntersection = new Range(minStart, IntersectionRange.Start - minStart);
            }

            if (maxEnd > IntersectionRange?.End)
            {
                RangeAfterIntersection = new Range(IntersectionRange.End + 1, maxEnd - IntersectionRange.End);
            }
        }
    }
}
