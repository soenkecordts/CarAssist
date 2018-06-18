
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();
		}
        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage as MainPage).GoHome();
            return true;
        }

    }
}