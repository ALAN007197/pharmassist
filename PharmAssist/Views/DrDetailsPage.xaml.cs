using PharmAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrDetailsPage : ContentPage
    {
        public List<DoctorsDetailModel> tempList = new List<DoctorsDetailModel>();
        public DrDetailsPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            AddDoctor.IsVisible = App.LoginEmployee == App.Employee.Admin;
            try
            {
                if (await App.DataBase.GetDoctorstList() != null)
                {
                    DrlistView.ItemsSource = await App.DataBase.GetDoctorstList();
                }
            }
            catch (Exception ex)
            {

            }          

        }

        private void OnclickToAddDoctor(object sender, EventArgs e)
        {
            AddDrPage page = new AddDrPage(null);
            Navigation.PushAsync(page);
        }

        private async void OnClickToSearch(object sender, EventArgs e)
        {
            tempList = await App.DataBase.GetDoctorstList();
            if(tempList != null && tempList.Count>0)
            {
                if (!string.IsNullOrEmpty(SearchBox.Text))
                {
                    DrlistView.ItemsSource = tempList.Where(x => x.DrName == SearchBox.Text.Trim()).ToList();
                }
                else
                {
                    DrlistView.ItemsSource = await App.DataBase.GetDoctorstList();
                }
            }
               
        }

        private void OnClickToEdit(object sender, EventArgs e)
        {
            DoctorsDetailModel sample = new DoctorsDetailModel();
            sample = ((Button)sender).BindingContext as DoctorsDetailModel;
            AddDrPage page = new AddDrPage(sample);
            Navigation.PushAsync(page);
        }

        private async void OnClickToDelete(object sender, EventArgs e)
        {
            DoctorsDetailModel sample = new DoctorsDetailModel();
            sample = ((Button)sender).BindingContext as DoctorsDetailModel;
            var result = await DisplayAlert("Importent", "Are you sure you want to Detele " + sample.DrName, "Yes", "No");
            if (result)
            {
                await App.DataBase.DeleteDoctor(sample);
                try
                {
                    if (await App.DataBase.GetDoctorstList() != null)
                    {
                        tempList = await App.DataBase.GetDoctorstList();
                        DrlistView.ItemsSource = tempList;
                    }
                    else
                    {
                        DrlistView.ItemsSource = null;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}