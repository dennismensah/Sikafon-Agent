using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Sikafon.Models;
using Sikafon.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sikafon.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register : ContentPage
	{
		public Register ()
		{
			InitializeComponent ();
		    BindingContext = new RegisterViewModel(Navigation);
		    gender.SelectedIndex = 0;
		}

	    private async void Button_OnClicked(object sender, EventArgs e)
	    {
	        if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    this.InputTransparent = true;
                    UserDialogs.Instance.ShowLoading("Registering Agent", MaskType.None);
                    var model = new Agent
                    {
                        Username = username.Text,
                        Password = password.Text,
                        Email = email.Text,
                        FirstName = firstname.Text,
                        LastName = lastname.Text,
                        DateOfBirth = dob.Date,
                        Gender = gender.SelectedItem.ToString()
                    };
                    var regService = new Services.Register();
                    Response response = await regService.Add(model);
                    if (!response.IsSuccessStatusCode)
                    {
                        this.InputTransparent = false;
                        if (response.StatusCode == 400)
                        {
                            await UserDialogs.Instance.AlertAsync("Username already taken");
                            UserDialogs.Instance.HideLoading();
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync("An error occured");
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                    else
                    {
                        this.InputTransparent = false;
                        username.Text = "";
                        password.Text = "";
                        firstname.Text = "";
                        lastname.Text = "";
                        email.Text = "";
                        await UserDialogs.Instance.AlertAsync("You have been registered successfully. Please login with your credentials");
                        UserDialogs.Instance.HideLoading();
                    }

                }
                catch (Exception ee)
                {
                    this.InputTransparent = false;
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("There was an error executing your request");
                }
            }
	        else
	        {
	            UserDialogs.Instance.HideLoading();
	            await UserDialogs.Instance.AlertAsync("You are not connected to the internet");
            }
        }
	}
}