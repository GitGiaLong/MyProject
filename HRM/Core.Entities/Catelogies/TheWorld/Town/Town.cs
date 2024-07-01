using Core.Entities.Applications.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Catelogies.TheWorld.Town
{
    public class Town : BaseModel
    {
        private string _IsOnly = string.Empty;
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get { return _IsOnly; } set { _IsOnly = value; OnPropertyChanged(); } }

        private string _CodeDistrist = string.Empty;
        /// <summary>
        /// Mã Quận huyện
        /// </summary>
        [Display(Name = "Mã Quận huyện")]
        public string CodeDistrist { get { return _CodeDistrist; } set { _CodeDistrist = value; OnPropertyChanged(); } }

        private string _DisplayName = string.Empty;
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        [Display(Name = "Tên tỉnh thành")]
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }

    }
}
