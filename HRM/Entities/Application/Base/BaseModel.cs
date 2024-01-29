using System.ComponentModel.DataAnnotations;

namespace Entities.Application.Base
{
    public class BaseModel
    {

        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get; set; } = string.Empty;
        public bool IsCurently { get; set; } = true;
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; } = "AdminSSR";
        public DateTime UpdateOn { get; set; }
        public string UpdateBy { get; set; } = "AdminSSR";
    }
}
