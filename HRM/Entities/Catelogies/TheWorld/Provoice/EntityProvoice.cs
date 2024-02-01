using Entities.Application.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.TheWorld.Provoice
{
    public class EntityProvoice : BaseModel
    {
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        [MaxLength(3)]
        public string IsOnly { get; set; } = string.Empty;

        /// <summary>
        /// Mã quốc gua
        /// </summary>
        [Display(Name = "Mã quốc gia")]
        public string CodeCountry { get; set; } = string.Empty;
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        [Display(Name = "Tên tỉnh thành")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
