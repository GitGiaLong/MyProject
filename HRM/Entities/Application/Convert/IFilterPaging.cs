namespace Entities.Application.Convert
{
    public interface IFilterPaging
    {
        public string? value { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
