namespace Entities.Application.Base
{
    public class BaseModel
    {
        public bool IsCurently { get; set; } = true;
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; } = "AdminSSR";
        public DateTime UpdateOn { get; set; }
        public string UpdateBy { get; set; } = "AdminSSR";
    }
}
