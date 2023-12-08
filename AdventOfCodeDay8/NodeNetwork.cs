namespace AdventOfCodeDay8
{
    public class NodeNetwork(string path, Dictionary<string, Node> nodes)
    {
        public string Path { get; } = path;
        public Dictionary<string, Node> Nodes { get; } = nodes;

        public static NodeNetwork Parse(string[] lines) => new(lines[0], lines.Skip(2).Select(Node.Parse).ToDictionary(n => n.Key));

        public long FindGhostPathLegnth() => LeastCommonMultiple(
            Nodes.Where(n => n.Key.EndsWith('A')).
                  Select(p => (long)FindPathLength(p.Value.Key, (n) => n.Key.EndsWith('Z')))
        );

        public int FindPathLength(string start = "AAA", Func<Node, bool>? predicate = null)
        {
            var result = 0;

            Node CurrentNode = Nodes.Single(n => n.Key == start).Value;
            while (!predicate?.Invoke(CurrentNode) ?? CurrentNode.Key != "ZZZ")
            {
                var isLeft = Path[result % Path.Length] == 'L';
                CurrentNode = Nodes[isLeft ? CurrentNode.Left : CurrentNode.Right];
                result++;
            }

            return result;
        }

        private static long LeastCommonMultiple(IEnumerable<long> numbers) => numbers.Aggregate(LeastCommonMultiple);
        private static long LeastCommonMultiple(long a, long b) => Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        private static long GreatestCommonDivisor(long a, long b) => b == 0 ? a : GreatestCommonDivisor(b, a % b);
    }
}
