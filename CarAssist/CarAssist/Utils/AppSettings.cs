using Xamarin.Forms;

namespace CarAssist.Utils
{
    public class AppSettings
    {
        public static TValue Get<TValue>(string key)
        {
            if (Application.Current.Properties.ContainsKey(key))
                return (TValue)Application.Current.Properties[key];

            return default(TValue);
        }

        public static void Set<TValue>(string key, TValue value)
        {
            Application.Current.Properties[key] = value;
        }

        public static string ConnectionStringTcp
        {
            get
            {
                var tcpAddress = AppSettings.Get<string>("TcpAddress");
                var tcpPort = AppSettings.Get<string>("TcpPort");

                return tcpAddress != null && tcpPort != null ?
                    $"tcp://{tcpAddress}:{tcpPort}" : null;
            }
        }
        public static string ConnectionStringBluetooth
        {
            get
            {
                var address = AppSettings.Get<string>("BluetoothAddress");
                var name = AppSettings.Get<string>("BluetoothName");

                return address != null && name != null ?
                    $"bt://{address.Replace(':', '-')}/{name}" : null;
            }
        }
    }
}
