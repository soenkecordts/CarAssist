using CarAssist.Connection;
using CarAssist.Utils;
using CarAssist.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage as MainPage).GoHome();
            return true;
        }

        void OnSelect(object sender, EventArgs e)
        {
            if ((sender as Cell).StyleId == "dialogBluetooth")
            {
                dialogBluetooth.IsVisible = true;
            }
            else if ((sender as Cell).StyleId == "dialogTcp")
            {
                dialogTcp.IsVisible = true;
            }
            else if ((sender as Cell).StyleId == "dialogSearch")
            {
                dialogSearch.IsVisible = true;
            }
            else if ((sender as Cell).StyleId == "dialogOwncloud")
            {
                dialogOwncloud.IsVisible = true;
            }
        }

        void OnSave(object sender, EventArgs args)
        {
            var parent = (sender as View).Parent;
            var grandparent = (parent.Parent.Parent as ContentView);

            (grandparent as View).IsVisible = false;

            if (grandparent.StyleId == "dialogBluetooth")
            {
                var item = bluetoothDevices.SelectedItem as BluetoothInfo;
                AppSettings.Set("BluetoothAddress", item?.Address);
                AppSettings.Set("BluetoothName", item?.Name);
            }
            else if (grandparent.StyleId == "dialogTcp")
            {
                AppSettings.Set("TcpAddress", ipAddress.Text);
                AppSettings.Set("TcpPort", port.Text);
            }
            else if (grandparent.StyleId == "dialogOwncloud")
            {
                AppSettings.Set("OwncloudUser", user.Text);
                AppSettings.Set("OwncloudPassword", password.Text);
                AppSettings.Set("OwncloudUrl", urlOwncloud.Text);
            }
        }

        void OnCancel(object sender, EventArgs args)
        {
            var parent = (sender as View).Parent;
            var grandparent = (parent.Parent.Parent as ContentView);

            (grandparent as View).IsVisible = false;
        }
    }
}