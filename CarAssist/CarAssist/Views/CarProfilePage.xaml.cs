using Acr.UserDialogs;
using CarAssist.Connection;
using CarAssist.Obd;
using CarAssist.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarProfilePage : ContentPage
    {
        public CarProfilePage()
        {
            InitializeComponent();
            Refresh();
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage as MainPage).GoHome();
            return true;
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            using (var progress = UserDialogs.Instance.Loading("Verbinden...", maskType: MaskType.Clear))
            {
                try
                {
                    if (await Connect() == false)
                    {
                        await DisplayAlert("CarAssist", "Es konnte keine Verbindung aufgebaut werden. Überprüfen Sie unter Einstellungen die Bluetooth- oder WLAN-Verbindungsdaten", "Ok");
                        return;
                    }
                }
                catch (ObdException ex)
                {
                    await DisplayAlert("CarAssist", ex.Message, "Ok");
                    return;
                }
            }
        }

        async Task<bool> Connect()
        {
            bool result;

            if ((result = await TryConnectAsync(AppSettings.ConnectionStringBluetooth)) == false)
                result = await TryConnectAsync(AppSettings.ConnectionStringTcp);

            return result;
        }

        async Task<bool> TryConnectAsync(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return false;

            bool isOpened = false;

            IConnection con = null;
            ObdConnection obdConnection = null;
            IList<string> ecus = null;
                 
            try
            {
                con = ConnectionFactory.Create(connectionString);

                await con.ConnectAsync();

                if (con.IsConnected)
                {
                    obdConnection = new ObdConnection(con);
                    ecus = obdConnection.Open();
                    if (ecus != null)
                    {
                        isOpened = true;
                    }
                }
                if (con.IsConnected && isOpened)
                {
                    var list = new List<PID>();

                    foreach (var pidCode in Enum.GetValues(typeof(PIDCode)).Cast<PIDCode>())
                    {
                        if (obdConnection.Command.IsPidSupported(pidCode))
                            list.Add(PID.GetPID(pidCode));
                    }

                    AppSettings.Set("SupportedPids", String.Join(", ", list.Select((p) => p.PIDCode)));
                    AppSettings.Set("SupportedPidsCount", list.Count);
                    AppSettings.Set("OBDProtocolDescription", obdConnection.Command.OBDProtocolDescription);
                    AppSettings.Set("OBDVoltage", obdConnection.Command.OBDVoltage);
                    AppSettings.Set("VIN", obdConnection.Command.VIN);
                    AppSettings.Set("FuelType", obdConnection.Command.GetValue(PIDCode.FuelType) as string);
                    AppSettings.Set("ECUs", string.Join(", ", ecus));

                    Refresh();
                }
            }
            catch (ObdException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return false;
            }

            return con != null && con.IsConnected && isOpened;
        }

        private void Refresh()
        {
            vin.Text = AppSettings.Get<string>("VIN");
            voltage.Text = AppSettings.Get<string>("OBDVoltage");
            protocol.Text = AppSettings.Get<string>("OBDProtocolDescription");
            supportedPIDs.Text = AppSettings.Get<string>("SupportedPids");
            fuelType.Text = AppSettings.Get<string>("FuelType");
            ecus.Text = AppSettings.Get<string>("ECUs");
        }
    }
}