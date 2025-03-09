namespace Core.Libraries.Models
{
    public interface IFilter
    {
        /// <summary>
        /// Select
        /// </summary>
        string Select { get; set; }

        /// <summary>
        /// From
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// Where ...
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Trang
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Số dòng trong trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Tổng số dòng dữ liệu
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
