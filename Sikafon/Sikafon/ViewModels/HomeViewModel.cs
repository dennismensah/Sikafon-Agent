using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Sikafon.Interfaces;
using Sikafon.Models;
using Sikafon.Services;
using Sikafon.Validation;
using Xamarin.Forms;
using Activation = Sikafon.Services.Activation;

namespace Sikafon.ViewModels
{
    public class HomeViewModel:BaseViewModel
    {
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }

        private bool _isrefreshing = false;
        public string ALabel { get; set; }
        public HomeViewModel()
        {
            Activations = new ObservableCollection<Models.Activation>();
            activations = new List<Models.Activation>();
            initItems();
            
        }

        public ObservableCollection<Models.Activation> Activations { get; set; }
        public List<Models.Activation> activations { get; set; }

        async void initItems()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    var activationService = new Activation();
                    Response response = await activationService.GetActivations();
                    activations = (JsonConvert.DeserializeObject<List<Models.Activation>>(response.Content)).OrderByDescending(x => x.Id).ToList();
                    foreach (var activation in activations)
                    {
                        Activations.Add(activation);
                    }

                    ALabel = $"My Activations ({activations.Count})";
                    OnPropertyChanged(nameof(ALabel));
                }
                catch (Exception e)
                {
                    UserDialogs.Instance.Toast("Could not connect to server", TimeSpan.FromSeconds(2000));
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Could not connect to server", TimeSpan.FromSeconds(2000));
            }
        }

        private async void refresh()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    isrefreshing = true;
                    var activationService = new Activation();
                    Response response = await activationService.GetActivations();
                    activations = (JsonConvert.DeserializeObject<List<Models.Activation>>(response.Content)).OrderByDescending(x => x.Id).ToList();
                    Activations.Clear();
                    foreach (var activation in activations)
                    {
                        Activations.Add(activation);
                    }
                    ALabel = $"My Activations ({activations.Count})";
                    OnPropertyChanged(nameof(ALabel));
                    isrefreshing = false;
                }
                catch (Exception e)
                {
                    UserDialogs.Instance.Toast("Could not connect to server", TimeSpan.FromSeconds(2));
                    isrefreshing = false;
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Could not connect to server", TimeSpan.FromSeconds(2));
                isrefreshing = false;
            }
        }

        private async void activate()
        {
            string qrstring = null;
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Activating Card", MaskType.None);
                    var model = new Models.Activation
                    {
                        CardNumber = PhoneNumber,
                        PhoneNumber = CardNumber,
                        AgentId = Application.Current.Properties.ContainsKey("Id") ? (int)Application.Current.Properties["Id"] : 0,
                        ActivationDate = DateTime.Now
                    };
                    var activationService = new Services.Activation();
                    Response response = await activationService.Activate(model);
                    if (!response.IsSuccessStatusCode)
                    {
                        await UserDialogs.Instance.AlertAsync("An error occured");
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        CardNumber = null;
                        PhoneNumber = null;
                        await UserDialogs.Instance.AlertAsync("Card activated successfully");
                        refresh();
                        UserDialogs.Instance.HideLoading();
                    }
                }
                catch (Exception ee)
                {
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

        private async void scan()
        {
            try
            {
                var scanner = DependencyService.Get<IQrCodeScanner>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    CardNumber = result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool isrefreshing
        {
            get { return _isrefreshing; }
            set
            {
                _isrefreshing = value;
                OnPropertyChanged(nameof(isrefreshing));
            }
        }

        private List<string> _gender =new List<string>{"Male","Female"};

        public ICommand RefreshCommand => new Command(refresh);
        public ICommand ActivateCommand => new Command(activate);
        public ICommand ScanCommand => new Command(scan);

    }
}
