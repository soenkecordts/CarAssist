using Acr.UserDialogs;
using CarAssist.Connection;
using CarAssist.IO;
using CarAssist.Obd;
using CarAssist.Utils;
using CarAssist.ViewModels;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripPage : TabbedPage
    {
        TripViewModel TripViewModel;

        ObdConnection _obdConnection;
        IProgressDialog _progress;
        ILogger _logger;
        string _file;

        public TripPage()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            BindingContext = TripViewModel = new TripViewModel();

            if (CrossGeolocator.IsSupported &&
                CrossGeolocator.Current.IsGeolocationAvailable &&
                CrossGeolocator.Current.IsGeolocationEnabled)
            {
                CrossGeolocator.Current.DesiredAccuracy = 500;
                if (!CrossGeolocator.Current.IsListening)
                {
                    CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromMilliseconds(500), 10, true);
                }
            }
        }

        async void OnRecord(object sender, EventArgs e)
        {
            using (_progress = UserDialogs.Instance.Loading("Verbinden...", maskType: MaskType.Clear))
            {
                try
                {
                    var now = DateTime.Now;
                    var name = $"Trip_{now.Year}{now.Month:00}{now.Day:00}_{now.Hour:00}.{now.Minute:00}.{now.Second:00}.{now.Millisecond:0000}";
                    _logger = new LoggerFile(Path.Combine(Folder.Personal, $"{name}.log"))
                    {
                        Overwrite = false
                    };

                    if (await Connect() == false)
                    {
                        await DisplayAlert("CarAssist", "Es konnte keine Verbindung aufgebaut werden. Überprüfen Sie unter Einstellungen die Bluetooth- oder WLAN-Verbindungsdaten", "Ok");
                        return;
                    }
                    TripViewModel.IsRecording = true;
                    Start();
                }
                catch (ObdException ex)
                {
                    await DisplayAlert("CarAssist", ex.Message, "Ok");
                    return;
                }
            }
        }

        void OnStopRecord(object sender, EventArgs e)
        {
            using (_progress = UserDialogs.Instance.Loading("Verbindung beenden...", maskType: MaskType.Clear))
            {
                TripViewModel.IsRecording = false;
                Stop();
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

            try
            {
                con = ConnectionFactory.Create(connectionString);
                con.Logger = _logger;

                await con.ConnectAsync();

                if (con.IsConnected)
                {
                    _obdConnection = new ObdConnection(con);
                    var ecus = _obdConnection.Open();
                    if (ecus != null)
                    {
                        TripViewModel.ECUs.AddRange(ecus);
                        isOpened = true;
                        _obdConnection.Command.Logger = _logger;
                    }
                }
                if (con.IsConnected && isOpened)
                    DetectCarProperties();
            }
            catch (ObdException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.WriteLine(ex.Message);
                return false;
            }

            return con != null && con.IsConnected && isOpened;
        }

        void DetectCarProperties()
        {
            var list = new List<PID>();

            foreach (var pidCode in Enum.GetValues(typeof(PIDCode)).Cast<PIDCode>())
            {
                if (_obdConnection.Command.IsPidSupported(pidCode))
                    list.Add(PID.GetPID(pidCode));
            }
            TripViewModel.SupportedPids = String.Join(", ", list.Select((p) => p.PIDCode));
            TripViewModel.SupportedPidsCount = list.Count;
            TripViewModel.OBDProtocolDescription = _obdConnection.Command.OBDProtocolDescription;
            TripViewModel.OBDVoltage = _obdConnection.Command.OBDVoltage;
            TripViewModel.VIN = _obdConnection.Command.VIN;
            TripViewModel.FuelType = _obdConnection.Command.GetValue(PIDCode.FuelType) as string;

            AppSettings.Set("SupportedPids", TripViewModel.SupportedPids);
            AppSettings.Set("SupportedPidsCount", TripViewModel.SupportedPidsCount);
            AppSettings.Set("OBDProtocolDescription", TripViewModel.OBDProtocolDescription);
            AppSettings.Set("OBDVoltage", TripViewModel.OBDVoltage);
            AppSettings.Set("VIN", TripViewModel.VIN);
            AppSettings.Set("FuelType", TripViewModel.FuelType);
            AppSettings.Set("ECUs", string.Join(", ", TripViewModel.ECUs));
        }

        void Start()
        {
            var now = DateTime.Now;
            var name = $"Trip_{now.Year}{now.Month:00}{now.Day:00}_{now.Hour:00}.{now.Minute:00}.{now.Second:00}.{now.Millisecond:0000}";

            _file = Path.Combine(TripViewModel.PersonalPath, $"{name}.csv");

            Device.StartTimer(TimeSpan.FromMilliseconds(500), OnTimer);
        }

        void Stop()
        {
            _obdConnection?.Dispose();
            _logger = null;

            TripViewModel.Trips.Add(Trip.FromFileToTrip(_file));
        }

        bool OnTimer()
        {
            if (!TripViewModel.IsRecording)
                return false;

            try
            {
                var position = CrossGeolocator.Current
                    .GetLastKnownLocationAsync()
                    .WithTimeout(200).Result;

                var speed = _obdConnection.Command.Speed;
                var celv = _obdConnection.Command.CalculatedEngineLoadValue;
                var rpm = _obdConnection.Command.RPM;
                var iat = _obdConnection.Command.IntakeAirTemperature;
                var maf = _obdConnection.Command.MAFAirFlowRate;
                var fuel = _obdConnection.Command.GetValue(PIDCode.FuelTankLevel);

                //var stft1 = _obdConnection.Command.GetValue(PIDCode.ShortTermFuelTrimBank1);
                //var ltft1 = _obdConnection.Command.GetValue(PIDCode.LongTermFuelTrimBank1);
                //var throttlePos = _obdConnection.Command.ThrottlePosition;
                //var runtimeSinceStart = _obdConnection.Command.GetValue(PIDCode.RuntimeSinceEngineStart);
                //var distanceWithMILOn = _obdConnection.Command.GetValue(PIDCode.DistanceTraveledWithMILOn);
                //var engineFuelRate = _obdConnection.Command.GetValue(PIDCode.EngineFuelRate);

                TripViewModel.OBDValues[0].Value = $"{speed}";
                TripViewModel.OBDValues[1].Value = $"{celv:0.0#}";
                TripViewModel.OBDValues[2].Value = $"{rpm:0.0#}";
                TripViewModel.OBDValues[3].Value = $"{iat}";
                TripViewModel.OBDValues[4].Value = $"{maf:0.0#}";
                TripViewModel.OBDValues[5].Value = $"{fuel:0.0#}";

                var now = DateTime.Now.ToString("o");
                string line = $"{now},{position?.Latitude:0.0#####},{position?.Longitude:0.0#####},{TripViewModel.VIN},{rpm:0.0###},{speed},{iat:0.0#},{maf:0.0###},{celv:0.0###},{fuel:0.0#}\r\n";
                File.AppendAllText(_file, line);
            }
            catch (Exception ex)
            {
                _logger.WriteLine(ex.ToString());
                TripViewModel.IsRecording = false;
                DisplayAlert("CarAssist", $"Problem beim Abrufen der Messwerte: {ex.Message}", "Ok");
                return false;
            }
            return true;
        }

        async void OnUploadTrip(object sender, EventArgs e)
        {
            if (trips.SelectedItem is Trip trip)
            {
                string file = Path.Combine(TripViewModel.PersonalPath, trip.FileName) + ".csv";
                if (await DisplayAlert("CarAssist", "Sollen die Daten zu ThingSpeak hochgeladen werden?", "Ja", "Nein") == true)
                {
                    string result = await UploadTrip(file);
                    _logger?.WriteLine(result);
                }

                if (await DisplayAlert("CarAssist", "Sollen die Daten in Ihre OwnCloud hochgeladen werden?", "Ja", "Nein") == true)
                {
                    UploadTripToOwnCloud(file);
                }
            }
        }

        async Task<string> UploadTrip(string fileName)
        {
            string result = "";
            var api_key = "<Your ThingSpeak key>";

            var data = new ThingSpeakModel()
            {
                write_api_key = api_key,
                updates = new List<ThingSpeakOBD>()
            };

            int count = 0;
            try
            {
                using (UserDialogs.Instance.Loading("CarAssist", maskType: MaskType.Black))
                {
                    using (var reader = new StreamReader(File.OpenRead(fileName)))
                    {
                        string line = null;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            // Datetime,Latitude,Longitude,VIN,rpm,speed,iat,maf,celv,fuel
                            var values = line.Split(',');
                            var created_at = DateTime.Parse(values[0]).ToString("o");

                            data.updates.Add(new ThingSpeakOBD
                            {
                                created_at = created_at,
                                field1 = values[3],     // VIN
                                field2 = values[4],     // RPM
                                field3 = values[5],    // Speed
                                field4 = values[6],     // IntakeAirTemperature
                                field5 = values[7],     // MAFAirFlowRate
                                field6 = values[8],     // CalculatedEngineLoadValue
                                field7 = values[9]     // FuelTankLevel
                                //lat = values[1],
                                //lon = values[2]
                            });
                            count++;
                            if (count == 960)   // Free license, max. count
                                break;
                        }
                    }
                    await Send(data);
                    // hochgeladene Datei löschen
                    //File.Delete(fileName);
                    await DisplayAlert("CarAssist", $"{count} Datensätze hochgeladen", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("CarAssist", $"Hochgeladene Datensätze: {count}\n" + ex.Message, "Ok");
            }
            return result;
        }

        async void UploadTripToOwnCloud(string file)
        {
            using (UserDialogs.Instance.Loading("CarAssist", maskType: MaskType.Black))
            {
                try
                {
                    var user = AppSettings.Get<string>("OwncloudUser");
                    var password = AppSettings.Get<string>("OwncloudPassword");
                    var url = AppSettings.Get<string>("OwncloudUrl");

                    var fileName = Path.GetFileName(file);

                    await Task.Run(() =>
                    {
                        var client = new owncloudsharp.Client(url, user, password);

                        var stream = new MemoryStream(File.ReadAllBytes(file));

                        if (client.Upload("/" + fileName, stream, "text/plain") == false)
                            throw new Exception("Fehler bei Upload owncloud");
                    });

                    await DisplayAlert("CarAssist", $"Datei {fileName} erfolgreich hochgeladen!", "Ok");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("CarAssist", ex.Message, "Ok");
                }
            }
        }

        async Task<string> Send(ThingSpeakModel data)
        {
            var result = "";
            var channelId = "<Your channel id>";

            using (var client = new HttpClient())
            {
                var url = $"https://api.thingspeak.com/channels/{channelId}/bulk_update.json";

                var content = JsonConvert.SerializeObject(data);
                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(content, Encoding.UTF8, "application/json") };
                var res = await client.SendAsync(req);


                if (res.StatusCode == HttpStatusCode.Accepted)
                    result += await res.Content.ReadAsStringAsync();
                else
                    throw new Exception($"Fehler beim Übertragen an ThingSpeak: {res.StatusCode}");
            }
            return result;
        }

        void OnViewTrip(object sender, EventArgs e)
        {
            if (trips.SelectedItem is Trip trip)
            {
                Navigation.PushAsync(
                    new ViewTripPage(
                        Path.Combine(TripViewModel.PersonalPath, trip.FileName) + ".csv"));
            }
        }

        void OnDeleteTrip(object sender, EventArgs e)
        {
            if (trips.SelectedItem is Trip trip)
            {
                TripViewModel.Trips.Remove(trip);
                File.Delete(Path.Combine(TripViewModel.PersonalPath, trip.FileName) + ".csv");
                if (TripViewModel.Trips.Count > 0)
                    trips.SelectedItem = TripViewModel.Trips[0];
            }
        }
    }
}