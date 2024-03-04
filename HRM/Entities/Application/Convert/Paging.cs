using Entities.Application.Base;
using GSMF.Extensions.Onchanged;

namespace Entities.Application.Convert
{
    public class Paging : OnChanged, IPaging
    {
        public int _Page = 1;
        public int Page { get { return _Page; } set { _Page = value; OnPropertyChanged(); } }
        public int _PageSize = 10;
        public int PageSize { get { return _PageSize; } set { _PageSize = value; OnPropertyChanged(); } }
    }
}
