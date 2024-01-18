using PharmAssist.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PharmAssist.Views
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