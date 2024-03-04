using Entities.Application.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.TheWorld.Country
{
    public class EntityCountry : BaseModel
    {
        private string _IsOnly = string.Empty;
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get { return _IsOnly; } set { _IsOnly = value; OnPropertyChanged(); } }

        private string _Code = string.Empty;
        /// <summary>
        /// Mã quốc gia
        /// </summary>
        [Display(Name = "Mã quốc gia")]
        public string Code { get { return _Code; } set { _Code = value; OnPropertyChanged(); } }

        private string _NameEN = string.Empty;
        /// <summary>
        /// Tên Tiếng Anh
        /// </summary>
        [Display(Name = "Tên tiếng Anh")]
        public string NameEN { get { return _NameEN; } set { _NameEN = value; OnPropertyChanged(); } }

        private string _NameVI = string.Empty;
        /// <summary>
        /// Tên tiếng Việt
        /// </summary>
        [Display(Name = "Tên tiếng Việt")]
        public string NameVI { get { return _NameVI; } set { _NameVI = value; OnPropertyChanged(); } }
    }
}
