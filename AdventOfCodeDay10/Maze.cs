namespace AdventOfCodeDay10
{
    public class Maze(List<List<PipeInfo>> pipes)
    {
        public List<List<PipeInfo>> PipeLines { get; } = pipes;
        public List<(int, int)> PipeLoop { get; } = [];
        public List<List<(int, int)>> PipeLoopRegions { get; } = [];

        public static Maze Parse(string[] lines) => new(lines.Select((l, i) => l.Select((c, j) => new PipeInfo(i, j, c, PipeInfo.PipeConstants[c])).ToList()).ToList());

        public void GeneratePath()
        {
            PipeInfo start = FindStart();
            start.Direction = AdjacentPipes.GetDirectionFromAdjacentPipes(GetAdjacentPipes(start));
            start.PipeChar = PipeInfo.PipeConstants.Single(p => p.Value == start.Direction).Key;

            List<(int, PipeInfo)> pipesToCheck = [(0, start)];
            while (pipesToCheck.Count != 0)
            {
                (int, PipeInfo) workingInfo = pipesToCheck.First();
                PipeInfo workingPipeInfo = workingInfo.Item2;
                var lengthCounter = workingInfo.Item1;

                if (!workingPipeInfo.IsVisited)
                {
                    PipeLoop.Add(workingPipeInfo.Point);
                    workingPipeInfo.Length = lengthCounter;
                    workingPipeInfo.IsVisited = true;

                    AdjacentPipes adjacentPipes = GetAdjacentPipes(workingPipeInfo);

                    void ConditionalInsert(PipeInfo? p, Directions direction)
                    {
                        if (p is not null && workingPipeInfo.IsConnected(p, direction))
                        {
                            pipesToCheck.Insert(0, (lengthCounter + 1, p));
                        }
                    }

                    ConditionalInsert(adjacentPipes.Bottom, Directions.Bottom);
                    ConditionalInsert(adjacentPipes.Right, Directions.Right);
                    ConditionalInsert(adjacentPipes.Top, Directions.Top);
                    ConditionalInsert(adjacentPipes.Left, Directions.Left);
                }

                _ = pipesToCheck.Remove((lengthCounter, workingPipeInfo));
            }
        }

        public int GetInnerRegionsCount()
        {
            PipeLoopRegions.Clear();
            PipeLines.ForEach(p => p.ForEach(i => i.PipeChar = i.Length == -1 ? '.' : i.PipeChar));
            PipeLines.ForEach(p => p.ForEach(i => { if (IsPointInPolygon(i.LineId, i.ColumnId)) { FillRegion(i); } }));
            return PipeLoopRegions.Sum(r => r.Count);
        }

        private AdjacentPipes GetAdjacentPipes(PipeInfo pipeInfo) => new(
            pipeInfo.ColumnId > 0 ? PipeLines[pipeInfo.LineId][pipeInfo.ColumnId - 1] : null,
            pipeInfo.LineId > 0 ? PipeLines[pipeInfo.LineId - 1][pipeInfo.ColumnId] : null,
            pipeInfo.ColumnId < PipeLines[0].Count - 1 ? PipeLines[pipeInfo.LineId][pipeInfo.ColumnId + 1] : null,
            pipeInfo.LineId < PipeLines.Count - 1 ? PipeLines[pipeInfo.LineId + 1][pipeInfo.ColumnId] : null
        );

        private PipeInfo FindStart() => PipeLines.Single(p => p.Any(C => C.Direction == Directions.All)).Single(s => s.Direction == Directions.All);

        private void FillRegion(PipeInfo start)
        {
            List<PipeInfo> pipesToCheck = [start];
            List<(int, int)> region = [];

            while (pipesToCheck.Count != 0)
            {
                PipeInfo workingPipeInfo = pipesToCheck.First();
                (int, int) point = new(workingPipeInfo.LineId, workingPipeInfo.ColumnId);

                if (!workingPipeInfo.IsVisited && IsPointInPolygon(point.Item1, point.Item2) && PipeLoopRegions.All(r => !r.Contains(point)))
                {
                    region.Add(point);
                    workingPipeInfo.IsVisited = true;

                    AdjacentPipes adjacentPipes = GetAdjacentPipes(workingPipeInfo);

                    void ConditionalInsert(PipeInfo? p)
                    {
                        if (p != null)
                        {
                            pipesToCheck.Insert(0, p);
                        }
                    }

                    ConditionalInsert(adjacentPipes.Bottom);
                    ConditionalInsert(adjacentPipes.Right);
                    ConditionalInsert(adjacentPipes.Top);
                    ConditionalInsert(adjacentPipes.Left);
                }

                _ = pipesToCheck.Remove(workingPipeInfo);
            }

            if (region.Count != 0)
            {
                PipeLoopRegions.Add(region);
            }
        }

        private bool IsPointInPolygon(int x, int y) => Enumerable.Range(0, y).Count(i => "|LJ".Contains(PipeLines[x][i].PipeChar)) % 2 == 1;
    }
}
