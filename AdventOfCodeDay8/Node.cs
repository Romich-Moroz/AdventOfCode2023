namespace AdventOfCodeDay8
{
    public class Node(string key, string left, string right)
    {
        public string Key { get; } = key;
        public string Left { get; } = left;
        public string Right { get; } = right;

        public static Node Parse(string line)
        {
            var split = line.Split('=', StringSplitOptions.TrimEntries);
            var split2 = split[1].Split(',', StringSplitOptions.TrimEntries);
            return new Node(split[0], split2[0], split2[1]);
        }
    }
}
