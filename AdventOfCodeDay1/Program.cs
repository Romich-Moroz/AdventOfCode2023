using AdventOfCode.Input;

namespace AdventOfCodeDay1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");

            Console.WriteLine(Task1(lines));
            Console.WriteLine(Task2(lines));
        }
        private static int GetDigitsSum(string input) =>
            ((input.First(char.IsDigit) - InputConstants.CharToIntegerOffset) * 10) +
            input.Last(char.IsDigit) - InputConstants.CharToIntegerOffset;

        private static int Task1(string[] lines) => lines.Sum(GetDigitsSum);

        private static int GetSumOfLine(string line)
        {
            var digits = "";

            for (var i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    digits += line[i] - InputConstants.CharToIntegerOffset;
                }
                else
                {
                    for (var j = 0; j < InputConstants.DigitStrings.Length; j++)
                    {
                        var digitString = InputConstants.DigitStrings[j];
                        if (i + digitString.Length <= line.Length && line.Substring(i, digitString.Length) == digitString)
                        {
                            digits += j + 1;
                        }
                    }
                }
            }

            return GetDigitsSum(digits);
        }

        private static int Task2(string[] lines) => lines.Sum(GetSumOfLine);
    }
}