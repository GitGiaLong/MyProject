using Entities.Catelogies.TheWorld.Country;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.TheWorld
{
    public class EntityTheWorld 
    {
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get; set; } = string.Empty;
    }
}
