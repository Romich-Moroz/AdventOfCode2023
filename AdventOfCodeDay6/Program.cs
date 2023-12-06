Func<long, long, long> getTotalWaysToWin = (time, dist) => Enumerable.Range(0, (int)time).Where(i => ((time - i) * (1 * i)) > dist).Count();

List<Tuple<long, long>> task1 = [new(62, 644), new(73, 1023), new(75, 1240), new(65, 1023)];
List<Tuple<long, long>> task2 = [new(62737565, 644102312401023)];

Console.WriteLine(task1.Select(r => getTotalWaysToWin(r.Item1, r.Item2)).Aggregate((x, y) => x * y));
Console.WriteLine(task2.Select(r => getTotalWaysToWin(r.Item1, r.Item2)).Aggregate((x, y) => x * y));