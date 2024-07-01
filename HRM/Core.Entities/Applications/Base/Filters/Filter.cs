using Core.Entities.Applications.Base.Responses;

namespace Core.Entities.Applications.Base.Filters
{
    public class Filter : Paging, IFilter
    {
        private static string _Select  = "*";
        /// <summary>
        /// Select
        /// </summary>
        public string Select { get { return _Select; } set { _Select = value; OnPropertyChanged(); } }

        private static string _From  = string.Empty;
        /// <summary>
        /// From
        /// </summary>
        public string From { get { return _From; } set { _From = value; OnPropertyChanged(); } }

        private static string? _Value = string.Empty;
        /// <summary>
        /// Where ...
        /// </summary>
        public string Value { get { return _Value; } set { _Value = value; OnPropertyChanged(); } }
    }
}
