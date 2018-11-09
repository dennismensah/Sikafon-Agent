using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Sikafon.Annotations;
using Sikafon.Interfaces;
using Sikafon.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sikafon.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : TabbedPage
	{

        public HomePage ()
		{
			InitializeComponent ();
            listv.ItemSelected+= OnItemSelected;
        }

	    void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
	        var item = e.SelectedItem as Activation;

	        if (item != null)
	        {
	            listv.SelectedItem = null;
	        }
	    }


        private async void Activate_Clicked(object sender, EventArgs e)
        {
            string qrstring = null;
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    this.InputTransparent = true;
                    UserDialogs.Instance.ShowLoading("Activating Card", MaskType.None);
                    var model = new Models.Activation
                    {
                        CardNumber = cnumber.Text,
                        PhoneNumber = cphone.Text,
                        AgentId = Application.Current.Properties.ContainsKey("Id") ? (int)Application.Current.Properties["Id"] : 0,
                        ActivationDate = DateTime.Now
                    };
                    var activationService = new Services.Activation();
                    Response response = await activationService.Activate(model);
                    if (!response.IsSuccessStatusCode)
                    {
                        this.InputTransparent = false;
                        await UserDialogs.Instance.AlertAsync("An error occured");
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        this.InputTransparent = false;
                        cnumber.Text = null;
                        cphone.Text = null;
                        await UserDialogs.Instance.AlertAsync("Card activated successfully");
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

        private void MenuItem_OnClicked(object sender, EventArgs e)
	    {
	        Navigation.InsertPageBefore(new LoginPage(),this);
	        Navigation.PopAsync();
	    }

	    private async void BtnScan_OnClicked(object sender, EventArgs e)
	    {
	        try
            {
                var scanner = DependencyService.Get<IQrCodeScanner>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    cnumber.Text = result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}