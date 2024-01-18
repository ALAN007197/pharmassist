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
    public partial class AddDrPage : ContentPage
    {
        bool isDepartmentselected=false;
        DoctorsDetailModel doctorDetails = new DoctorsDetailModel();
        public AddDrPage(DoctorsDetailModel sample)
        {
            InitializeComponent();
            DrDepartMent.ItemsSource = new List<string> { "General medicine", "Cardiology", "General surgery", "Urology", "Orthopedics", "Neurology", "Pediatrics" };
            doctorDetails = sample;

            if(doctorDetails!= null)
            GetAllDetails();
        }

        private void GetAllDetails()
        {
            DrName.Text = doctorDetails.DrName;
            DrAddress.Text = doctorDetails.DrAddress;
            DrAge.Text = doctorDetails.DrAge;
            DrDepartMent.SelectedItem = doctorDetails.DrDepartment;
            DrMobile.Text = doctorDetails.DrMobile;
            DrFromTime.Time = doctorDetails.Fromdate.TimeOfDay;
            DrTotime.Time = doctorDetails.ToDate.TimeOfDay;
            DrQualification.Text = doctorDetails.DrQualification;
            Male.IsChecked = doctorDetails.DrGender == "Male" ? true : false;
            Female.IsChecked = doctorDetails.DrGender == "Female" ? true : false;
        }

        private void OnClickToCancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void OnClickToAddDr(object sender, EventArgs e)
        {
            if(doctorDetails!=null)
            {
                DateTime from = DateTime.Now.Date + DrFromTime.Time;
                DateTime to = DateTime.Now.Date + DrTotime.Time;
                DoctorsDetailModel updateDoctor = new DoctorsDetailModel
                {
                    ID = doctorDetails.ID,
                    DrName = DrName.Text,
                    DrAddress = DrAddress.Text,
                    DrAge = DrAge.Text,
                    DrDepartment = DrDepartMent.SelectedItem.ToString(),
                    DrMobile = DrMobile.Text,
                    DrQualification = DrQualification.Text,
                    Fromdate = from,
                    ToDate = to,
                    DrTime = from.ToString("hh:mm:ss tt") + " To " + to.ToString("hh:mm:ss tt"),
                    DrGender = Male.IsChecked ? "Male" : Female.IsChecked ? "Female" : "Other",
                };
                await App.DataBase.UpdateDoctor(updateDoctor);
                await DisplayAlert("Success", "Doctor Details Updated Successfully", "Ok");
                await Navigation.PopAsync();
            }
            else
            {
                try
                {
                    DateTime from = DateTime.Now.Date + DrFromTime.Time;
                    DateTime to = DateTime.Now.Date + DrTotime.Time;
                    DoctorsDetailModel newDoctor = new DoctorsDetailModel
                    {
                        DrName = DrName.Text,
                        DrAddress = DrAddress.Text,
                        DrAge = DrAge.Text,
                        DrDepartment = DrDepartMent.SelectedItem.ToString(),
                        DrMobile = DrMobile.Text,
                        DrQualification = DrQualification.Text,
                        Fromdate = from,
                        ToDate = to,
                        DrTime = from.ToString("hh:mm:ss tt") + " To " + to.ToString("hh:mm:ss tt"),
                        DrGender = Male.IsChecked ? "Male" : Female.IsChecked ? "Female" : "Other",
                    };
                    await App.DataBase.AddDoctor(newDoctor);
                    await DisplayAlert("Success", "Doctor Details Added Successfully", "Ok");
                    ClearData();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void ClearData()
        {
            DrName.Text = null;
            DrAddress.Text = null;
            DrAge.Text= null;
            DrDepartMent.SelectedItem = null;
            DrMobile.Text= null;
            DrQualification.Text= null;
            isDepartmentselected = false;
        }

        private void ValidateField(object sender, TextChangedEventArgs e)
        {
            validate();
        }
        private void validate()
        {
            DrSubmitButton.IsEnabled = !string.IsNullOrEmpty(DrName.Text) && !string.IsNullOrEmpty(DrAddress.Text) && isDepartmentselected &&
                                !string.IsNullOrEmpty(DrAge.Text) && !string.IsNullOrEmpty(DrMobile.Text) && !string.IsNullOrEmpty(DrQualification.Text);
        }

        private void Depatmentchanges(object sender, FocusEventArgs e)
        {
            isDepartmentselected = DrDepartMent.SelectedItem != null;
            validate();
        }
    }
}