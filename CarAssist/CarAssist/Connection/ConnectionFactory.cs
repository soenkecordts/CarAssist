using System;
using Xamarin.Forms;

namespace CarAssist.Connection
{
    public class ConnectionFactory
    {
        public static IConnection Create(string connectionString)
        {
            IConnection connection;

            var uri = new Uri(connectionString);

            if (uri.Scheme == "tcp")
            {
                connection = new Tcp() { Address = uri.Host, Port = uri.Port };
            }
            else if (uri.Scheme == "bt")
            {
                connection = DependencyService.Get<IBluetooth>();
                connection.Address = uri.Host.Replace('-', ':').ToUpper();
            }
            else
            {
                throw new ArgumentException("ConnectionFactory: Wrong scheme in connection string.");
            }
            return connection;
        }

        static public BluetoothInfo[] PairedDevices
        {
            get
            {
                return DependencyService.Get<IBluetooth>()?.PairedDevices;
            }
        }
    }
}
