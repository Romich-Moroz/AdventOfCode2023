namespace AdventOfCodeDay3
{
    public class Number(string? previousLine, string currentLine, string? nextLine, NumberInfo numberInfo)
    {
        public string? PreviousLine { get; set; } = previousLine;
        public string CurrentLine { get; set; } = currentLine;
        public string? NextLine { get; set; } = nextLine;

        public NumberInfo NumberInfo { get; set; } = numberInfo;

        public bool IsPartOfEngine()
        {
            var backwardsDiagonalSearch = NumberInfo.ColumnStartIndex != 0 ? 1 : 0;
            var searchStart = NumberInfo.ColumnStartIndex - backwardsDiagonalSearch;

            var forwardDiagonalSearch = NumberInfo.ColumnEndIndex != CurrentLine.Length - 1 ? 1 : 0;
            var searchEnd = NumberInfo.ColumnEndIndex + forwardDiagonalSearch;

            var searchLength = searchEnd - searchStart + 1;

            return PreviousLine?.Substring(searchStart, searchLength).Any(c => c != '.') is true
                || (backwardsDiagonalSearch != 0 && CurrentLine[searchStart] != '.')
                || ((forwardDiagonalSearch != 0) && (CurrentLine[searchEnd] != '.'))
                || NextLine?.Substring(searchStart, searchLength).Any(c => c != '.') is true;
        }

        public bool IsCloseToGear(int gearRow, int gearColumn) => gearRow >= NumberInfo.LineId - 1
                                                               && gearRow <= NumberInfo.LineId + 1
                                                               && gearColumn >= NumberInfo.ColumnStartIndex - 1
                                                               && gearColumn <= NumberInfo.ColumnEndIndex + 1;

        public static List<Number> Parse(string[] input)
        {
            var result = new List<Number>();

            for (var i = 0; i < input.Length; i++)
            {
                var prevLine = i - 1 >= 0 ? input[i - 1] : null;
                var nextLine = i + 1 < input.Length ? input[i + 1] : null;

                for (var j = 0; j < input[i].Length; j++)
                {
                    if (char.IsDigit(input[i][j]))
                    {
                        var number = new Number(prevLine, input[i], nextLine, NumberInfo.Parse(i, input[i], j));
                        j = number.NumberInfo.ColumnEndIndex;
                        result.Add(number);
                    }
                }
            }

            return result;
        }
    }
}
