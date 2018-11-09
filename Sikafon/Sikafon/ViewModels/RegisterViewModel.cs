using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using Sikafon.Models;
using Sikafon.Services;
using Xamarin.Forms;

namespace Sikafon.ViewModels
{
    public class RegisterViewModel:BaseViewModel
    {

        private readonly INavigation _navigation;

        public RegisterViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        private List<string> _gender = new List<string> { "Male", "Female" };
        public List<string> Gender
        {
            get { return _gender; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string SGender { get; set; }
        public string Username { get; set; }

        public ICommand RegisterCommand => new Command(Register);

        private async void Register()
        {
            UserDialogs.Instance.ShowLoading("Registering Agent", MaskType.None);
            var model = new Agent
            {
                Username = Username,
                Password = Password,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                Gender = SGender
            };
            var regService = new Register();
            Response response = await regService.Add(model);
            if (!response.IsSuccessStatusCode)
            {
                await UserDialogs.Instance.AlertAsync("An error occured");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Agent registered successfully");
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
