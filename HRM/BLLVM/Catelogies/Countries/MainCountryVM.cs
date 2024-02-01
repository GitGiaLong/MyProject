using BLLVM.Application.Api;
using Entities.Catelogies.TheWorld.Country;
using GSMF.Extensions;
using System.Collections.ObjectModel;

namespace BLLVM.Catelogies.Countries
{
    public class MainCountryVM
    {
        private ObservableCollection<EntityCountry> _Countries = new ObservableCollection<EntityCountry>();
        public ObservableCollection<EntityCountry> DataCountries { get; set; }
        public EntityCountry DataCountry = new EntityCountry();
        ConvertClass convertClass = new ConvertClass(); 

        ApiResponseResult Api = new ApiResponseResult();

        public MainCountryVM() { DataCountries = new ObservableCollection<EntityCountry>(); }

        public async Task Get()
        {
            DataCountries = await Api.AsyncActionApi<ObservableCollection<EntityCountry>>("/theWorld?EnumTheWorld=0");
        }
    }
}
