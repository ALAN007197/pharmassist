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
    public partial class PharmacyPage : ContentPage
    {
        public List<MedicineModel> tempMedList = new List<MedicineModel>();
        public PharmacyPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (await App.DataBase.GetMedicineList() != null)
                {
                    tempMedList = await App.DataBase.GetMedicineList();
                    MedlistView.ItemsSource = tempMedList;
                }
            }
            catch (Exception ex)
            {

            }
           
        }
        private void OnclickToAddMedicines(object sender, EventArgs e)
        {
            AddMedicinePage page = new AddMedicinePage(null);
            Navigation.PushAsync(page);
        }

        private void OnClickToGetPetienstList(object sender, EventArgs e)
        {
            DrPrescriptionPage page = new DrPrescriptionPage(false);
            Navigation.PushAsync(page);
        }

        private void OnClickToSearch(object sender, EventArgs e)
        {
            if (tempMedList != null && tempMedList.Count > 0)
            {
                if (!string.IsNullOrEmpty(SearchBox.Text))
                {
                    MedlistView.ItemsSource = tempMedList.Where(x => x.Name == SearchBox.Text.Trim()).ToList();
                }
                else
                {
                    MedlistView.ItemsSource = tempMedList;
                }
            }
        }
        private void OnclickToExpiredMedicines(object sender, EventArgs e)
        {
            if (tempMedList != null && tempMedList.Count > 0)
            {
               MedlistView.ItemsSource = tempMedList.Where(x => x.EXpDate < DateTime.Now.Date).ToList();               
            }
        }

        private void OnClickToEdit(object sender, EventArgs e)
        {
            MedicineModel sample = new MedicineModel();
            sample = ((Button)sender).BindingContext as MedicineModel;
            AddMedicinePage page = new AddMedicinePage(sample);
            Navigation.PushAsync(page);
        }

        private async void OnClickToDelete(object sender, EventArgs e)
        {
            MedicineModel sample = new MedicineModel();
            sample = ((Button)sender).BindingContext as MedicineModel;
            var result = await DisplayAlert("Importent", "Are you sure you want to Detele " + sample.Name, "Yes", "No");
            if(result)
            {
                await App.DataBase.DeleteMedicine(sample);
                try
                {
                    if (await App.DataBase.GetMedicineList() != null)
                    {
                        tempMedList = await App.DataBase.GetMedicineList();
                        MedlistView.ItemsSource = tempMedList;
                    }
                    else
                    {
                        MedlistView.ItemsSource = null;
                    }
                }
                catch (Exception ex)
                {

                }
            }                     
        }
    }    
}