namespace AdventOfCodeDay3
{
    public class NumberInfo(int lineId, int columnStartIndex, int numberValue, int columnEndIndex)
    {
        public int LineId { get; set; } = lineId;
        public int ColumnStartIndex { get; set; } = columnStartIndex;
        public int NumberValue { get; set; } = numberValue;
        public int ColumnEndIndex { get; set; } = columnEndIndex;

        public static NumberInfo Parse(int lineId, string currentLine, int startIndex)
        {
            int i;
            for (i = startIndex; i < currentLine.Length && char.IsDigit(currentLine[i]); i++)
            {
                ;
            }

            return new NumberInfo(lineId, startIndex, int.Parse(currentLine[startIndex..i]), i - 1);
        }
    }
}
