using BLLVM.Catelogies.Countries;
using GUIVWPF.Common;
using System.Windows.Input;

namespace GUIVWPF.ViewModels 
{
    public class TheWorld : BaseViewModel
    {
        public MainCountryVM MainCountryVM { get; set; }
        public TheWorld() 
        {
            MainCountryVM = new MainCountryVM();
            MainCountryVM.Get().GetAwaiter();

            FistPage = new RelayCommand<object>((p) =>
            {
                if (MainCountryVM.DataCountries.Data != null)
                {
                    return true;
                }
                return false;
            },
            (p) => {
                MainCountryVM.Get(MainCountryVM.DataCountries.Page = 1).GetAwaiter();
            });

            BackPage = new RelayCommand<object>((p) =>
            {
                if (MainCountryVM.DataCountries.Data != null)
                {
                    return true;
                }
                return false;
            },
            (p) => {
                MainCountryVM.Get(MainCountryVM.DataCountries.Page -= 1).GetAwaiter();
            });

            NextPage = new RelayCommand<object>((p) => 
            {
                if (MainCountryVM.DataCountries.Data != null)
                {
                    return true;
                }
                return false;
            }, 
            (p) =>{
                MainCountryVM.Get(MainCountryVM.DataCountries.Page += 1).GetAwaiter();
            });

            LastPage = new RelayCommand<object>((p) =>
            {
                if (MainCountryVM.DataCountries.Data != null)
                {
                    return true;
                }
                return false;
            },
            (p) => {
                MainCountryVM.Get(MainCountryVM.DataCountries.Page = MainCountryVM.DataCountries.TotalPages).GetAwaiter();
            });
        }
        public ICommand FistPage { get; set; }
        public ICommand BackPage { get; set; }
        public ICommand NextPage { get; set; }
        public ICommand LastPage { get; set; }

    }
}
