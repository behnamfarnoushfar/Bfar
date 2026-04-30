namespace Bfar.XCutting.Abstractions.Entities.Models
{
    public sealed class PaginationModel
    {
        public int PageSize { get; set; }
        public int OffSet { get; set; }
        public int Total { get; set; }
        public double Sum { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
