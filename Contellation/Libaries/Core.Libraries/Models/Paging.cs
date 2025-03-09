namespace Core.Libraries.Models
{
    public class Paging
    {
        private static int _Page = 1;
        public int Page { get { return _Page; } set { _Page = value; } }

        private static int _PageSize = 10;
        public int PageSize { get { return _PageSize; } set { _PageSize = value; } }

        private static int _TotalPages = 0;
        public int TotalPages { get { return _TotalPages; } set { _TotalPages = value; } }

        private int _TotalRecords { get; set; } = 0;
        public int TotalRecords { get { return _TotalRecords; } set { _TotalRecords = value; } }
    }
}
