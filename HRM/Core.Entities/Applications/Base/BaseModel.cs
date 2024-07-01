using Core.Entities.Extensions;

namespace Core.Entities.Applications.Base
{
    public class BaseModel : Onchanged, IBaseModel
    {
        private bool _IsCurently = true;
        /// <summary>
        /// IsCurently
        /// </summary>
        public bool IsCurently { get { return _IsCurently; } set { _IsCurently = value; OnPropertyChanged(); } }

        private static DateTime _CreateOn = DateTime.Now;
        /// <summary>
        /// Ngày tạo đầu tiên
        /// </summary>
        public DateTime CreateOn { get { return _CreateOn; } set { _CreateOn = value; OnPropertyChanged(); } }

        private string _CreateBy = "AdminSSR";
        /// <summary>
        /// Người tạo đầu tiên
        /// </summary>
        public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; OnPropertyChanged(); } }

        public static DateTime _UpdateOn = DateTime.Now;
        /// <summary>
        /// Ngày cập nhập
        /// </summary>
        public DateTime UpdateOn { get { return _UpdateOn; } set { _UpdateOn = value; OnPropertyChanged(); } }

        private string _UpdateBy = "AdminSSR";
        /// <summary>
        /// Người đã cập nhập
        /// </summary>
        public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; OnPropertyChanged(); } }
    }
}
