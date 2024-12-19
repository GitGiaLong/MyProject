namespace Core.Libraries.Models
{
    public class Filter : Paging, IFilter
    {
        private static string _Select = "*";
        public string Select { get { return _Select; } set { _Select = value; } }

        private static string _From = string.Empty;
        public string From { get { return _From; } set { _From = value; } }

        private static string? _Value = string.Empty;
        public string Value { get { return _Value; } set { _Value = value; } }
    }
}
