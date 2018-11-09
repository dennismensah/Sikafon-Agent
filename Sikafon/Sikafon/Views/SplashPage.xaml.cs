using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sikafon.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashPage : ContentPage
	{
		public SplashPage ()
		{
			InitializeComponent();
		}
	    protected async override void OnAppearing()
	    {

	        await FadingSplashEffect();
	    }

	    private async Task FadingSplashEffect()
	    {
	        base.OnAppearing();
	        await Task.Delay(2000);

	        //await Task.WhenAll(
	        //    img.FadeTo(0,1000)
	        //    //,logo.ScaleTo(10, 2000)
	        //);
	        //splash.IsVisible = false;
	        loading.IsRunning = false;
	        //await Navigation.PushAsync(new LoginPage());
	        Application.Current.MainPage = (new NavigationPage(new LoginPage()));
	    }
    }
}