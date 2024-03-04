using GSMF.Extensions.Onchanged;
using System.ComponentModel.DataAnnotations;

namespace Entities.Application.Base
{
    public class BaseModel : OnChanged, IBaseModel
    {
        public bool IsCurently { get; set; } = true;
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; } = "AdminSSR";
        public DateTime UpdateOn { get; set; }
        public string UpdateBy { get; set; } = "AdminSSR";
    }

}
