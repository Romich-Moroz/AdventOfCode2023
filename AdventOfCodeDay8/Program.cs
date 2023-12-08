namespace AdventOfCodeDay8
{
    internal class Program
    {
        public class Node(string key, string left, string right)
        {
            public string Key { get; set; } = key;
            public string Left { get; set; } = left;
            public string Right { get; set; } = right;

            public static Node Parse(string line)
            {
                var split = line.Split('=', StringSplitOptions.TrimEntries);
                var split2 = split[1].Split(',', StringSplitOptions.TrimEntries);
                return new Node(split[0], split2[0], split2[1]);
            }
        }

        public class Network(string path, Dictionary<string, Node> nodes)
        {
            public string Path { get; } = path;
            public Dictionary<string, Node> Nodes { get; } = nodes;


            public static Network Parse(string[] lines) => new(lines[0], lines.Skip(2).Select(Node.Parse).ToDictionary(n => n.Key));


            public long FindGhostPathLegnth()
            {
                var startNodes = Nodes.Where(n => n.Key.EndsWith('A')).Select(p => p.Value).ToList();
                var endNodes = Nodes.Where(n => n.Key.EndsWith('Z')).Select(p => p.Value).ToList();

                var result = startNodes.Select(sn => FindPathLength(sn.Key, (n) => n.Key.EndsWith('Z'))).ToArray();

                return LCM(result.Select(i => (long)i).ToArray());
            }

            public int FindPathLength(string start = "AAA", Func<Node, bool>? predicate = null)
            {
                int result = 0;

                var CurrentNode = Nodes.Single(n => n.Key == start).Value;

                while(predicate is null ? CurrentNode.Key != "ZZZ" : !predicate.Invoke(CurrentNode))
                {
                    var isLeft = Path[(int)(result % Path.Length)] == 'L';
                    CurrentNode = Nodes[isLeft ? CurrentNode.Left : CurrentNode.Right];
                    result++;
                }

                return result;
            }

            static long LCM(long[] numbers)
            {
                return numbers.Aggregate(lcm);
            }
            static long lcm(long a, long b)
            {
                return Math.Abs(a * b) / GCD(a, b);
            }
            static long GCD(long a, long b)
            {
                return b == 0 ? a : GCD(b, a % b);
            }
        }

        static void Main(string[] args)
        {
            var text = File.ReadAllText("../../../input.txt").Replace("(", null).Replace(")", null).Split("\r\n", StringSplitOptions.TrimEntries);
            var net = Network.Parse(text);

            Console.WriteLine(net.FindPathLength());
            Console.WriteLine(net.FindGhostPathLegnth());
        }
    }
}
