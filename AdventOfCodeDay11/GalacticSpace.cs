namespace AdventOfCodeDay11
{
    public class GalacticSpace(List<List<GalacticObject>> galacticObjects)
    {
        private List<List<GalacticObject>> GalacticObjects { get; } = galacticObjects;
        private List<GalacticObject> Galaxies { get; } = galacticObjects.Select(go => go.Where(g => g.Char == '#')).SelectMany(g => g).ToList();

        public static GalacticSpace Parse(string[] lines) => new(lines.Select((l, i) => l.Select((c, j) => new GalacticObject(i, j, c)).ToList()).ToList());

        public void Expand(int scale)
        {
            Enumerable.Range(0, GalacticObjects[0].Count).
                       Where(i => GalacticObjects.All(g => g[i].Char == '.')).
                       ToList().
                       ForEach(i => GalacticObjects.ForEach(g => g[i].Scale = scale));

            GalacticObjects.Where(g => g.All(g => g.Char == '.')).
                            ToList().
                            ForEach(l => l.ForEach(i => i.Scale = scale));
        }

        public long GetShortestPath() => Galaxies.Sum(gSource => Galaxies.Where(gDest => gDest != gSource).Sum(gDest => GetShortestPath(gSource, gDest))) / 2;

        public long GetShortestPath(GalacticObject from, GalacticObject to)
        {
            (int, int) path = from.GetPathTo(to);
            var result = 0L;

            var iStep = path.Item1 > 0 ? 1 : -1;
            for (var i = from.RowId; i != from.RowId + path.Item1; i += iStep)
            {
                result += GalacticObjects[i][from.ColumnId].Scale;
            }

            var jStep = path.Item2 > 0 ? 1 : -1;
            for (var j = from.ColumnId; j != from.ColumnId + path.Item2; j += jStep)
            {
                result += GalacticObjects[from.RowId + path.Item1][j].Scale;
            }

            return result;
        }
    }
}
