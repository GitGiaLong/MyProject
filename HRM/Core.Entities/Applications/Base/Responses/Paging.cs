using Core.Entities.Extensions;

namespace Core.Entities.Applications.Base.Responses
{
    public class Paging : Onchanged
    {

        private static int _Page = 1;
        /// <summary>
        /// Trang
        /// </summary>
        public int Page { get { return _Page; } set { _Page = value; OnPropertyChanged(); } }

        private static int _PageSize = 10;
        /// <summary>
        /// Số dòng trong trang
        /// </summary>
        public int PageSize { get { return _PageSize; } set { _PageSize = value; OnPropertyChanged(); } }

        private static int _TotalPages = 0;
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get { return _TotalPages; } set { _TotalPages = value; OnPropertyChanged(); } }

        private int _TotalRecords { get; set; } = 0;
        /// <summary>
        /// Tổng số dòng dữ liệu
        /// </summary>
        public int TotalRecords { get { return _TotalRecords; } set { _TotalRecords = value; OnPropertyChanged(); } }

    }
}
