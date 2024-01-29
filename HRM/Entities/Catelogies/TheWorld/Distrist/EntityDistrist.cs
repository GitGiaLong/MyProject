using Entities.Application.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.TheWorld.Distrist
{
    public class EntityDistrist : BaseModel
    {
        /// <summary>
        /// Mã quốc tỉnh
        /// </summary>
        [Display(Name = "Mã Tỉnh")]
        public string CodeProvoice { get; set; } = string.Empty;
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        [Display(Name = "Tên tỉnh thành")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
