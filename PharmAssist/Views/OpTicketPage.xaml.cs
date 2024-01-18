using PharmAssist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpTicketPage : ContentPage
    {
        public OpTicketPage()
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
                    OplistView.ItemsSource = await App.DataBase.GetOpTicketList();
                }
            }
            catch (Exception ex)
            {

            }
                    
        }

        private void OnclickToCreateOpTicket(object sender, EventArgs e)
        {
            CreateOPTicket page = new CreateOPTicket();
            Navigation.PushAsync(page);
        }

        private void OnClickToCreateDoctorDetails(object sender, EventArgs e)
        {
            DrDetailsPage page = new DrDetailsPage();
            Navigation.PushAsync(page);
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(OpSearch.Text))
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