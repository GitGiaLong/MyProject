using BLLVM.Application.Api;
using Entities.Catelogies.Countries;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace BLLVM.Catelogies.Countries
{
    public class MainCountryVM
    {
        private ObservableCollection<EntityCountry> _Countries = new ObservableCollection<EntityCountry>();
        public ObservableCollection<EntityCountry> DataCountries { get; set; }
        public EntityCountry DataCountry = new EntityCountry();

        ApiResponseResult Api = new ApiResponseResult();

        public MainCountryVM() { DataCountries = new ObservableCollection<EntityCountry>(); }

        public async Task Get()
        {
            DataCountries = await Api.AsyncActionApi<ObservableCollection<EntityCountry>>("/countries");
        }
    }
}
