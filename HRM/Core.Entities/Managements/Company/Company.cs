using Core.Entities.Applications.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Entities.Managements.Company
{
    public class Company : BaseModel, ICompany
    {
        public string _CompanyId { get; set; }

        [DisplayName("Công Ty")]
        [Required(ErrorMessage = "{0} Không Được Để Trống !")]
        public string CompanyId { get { return _CompanyId; } set { _CompanyId = value; OnPropertyChanged(); } }


        public string _CompanyName { get; set; }
        public string CompanyName { get { return _CompanyName; } set { _CompanyName = value; OnPropertyChanged(); } }
    }
}
