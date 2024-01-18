using PharmAssist.Models;
using PharmAssist.Save;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrPrescriptionDetailsPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<MedicineModel> medList = new ObservableCollection<MedicineModel>();
        public OpMethod OpDetails = new OpMethod();
        public double Total;
        public DrPrescriptionDetailsPage(OpMethod opDetails)
        {
            InitializeComponent();
            OpDetails = opDetails;
            Prescription = new ObservableCollection<samplePriscriptionModel>();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetDetails(OpDetails);
            submitBtn.IsEnabled = false;
        }

        public class samplePriscriptionModel
        {
            private ObservableCollection<MedicineModel> medlist;
            private bool isAlertVisible = true;
            public ObservableCollection<MedicineModel> MedList
            {
                get => medlist;
                set
                {
                    if (medlist != value)
                    {
                        medlist = value;
                        SetProperty(ref medlist, value);
                    }
                }
            }
            public MedicineModel SelectedDedicine { get; set; }
            public bool IsAlertVisible
            {
                get => isAlertVisible;
                set
                {
                    if (isAlertVisible != value)
                    {
                        isAlertVisible = value;
                        SetProperty(ref isAlertVisible, value);
                    }
                }
            }
            public bool Morning { get; set; }
            public bool Noon { get; set; }
            public bool Evening { get; set; }
            public bool Befor { get; set; }
            public string NoDays { get; set; }
            public int Qty { get; set; }
            public double RecordTotal { get; set; }
            public double UnitPrice { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
            }

            protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(storage, value))
                {
                    return false;
                }
                storage = value;
                OnPropertyChanged(propertyName);

                return true;
            }
        }

        private async
        Task
        SetDetails(OpMethod opDetails)
        {
            PName.Text = opDetails.OpName;
            PGender.Text = opDetails.OpGender;
            PAge.Text = opDetails.OpAge;
            PTocken.Text = opDetails.OpticketId.ToString();
            if (opDetails.IsDoctorComplete)
            {
                await GetAllDetails();
                AddButton.IsVisible = false;
                PrintBtn.IsVisible = true;
            }
            else
            {
                GetOpDetails();
                PrintBtn.IsVisible = false;
            }

        }

        private async Task GetAllDetails()
        {
            Prescription = new ObservableCollection<samplePriscriptionModel>();
            DoctorsPriscriptionModel newList = new DoctorsPriscriptionModel();
            newList = await App.DataBase.GetDoctorsPriscription(OpDetails.ID);
            List<MedicineModel> templist = new List<MedicineModel>(await App.DataBase.GetMedicineList());
            medList = new ObservableCollection<MedicineModel>(templist);
            newList.PatientDetails = OpDetails;
            double total = 0;
            foreach (var item in newList.MedList)
            {
                Prescription.Add(new samplePriscriptionModel
                {
                    MedList = medList,
                    SelectedDedicine = item.SelectedMedicine,
                    Evening = item.Evening,
                    Morning = item.Morning,
                    Noon = item.Noon,
                    Befor = item.Befor,
                    IsAlertVisible = false,
                    NoDays = item.NoDays,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    RecordTotal = item.RecordTotal,                                  
                });
                total = total + item.RecordTotal;
            }
            TotalTxt.Text = total.ToString();
            totalColunt.IsVisible = true;
            OplistView.ItemsSource = Prescription;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        private ObservableCollection<samplePriscriptionModel> prescription;
        public ObservableCollection<samplePriscriptionModel> Prescription
        {
            get => prescription;
            set
            {
                if (prescription != value)
                {
                    prescription = value;
                    SetProperty(ref prescription, value);
                }
            }
        }


        private async void GetOpDetails()
        {
            List<MedicineModel> templist = new List<MedicineModel>(await App.DataBase.GetMedicineList());
            medList = new ObservableCollection<MedicineModel>(templist);
            Prescription.Add(new samplePriscriptionModel
            {
                MedList = medList,
                IsAlertVisible = true,
                Evening = false,
                Morning = false,
                Noon = false,
            });
            totalColunt.IsVisible = false;
            TotalRecordTextLabel.IsVisible = false;
            OplistView.ItemsSource = Prescription;
        }

        private void OnClicktoAdd(object sender, EventArgs e)
        {
            Prescription.Add(new samplePriscriptionModel
            {
                MedList = medList,
                IsAlertVisible = true,
                Evening = false,
                Morning = false,
                Noon = false,
            });
            Validate();
        }

        private void OnClicktoDelete(object sender, EventArgs e)
        {
            samplePriscriptionModel sample = new samplePriscriptionModel();
            sample = ((Button)sender).BindingContext as samplePriscriptionModel;
            Prescription.Remove(sample);  
            
            if(Prescription?.Count() ==0)
            {
                Prescription.Add(new samplePriscriptionModel
                {
                    MedList = medList,
                    IsAlertVisible = true,
                    Evening = false,
                    Morning = false,
                    Noon = false,
                });
            }
            Validate();
        }

        private async void OnclickToSubmit(object sender, EventArgs e)
        {
            DoctorsPriscriptionModel newList = new DoctorsPriscriptionModel();
            if (OpDetails.IsDoctorComplete)
            {
                OpDetails.IsPharmacyCompleted = true;
            }
            else
            {
                OpDetails.IsDoctorComplete = true;
                OpDetails.IsPharmacyCompleted = false;
            }

            newList.PatientDetails = OpDetails;

            newList.Total = 0;
            newList.MedList = new List<PriscriptionModel>();
            foreach (var Item in Prescription)
            {
                newList.MedList.Add(new PriscriptionModel
                {
                    SelectedMedicine = Item.SelectedDedicine,
                    Noon = Item.Noon,
                    Morning = Item.Morning,
                    Evening = Item.Evening,
                    Befor = Item.Befor,
                    UnitPrice = Item.UnitPrice,
                    NoDays = Item.NoDays,
                    Qty = Item.Qty,
                    RecordTotal = Item.RecordTotal,
                });
            }
            IsLoading.IsVisible = true;
            IsLoading.IsRunning = true;
            try
            {
                await App.DataBase.AddDoctorsPriscription(newList);
            }
            catch (Exception ex)
            {

            }

            IsLoading.IsVisible = false;
            IsLoading.IsRunning = false;
            await DisplayAlert("Success", "New Priscription Added Successfully ", "ok");
            await Navigation.PopAsync();
        }

        private void OnclicktoSelectionChange(object sender, EventArgs e)
        {
            samplePriscriptionModel sample = new samplePriscriptionModel();
            sample = ((Picker)sender).BindingContext as samplePriscriptionModel;

            foreach (var item in Prescription)
            {
                if (item == sample)
                {
                    if(sample.SelectedDedicine == null)
                    {
                        return;
                    }
                    if (sample?.SelectedDedicine?.Qty == 0)
                    {
                        item.IsAlertVisible = true;
                        DisplayAlert("Error", "Selected " + item.SelectedDedicine.Name + "Medicine out of stock please selcect Substitute Medicine", "ok");
                    }
                    else
                    {
                        item.IsAlertVisible = false;
                        Validate();
                        return;
                    }
                }
            }
            Validate();
            // Prescription.Add(sample2);
            //OplistView.ItemsSource = Prescription;
        }

        public void Validate()
        {
            bool isvalid = true;
            if (Prescription != null && Prescription.Count > 0)
            {
                foreach (var item in Prescription)
                {

                    isvalid = !string.IsNullOrEmpty(item.NoDays) && int.Parse(item.NoDays) > 0 && !item.IsAlertVisible && item.SelectedDedicine != null &&
                               (item.Noon || item.Evening || item.Morning) && isvalid;
                }
            }
            submitBtn.IsEnabled = isvalid;
        }

        private void OnTextChange(object sender, TextChangedEventArgs e)
        {
            samplePriscriptionModel sample = new samplePriscriptionModel();
            samplePriscriptionModel sample2 = new samplePriscriptionModel();
            sample = ((Entry)sender).BindingContext as samplePriscriptionModel;

            if (string.IsNullOrEmpty(sample.NoDays) || OpDetails.IsDoctorComplete)
            {
                if (OpDetails.IsDoctorComplete && OpDetails.IsPharmacyCompleted)
                {
                    submitBtn.IsVisible = false;
                }
                else if (OpDetails.IsDoctorComplete)
                {
                    submitBtn.IsEnabled = true;
                }
                else
                {
                    submitBtn.IsEnabled = false;
                }

                return;
            }

            foreach (var Item in Prescription)
            {
                if (Item.Morning && Item.Evening && Item.Noon)
                {
                    Item.Qty = 3 * int.Parse(Item.NoDays);
                }
                else if ((Item.Morning && Item.Evening) ||
                    (Item.Morning && Item.Noon) ||
                    (Item.Noon && Item.Evening))
                {
                    Item.Qty = 2 * int.Parse(Item.NoDays);
                }
                else if (Item.Morning || Item.Evening || Item.Noon)
                {
                    Item.Qty = int.Parse(Item.NoDays);
                }
                Item.RecordTotal = Item.Qty * Item.SelectedDedicine.Price;

                if (Item == sample && !string.IsNullOrEmpty(sample.NoDays))
                {

                    if (sample.SelectedDedicine.Qty < sample.Qty)
                    {
                        Item.IsAlertVisible = true;
                        DisplayAlert("Error", "Selected " + Item.SelectedDedicine.Name + "Medicine out of stock please selcect Substitute Medicine or reduce Days", "ok");
                    }
                    else
                    {
                        Item.IsAlertVisible = false;
                        Validate();
                        return;
                    }
                }
            }
            Validate();
        }

        private void OnCheckChanged(object sender, CheckedChangedEventArgs e)
        {
            Validate();
        }

        private void OnClicktoPrintBill(object sender, EventArgs e)
        {
            try
            {
                PrintMethod();
            }
            catch (Exception ex)
            {

            }
           
        }

        private void PrintMethod()
        {
            //Creates a new PDF document.
            PdfDocument document = new PdfDocument();
            //Adds page settings.
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            document.PageSettings.Margins.All = 50;
            //Adds a page to the document.
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            //Loads the image as stream.
           // Stream imageStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("PharmAssist.UWP.Assets.DP.jpg");
            RectangleF bounds = new RectangleF(75, 0, 390, 75);
            //PdfImage image = PdfImage.FromStream(imageStream);
            ////Draws the image to the PDF page.
            //page.Graphics.DrawImage(image, bounds);



            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            bounds = new RectangleF(0, bounds.Bottom + 50, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page.
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number.
            PdfTextElement element = new PdfTextElement("Bill No: " + OpDetails.ID.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page.
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            //Measures the width of the text to place it in the correct location.
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method.
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement("BILL TO ", timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 10));

            element = new PdfTextElement("Name :" + OpDetails.OpName, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 2));

            element = new PdfTextElement("Address :" + OpDetails.OpAddress, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 2)); 
            
            element = new PdfTextElement("Phone :" + OpDetails.OpMobileNo, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 2));

            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address.
            graphics.DrawLine(linePen, startPoint, endPoint);


            //Creates the datasource for the table.
            DataTable invoiceDetails = GetProductDetailsAsDataTable();
            //Creates a PDF grid.
            PdfGrid grid = new PdfGrid();
            //Adds the data source.
            grid.DataSource = invoiceDetails;
            //Creates the grid cell styles.
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style.
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

            //Adds cell customizations.
            for (int i = 0; i < header.Cells.Count; i++)
            {
               header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);             
            }

            //Applies the header style.
            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            //Creates the layout format for grid.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            //Creates layout format settings to allow the table pagination.
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 30), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            PdfFont Roman = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            element = new PdfTextElement("Total :" + Total, Roman);
            element.Brush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            result = element.Draw(page, new PointF(graphics.ClientSize.Width-60, gridResult.Bounds.Bottom +10));


            //Save the PDF document to stream.
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            //Close the document.
            document.Close(true);
            //Save the stream as a file in the device and invoke it for viewing
            Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("BillNo"+OpDetails.ID+".pdf", "application/pdf", stream);
        }

        private DataTable GetProductDetailsAsDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Med ID", typeof(double));
            dataTable.Columns.Add("Medicine", typeof(string));
            dataTable.Columns.Add("Quantity", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Total Price", typeof(string));
            Total = 0;
            foreach ( var item in Prescription)
            {
                Total = Total + item.RecordTotal;
                dataTable.Rows.Add(item.SelectedDedicine.ID, item.SelectedDedicine.Name,item.Qty,item.SelectedDedicine.Price,item.RecordTotal);
            }
            return dataTable;
        }
    }
}