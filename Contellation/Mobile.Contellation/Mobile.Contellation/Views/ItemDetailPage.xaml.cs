using Mobile.Contellation.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Mobile.Contellation.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}