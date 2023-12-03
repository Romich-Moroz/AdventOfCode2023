namespace AdventOfCodeDay1
{
    public class Program
    {
        public const int CharToIntegerOffset = 0x30;
        public static readonly string[] DigitStrings =
        [
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine"
        ];

        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");

            Console.WriteLine(Task1(lines));
            Console.WriteLine(Task2(lines));
        }
        private static int GetDigitsSum(string input) =>
            ((input.First(char.IsDigit) - CharToIntegerOffset) * 10) +
            input.Last(char.IsDigit) - CharToIntegerOffset;

        private static int Task1(string[] lines) => lines.Sum(GetDigitsSum);

        private static int GetSumOfLine(string line)
        {
            var digits = "";

            for (var i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    digits += line[i] - CharToIntegerOffset;
                }
                else
                {
                    for (var j = 0; j < DigitStrings.Length; j++)
                    {
                        var digitString = DigitStrings[j];
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