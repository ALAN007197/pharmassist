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
    public partial class CreateOPTicket : ContentPage
    {
        public List<OpMethod> tempOpList =new List<OpMethod>();
        public List<string> DepartmentList;
        bool PickerSelected = false;
        public CreateOPTicket()
        {
            InitializeComponent();
            OpDepartMent.ItemsSource  = new List<string>{ "General medicine","Cardiology", "General surgery", "Urology", "Orthopedics", "Neurology", "Pediatrics" };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {

                await GetOpId();
                await GetDoctors();
            }
            catch (Exception ex)
            {
                PickerSelected = true;
            }
        }

        private async Task GetDoctors()
        {
            List<DoctorsDetailModel> doctorList = new List<DoctorsDetailModel>();
            doctorList = await App.DataBase.GetDoctorstList();
            if (doctorList!=null && doctorList.Count>0)
            {
                OpDoctor.ItemsSource = doctorList;
                //OpDoctor.ItemDisplayBinding = new Binding("DrName");
            }
        }

        private async Task  GetOpId()
        {
            tempOpList = await App.DataBase.GetOpTicketList();
            if (tempOpList != null && tempOpList.Count() > 0)
            {
                OpId.Text = (tempOpList[tempOpList.Count() - 1].OpticketId+1).ToString();
            }
            else
            {
                OpId.Text = "1";
            }
        }

        private void OnClickCancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void OnClickToCreateOpTicket(object sender, EventArgs e)
        {
            GetOpDetails();
        }

        private async void GetOpDetails()
        {
            OpMethod OpDetails = new OpMethod
            {
                OpName = OpName.Text,
                OpAddress = OpAddress.Text,
                OpAge = OpAge.Text,
                OpDoctor = OpDoctor.SelectedItem.ToString(),
                OpDepartMent = OpDepartMent.SelectedItem.ToString(),
                OpGender = Male.IsChecked ? "Male" : Female.IsChecked ? "Female" : "Other",
                OpMobileNo = OpMobile.Text,
                OpticketId = int.Parse(OpId.Text),
                CreatedDate = DateTime.Now.Date,
            };
            await App.DataBase.AddOpTicket(OpDetails);
            await DisplayAlert("Sucess", "OP Ticket Created Successfully", "Ok");
            await ClearOldDataRedyForNext();
        }

        private async Task  ClearOldDataRedyForNext()
        {
            OpName.Text = null;
            OpAddress.Text = null;
            OpAge.Text = null;
            OpDoctor.SelectedItem = null;
            OpDepartMent.SelectedItem = null;
            Male.IsChecked = true;
            OpMobile.Text=null;
            PickerSelected = false;
            await GetOpId();
        }

        private void OnTextChange(object sender, TextChangedEventArgs e)
        {
            ValidateFields();
        }

        private void ValidateFields()
        {
            try {
                OpSubmit.IsEnabled = !string.IsNullOrEmpty(OpName.Text) && !string.IsNullOrEmpty(OpAddress.Text) &&
                                !string.IsNullOrEmpty(OpAge.Text) && !string.IsNullOrEmpty(OpMobile.Text);
            }
            catch (Exception ex)
            {

            }
            
        }

        private void SelectionChange(object sender, FocusEventArgs e)
        {
            PickerSelected = (OpDoctor.SelectedItem != null && OpDepartMent.SelectedItem != null);
            ValidateFields();
        }
    }
}