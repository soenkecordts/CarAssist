using CarAssist.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage
	{
		public MainPage ()
		{
			InitializeComponent ();

            this.Detail = _tripPage = new NavigationPage(new TripPage());

            masterPage.MenuList.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        NavigationPage _tripPage;
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as HamburgerMenuItem;

            if (e.SelectedItem != null)
            {
                Page page;
                bool isTripPage = item.TargetType == typeof(TripPage);

                if (isTripPage)    // cache TripPage when OBD recording
                {
                    Detail = _tripPage;
                }
                else
                {
                    page = (Page)Activator.CreateInstance(item.TargetType);
                    Detail = new NavigationPage(page);
                }
                masterPage.MenuList.SelectedItem = null;
                IsPresented = false;
            }
        }

        public void GoHome()
        {
            Detail = _tripPage;
        }
    }
}