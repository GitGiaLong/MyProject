using BLLVM.Application.Api;
using Entities.Application.Connect.Api;
using Entities.Catelogies.TheWorld;
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

        public async Task Get(EnumTheWorld enumTheWorld = EnumTheWorld.Country)
        {
            DataCountries = await Api.AsyncActionApi<ObservableCollection<EntityCountry>>($"/theWorld?EnumTheWorld={enumTheWorld}");
        }

        public async Task Action(EnumMethodApi TypeActionApi, object? Data = null)
        {
           var data = await Api.AsyncActionApi<ObservableCollection<EntityCountry>>($"/theWorld?type={EnumTheWorld.Country}", TypeActionApi, Data);
        }

        public int buttonAction = 0;

    }
}
