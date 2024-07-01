namespace Core.Entities.Applications.Base
{
    public interface IBaseModel
    {
        bool IsCurently { get; set; }
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public string UpdateBy { get; set; }
    }
}
