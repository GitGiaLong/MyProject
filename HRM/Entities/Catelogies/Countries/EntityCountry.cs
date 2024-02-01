using Entities.Application.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.Countries
{
    public class EntityCountry : BaseModel
    {
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get; set; } = string.Empty;

        /// <summary>
        /// Mã quốc gia (3 ký tự)
        /// </summary>
        [Display(Name = "Mã quốc gia (3 ký tự)")]
        [MaxLength(3)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Tên Tiếng Anh
        /// </summary>
        [Display(Name = "Tên tiếng Anh")]
        public string NameEN { get; set; } = string.Empty;

        /// <summary>
        /// Tên tiếng Việt
        /// </summary>
        [Display(Name = "Tên tiếng Việt")]
        public string NameVI { get; set; } = string.Empty;
    }
}
