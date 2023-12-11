namespace AdventOfCodeDay11
{
    public class GalacticObject(int rowId, int columnId, char ch)
    {
        public int RowId { get; set; } = rowId;
        public int ColumnId { get; set; } = columnId;
        public char Char { get; } = ch;
        public int Scale { get; set; } = 1;

        public (int, int) GetPathTo(GalacticObject galacticObject) => (galacticObject.RowId - RowId, galacticObject.ColumnId - ColumnId);
    }
}
