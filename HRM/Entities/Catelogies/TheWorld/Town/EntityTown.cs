using Entities.Application.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.TheWorld.Town
{
    public class EntityTown : BaseModel
    {
        /// <summary>
        /// Mã quốc gua
        /// </summary>
        [Display(Name = "Mã Quận huyện")]
        public string CodeDistrist { get; set; } = string.Empty;
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        [Display(Name = "Tên tỉnh thành")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
