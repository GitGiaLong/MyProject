namespace Core.Entities.Applications.Base.Filters
{
    public interface IFilter
    {
        string Select { get; set; }
        string From { get; set; }
        string Value { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

    }
}
