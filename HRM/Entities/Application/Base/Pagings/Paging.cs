namespace Entities.Application.Base.Pagings
{
    public interface IPaging
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class Paging : IPaging 
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
