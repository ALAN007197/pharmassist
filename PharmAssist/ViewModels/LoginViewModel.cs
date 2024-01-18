using PharmAssist.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PharmAssist.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        string _username;
        string _password;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            if(Username == "d123" && Password == "123")
            {
                App.LoginEmployee = App.Employee.Doctor;
                App.Current.MainPage = new AppShell();              
            }
            else if(Username == "p123" && Password == "123")
            {
                App.LoginEmployee = App.Employee.pharmassist;
                App.Current.MainPage = new AppShell();            
            }
            else if(Username == "a123" && Password == "123")
            {
                App.LoginEmployee = App.Employee.Admin;
                App.Current.MainPage = new AppShell();              
            } 
            else if(Username == "r123" && Password == "123")
            {
                App.LoginEmployee = App.Employee.Rereception;
                App.Current.MainPage = new AppShell();              
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Please enter valid Username or Password","Ok");
            }
        }
                   
    }
}
