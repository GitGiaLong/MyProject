using BLLVM.Catelogies.Countries;

namespace GUIVWPF.ViewModels
{
    public class TheWorld 
    {
        public MainCountryVM MainCountryVM { get; set; }
        public TheWorld() 
        {
            MainCountryVM = new MainCountryVM();
            MainCountryVM.Get().GetAwaiter();
        }
    }
}
