using BLLVM.Application.Api;
using Entities.Application.Connect.Api;
using Entities.Application.Convert;
using Entities.Catelogies.TheWorld;
using Entities.Catelogies.TheWorld.Country;
using GSMF.Extensions;
using GSMF.Extensions.Onchanged;
using System.Collections.ObjectModel;

namespace BLLVM.Catelogies.Countries
{
    public class MainCountryVM : OnChanged
    {
        public ApiRestFul<ObservableCollection<EntityCountry>> _DataCountries = new ApiRestFul<ObservableCollection<EntityCountry>>();
        public ApiRestFul<ObservableCollection<EntityCountry>> DataCountries { get { return _DataCountries; } set { _DataCountries = value; OnPropertyChanged(); } }
        public EntityCountry DataCountry = new EntityCountry();
        ConvertClass convertClass = new ConvertClass();

        ApiResponseResult Api = new ApiResponseResult();

        public MainCountryVM()
        {
            DataCountries = new ApiRestFul<ObservableCollection<EntityCountry>>();
            //Get().GetAwaiter();
        }
        public async Task Get(int page = 1, int pageSize = 10, EnumTheWorld enumTheWorld = EnumTheWorld.Country)
        {
            try
            {
                DataCountries = await Api.AsyncActionApi<ObservableCollection<EntityCountry>>($"/theWorld?Page={page}&PageSize={pageSize}&typeTheWorld={enumTheWorld}");
            }
            catch (Exception Ex)
            {
                DataCountries.Message = Ex.Message;
            }
        }
        public async Task Action(EnumMethodApi TypeActionApi, object? Data = null)
        {
            var data = await Api.AsyncActionApi<ObservableCollection<EntityCountry>>($"/theWorld?type={EnumTheWorld.Country}", TypeActionApi, Data);

        }

        public int buttonAction = 0;

    }
}
