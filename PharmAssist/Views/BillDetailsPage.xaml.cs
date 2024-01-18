using PharmAssist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BillDetailsPage : ContentPage
    {
        public BillDetailsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (await App.DataBase.GetOpTicketList() != null)
                {
                    List<OpMethod> tempList = new List<OpMethod>();
                    tempList = await App.DataBase.GetOpTicketList();
                    OplistView.ItemsSource = tempList.Where(x => x.IsPharmacyCompleted).ToList();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnClickProceed(object sender, EventArgs e)
        {
            OpMethod opDetails = new OpMethod();
            opDetails = ((Button)sender).BindingContext as OpMethod;
            DrPrescriptionDetailsPage page = new DrPrescriptionDetailsPage(opDetails);
            Navigation.PushAsync(page);
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(OpSearch.Text))
            {
                List<OpMethod> tempList = new List<OpMethod>();
                tempList = await App.DataBase.GetOpTicketList();
                OplistView.ItemsSource = tempList.Where(t => t.OpName == OpSearch.Text || t.OpticketId.ToString() == OpSearch.Text).ToList();
            }
            else
            {
                OplistView.ItemsSource = await App.DataBase.GetOpTicketList();
            }
        }
    }
}