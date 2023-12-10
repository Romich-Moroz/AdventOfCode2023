namespace AdventOfCodeDay10
{
    public class AdjacentPipes(PipeInfo? left, PipeInfo? top, PipeInfo? right, PipeInfo? bottom)
    {
        public PipeInfo? Left { get; } = left;
        public PipeInfo? Top { get; } = top;
        public PipeInfo? Right { get; } = right;
        public PipeInfo? Bottom { get; } = bottom;

        public static Directions GetDirectionFromAdjacentPipes(AdjacentPipes adjacentPipes)
        {
            Directions left = adjacentPipes?.Left?.Right is true ? Directions.Left : Directions.None;
            Directions top = adjacentPipes?.Top?.Bottom is true ? Directions.Top : Directions.None;
            Directions right = adjacentPipes?.Right?.Left is true ? Directions.Right : Directions.None;
            Directions bottom = adjacentPipes?.Bottom?.Top is true ? Directions.Bottom : Directions.None;

            return left | top | right | bottom;
        }
    }
}
