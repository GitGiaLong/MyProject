namespace Entities.Application.Base
{
    internal interface IBaseModel
    {
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public string UpdateBy { get; set; }
    }
}
