namespace Entities.Application.Convert
{
    public class Filter : Paging, IFilterPaging
    {
        public string? value { get; set; } = null;
    }
}
