namespace Entities.Application.Base
{
    public class Paging
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? TotalPages { get; set; }
        public int? TotalRecords { get; set; }
    }
}
