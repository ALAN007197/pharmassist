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
    public partial class DrPrescriptionPage : ContentPage
    {
        public bool IsDoctor;
        public DrPrescriptionPage(bool isDoctor)
        {
            IsDoctor = isDoctor;
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
                    if(IsDoctor)
                    {
                        OplistView.ItemsSource = tempList.Where(x => !x.IsDoctorComplete).ToList();
                    }
                    else
                    {
                        OplistView.ItemsSource = tempList.Where(x => x.IsDoctorComplete && !x.IsPharmacyCompleted).ToList();
                    }
                   
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
    }
}