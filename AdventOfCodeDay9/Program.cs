namespace AdventOfCodeDay9
{
    public class OasisReport(List<List<int>> history)
    {
        public List<List<int>> History { get; } = history;
        public static OasisReport Parse(string[] lines) => new(lines.Select(l => l.Split(' ').Select(s => int.Parse(s)).ToList()).ToList());

        public int GetPrediction(bool isForward) => History.Select(e => GetPrediction(e, isForward)).Sum();

        private int GetPrediction(List<int> historyEntry, bool forward)
        {
            List<List<int>> predictions = [historyEntry];
            List<int> newPrediction = historyEntry;
            do
            {
                newPrediction = newPrediction.SkipLast(1).Select((e, i) => newPrediction[i + 1] - e).ToList();
                predictions.Add(newPrediction);
            }
            while (!newPrediction.All(i => i == 0));

            for (var i = predictions.Count - 1; i > 0; i--)
            {
                if (forward)
                {
                    predictions[i - 1].Add(predictions[i].Last() + predictions[i - 1].Last());
                }
                else
                {
                    predictions[i - 1].Insert(0, predictions[i - 1].First() - predictions[i].First());
                }
            }

            return forward ? predictions[0].Last() : predictions[0].First();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var report = OasisReport.Parse(File.ReadAllLines("../../../input.txt"));
            Console.WriteLine(report.GetPrediction(true));
            Console.WriteLine(report.GetPrediction(false));
        }
    }
}
