using PharmAssist.Database;
using PharmAssist.Services;
using PharmAssist.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PharmAssist
{
    public partial class App : Application
    {   
        public enum Employee
        {
            Doctor,
            Admin,
            pharmassist,
            Rereception,
        }
        public static Employee LoginEmployee { get; set; }

        private static PharmAssistDB dataBase;
        public static PharmAssistDB DataBase
        {
            get 
            { 
                if (dataBase == null)
                {
                    dataBase = new PharmAssistDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PharmAssistDB.db3"));
                }
                return dataBase; 
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
