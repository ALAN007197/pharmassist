using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmAssist.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetButtonVisibility();
        }

        private void GetButtonVisibility()
        {
            switch (App.LoginEmployee)
            {
                case App.Employee.Admin:
                    ResepBtn.IsVisible = true;
                    DoctorBtn.IsVisible = true;
                    PharmaBtn.IsVisible = true;
                    DoctorDetailsBtn.IsVisible = true;
                    BillBtn.IsVisible = true;
                    break;

                case App.Employee.Rereception:
                    ResepBtn.IsVisible = true;
                    DoctorBtn.IsVisible = false;
                    PharmaBtn.IsVisible = false;
                    DoctorDetailsBtn.IsVisible = false;
                    BillBtn.IsVisible = true;
                    break;

                case App.Employee.Doctor:
                    ResepBtn.IsVisible = false;
                    DoctorBtn.IsVisible = true;
                    PharmaBtn.IsVisible = false;
                    DoctorDetailsBtn.IsVisible = false;
                    BillBtn.IsVisible = false;
                    break;

                case App.Employee.pharmassist:
                    ResepBtn.IsVisible = false;
                    DoctorBtn.IsVisible = false;
                    PharmaBtn.IsVisible = true;
                    DoctorDetailsBtn.IsVisible = false;
                    BillBtn.IsVisible = true;
                    break;
            }
        }

        private void PharmClicked(object sender, EventArgs e)
        {
            OpTicketPage page = new OpTicketPage();
            Navigation.PushAsync(page);
        }

        private void OnclickDoctor(object sender, EventArgs e)
        {
            DrPrescriptionPage page = new DrPrescriptionPage(true);
            Navigation.PushAsync(page); 
           
        }

        private void OnClickDoctorDetails(object sender, EventArgs e)
        {
            DrDetailsPage page = new DrDetailsPage();
            Navigation.PushAsync(page);
        }

        private void OnClickPharmacy(object sender, EventArgs e)
        {
            PharmacyPage page = new PharmacyPage();
            Navigation.PushAsync(page);
        }

        private void OnClickBillDetails(object sender, EventArgs e)
        {
            BillDetailsPage page = new BillDetailsPage();
            Navigation.PushAsync(page);
        }
    }
}