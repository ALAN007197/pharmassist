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
    public partial class AddMedicinePage : ContentPage
    {
        public bool isPickerSelected = false;
        public List<MedicineModel> tempMedList = new List<MedicineModel>();
        public MedicineModel MedDetailsEdit = new MedicineModel();
        public AddMedicinePage(MedicineModel medicineDetails)
        {
            InitializeComponent();
            MedDetailsEdit = medicineDetails;
        }

        private void OnClickToCancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(MedDetailsEdit != null)
            {
                SetMedDetails();
            }          
        }

        private void SetMedDetails()
        {
            MedName.Text = MedDetailsEdit.Name;
            MedQty.Text = MedDetailsEdit.Qty.ToString();
            MedPrice.Text = MedDetailsEdit.Price.ToString();
            ExpDate.Date = MedDetailsEdit.EXpDate;
        }

        private async void OnClickToSubmit(object sender, EventArgs e)
        {
            if(MedDetailsEdit != null)
            {
                MedicineModel med = new MedicineModel
                {
                    ID = MedDetailsEdit.ID,
                    Name = MedName.Text,
                    Qty = int.Parse(MedQty.Text),
                    Price = int.Parse(MedPrice.Text),
                    EXpDate = ExpDate.Date,
                    Description = MedDetailsEdit.Description,
                };
                await App.DataBase.UpdateMedicine(med);
                await DisplayAlert("Success", MedName.Text + " Successfully Updated", "Ok");
                await Navigation.PopAsync();
            }
            else
            {
                MedicineModel med = new MedicineModel
                {
                    Name = MedName.Text,
                    Qty = int.Parse(MedQty.Text),
                    Price = int.Parse(MedPrice.Text),
                    EXpDate = ExpDate.Date
                };
                await App.DataBase.AddMedicine(med);
                await DisplayAlert("Success", MedName.Text + " Successfully Added", "Ok");
                ClearFields();
            }          
        }

        private void ClearFields()
        {
            isPickerSelected = false;
            MedName.Text = null;
            MedQty.Text = null;
            MedPrice.Text = null;
            ExpDate.Date = DateTime.Now.Date;
        }

        private void OnTextChange(object sender, TextChangedEventArgs e)
        {
            SubmitButton.IsEnabled = !string.IsNullOrEmpty(MedPrice.Text) && !string.IsNullOrEmpty(MedQty.Text) && !string.IsNullOrEmpty(MedName.Text);
        }

        
    }
}