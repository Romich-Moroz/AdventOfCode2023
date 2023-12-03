namespace AdventOfCodeDay3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");
            var numbers = Number.Parse(lines);
            
            Console.WriteLine(Task1(numbers));
            Console.WriteLine(Task2(lines, numbers));
        }

        public class NumberLocationInfo
        {
            public int LineIndex { get; set; }
            public int ColumnStartIndex { get; set; }
            public int NumberValue { get; set; }
            public int ColumnEndIndex { get; set; }
        }

        public class Number
        {
            public string? PreviousLine { get; set; }
            public string CurrentLine { get; set; } = null!;
            public string? NextLine { get; set; }

            public NumberLocationInfo NumberLocationInfo { get; set; } = new();
            

            public bool IsPartOfEngine()
            {
                int backwardsDiagonalSearch = NumberLocationInfo.ColumnStartIndex != 0 ? 1 : 0;
                var searchStart = NumberLocationInfo.ColumnStartIndex - backwardsDiagonalSearch;
                
                int forwardDiagonalSearch = NumberLocationInfo.ColumnEndIndex != CurrentLine.Length - 1 ? 1 : 0;
                var searchEnd = NumberLocationInfo.ColumnEndIndex + forwardDiagonalSearch;

                var searchLength = searchEnd - searchStart + 1;

                return PreviousLine?.Substring(searchStart, searchLength).Any(c => c != '.') is true
                    || (backwardsDiagonalSearch != 0 && CurrentLine[searchStart] != '.')
                    || (forwardDiagonalSearch != 0) && (CurrentLine[searchEnd] != '.')
                    || NextLine?.Substring(searchStart, searchLength).Any(c => c != '.') is true;
            }

            public bool IsPartOfGear(int gearRow, int gearColumn) => gearRow >= NumberLocationInfo.LineIndex - 1 
                                                                  && gearRow <= NumberLocationInfo.LineIndex + 1 
                                                                  && gearColumn >= NumberLocationInfo.ColumnStartIndex - 1 
                                                                  && gearColumn <= NumberLocationInfo.ColumnEndIndex + 1;

            public static List<Number> Parse(string[] input)
            {
                var result = new List<Number>();

                for (int i = 0; i < input.Length; i++)
                {
                    var prevLine = i - 1 >= 0 ? input[i - 1] : null;
                    var nextLine = i + 1 < input.Length ? input[i + 1] : null;

                    for (int j = 0; j < input[i].Length; j++)
                    {
                        if (char.IsDigit(input[i][j]))
                        {
                            var number = new Number
                            {
                                NumberLocationInfo = Parse(i, input[i], j),
                                PreviousLine = prevLine,
                                CurrentLine = input[i],
                                NextLine = nextLine
                            };
                            result.Add(number);

                            j = number.NumberLocationInfo.ColumnEndIndex;
                        }
                    }
                }

                return result;
            }

            private static NumberLocationInfo Parse(int lineIndex, string currentLine, int startIndex)
            {
                int i;
                for (i = startIndex; i < currentLine.Length && char.IsDigit(currentLine[i]); i++) ;

                var result = new NumberLocationInfo()
                {
                    LineIndex = lineIndex,
                    ColumnStartIndex = startIndex,
                    ColumnEndIndex = i - 1,
                    NumberValue = int.Parse(currentLine[startIndex..i])
                };

                return result;
            }
        }

        private static int Task1(List<Number> numbers)
        {
            return numbers.Where(n => n.IsPartOfEngine()).Sum(n => n.NumberLocationInfo.NumberValue);
        }

        private static int Task2(string[] lines, List<Number> numbers)
        {
            var result = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] is '*')
                    {
                        var gearNumbers = numbers.Where(n => n.IsPartOfGear(i, j)).Select(n => n.NumberLocationInfo.NumberValue).ToList();
                        if (gearNumbers.Count == 2)
                        {
                            result += gearNumbers.First() * gearNumbers.Last();
                        }
                        
                    }
                }
            }

            return result;
        }
    }
}
