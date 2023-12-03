namespace AdventOfCodeDay3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");
            List<Number> numbers = Number.Parse(lines);

            Console.WriteLine(Task1(numbers));
            Console.WriteLine(Task2(lines, numbers));
        }

        private static int Task1(List<Number> numbers) => numbers.Where(n => n.IsPartOfEngine()).
                                                                  Sum(n => n.NumberInfo.NumberValue);

        private static int Task2(string[] lines, List<Number> numbers)
        {
            var result = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] is '*')
                    {
                        var gearNumbers = numbers.Where(n => n.IsCloseToGear(i, j)).Select(n => n.NumberInfo.NumberValue).ToList();

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
