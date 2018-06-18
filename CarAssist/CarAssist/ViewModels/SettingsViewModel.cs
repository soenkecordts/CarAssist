using CarAssist.Connection;
using CarAssist.Utils;

namespace CarAssist.ViewModels
{
    public class SettingsViewModel : BaseNotifyPropertyChanged
    {
        string _bluetoothAddress;
        string _bluetoothName;
        string _tcpAddress;
        string _tcpPort = "35000";
        string _owncloudUser;
        string _owncloudUrl = "https://www2.wi.fh-flensburg.de/owncloud";
        string _owncloudPassword;

        public SettingsViewModel()
        {
            BluetoothAddress = AppSettings.Get<string>("BluetoothAddress");
            BluetoothName = AppSettings.Get<string>("BluetoothName");

            if (BluetoothAddress != null)
            {
                for (int i = 0; i < ConnectionFactory.PairedDevices.Length; i++)
                {
                    var device = ConnectionFactory.PairedDevices[i];
                    if (device.Address == BluetoothAddress)
                    {
                        SelectedBluetoothIndex = i;
                        break;
                    }
                }
            }
            if (AppSettings.Get<string>("TcpAddress") != null)
                TcpAddress = AppSettings.Get<string>("TcpAddress");
            if (AppSettings.Get<string>("TcpPort") != null)
                TcpPort = AppSettings.Get<string>("TcpPort");
            if (AppSettings.Get<string>("OwncloudUrl") != null)
                OwncloudUrl = AppSettings.Get<string>("OwncloudUrl");
            if (AppSettings.Get<string>("OwncloudUser") != null)
                OwncloudUser = AppSettings.Get<string>("OwncloudUser");
            if (AppSettings.Get<string>("OwncloudPassword") != null)
                OwncloudPassword = AppSettings.Get<string>("OwncloudPassword");
        }

        public string BluetoothAddress
        {
            get
            {
                return _bluetoothAddress;
            }
            set
            {
                OnPropertyChanged(ref _bluetoothAddress, value);
            }
        }
        public string BluetoothName
        {
            get
            {
                return _bluetoothName;
            }
            set
            {
                OnPropertyChanged(ref _bluetoothName, value);
            }
        }
        public string TcpAddress
        {
            get
            {
                return _tcpAddress;
            }
            set
            {
                OnPropertyChanged(ref _tcpAddress, value);
            }
        }
        public string TcpPort
        {
            get
            {
                return _tcpPort;
            }
            set
            {
                OnPropertyChanged(ref _tcpPort, value);
            }
        }

        public BluetoothInfo[] BluetoothDevices
        {
            get
            {
                return ConnectionFactory.PairedDevices;
            }
        }

        int _selectedBluetoothIndex;
        public int SelectedBluetoothIndex
        {
            get
            {
                return _selectedBluetoothIndex;
            }
            set
            {
                OnPropertyChanged(ref _selectedBluetoothIndex, value);
            }
        }

        public string OwncloudUser
        {
            get
            {
                return _owncloudUser;
            }
            set
            {
                OnPropertyChanged(ref _owncloudUser, value);
            }
        }

        public string OwncloudUrl
        {
            get
            {
                return _owncloudUrl;
            }
            set
            {
                OnPropertyChanged(ref _owncloudUrl, value);
            }
        }
        public string OwncloudPassword
        {
            get
            {
                return _owncloudPassword;
            }
            set
            {
                OnPropertyChanged(ref _owncloudPassword, value);
            }
        }
    }
}
