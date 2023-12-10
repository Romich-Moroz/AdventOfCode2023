namespace AdventOfCodeDay10
{
    public class PipeInfo(int lineId, int columntId, char pipeChar, params Directions[] directions)
    {
        public static readonly Dictionary<char, Directions> PipeConstants = new()
        {
            { '.', Directions.None },
            { '|', Directions.Top | Directions.Bottom },
            { '-', Directions.Left | Directions.Right },
            { 'L', Directions.Top | Directions.Right },
            { 'J', Directions.Top | Directions.Left },
            { '7', Directions.Bottom | Directions.Left },
            { 'F', Directions.Bottom | Directions.Right },
            { 'S', Directions.Left | Directions.Top | Directions.Right | Directions.Bottom },
        };

        public int LineId { get; } = lineId;
        public int ColumnId { get; } = columntId;
        public (int, int) Point => (LineId, ColumnId);

        public bool Left => IsConnected(Directions.Left);
        public bool Top => IsConnected(Directions.Top);
        public bool Right => IsConnected(Directions.Right);
        public bool Bottom => IsConnected(Directions.Bottom);

        public Directions Direction { get; set; } = directions.Aggregate((x, y) => x | y);
        public int Length { get; set; } = -1;
        public bool IsVisited { get; set; } = false;
        public char PipeChar { get; set; } = pipeChar;

        public bool IsConnected(Directions direction) => (Direction & direction) != 0;

        public bool IsConnected(PipeInfo? dest, Directions direction) => dest is not null && dest.Direction != Directions.None
        && direction switch
        {
            Directions.Left => Left & dest.Right,
            Directions.Top => Top & dest.Bottom,
            Directions.Right => Right & dest.Left,
            Directions.Bottom => Bottom & dest.Top,
            _ => false,
        };
    }
}
