using System.Threading.Tasks;

namespace CarAssist.Connection
{
    public interface IBluetooth : IConnection
    {
        string Uuid { get; set; }
        BluetoothInfo[] PairedDevices { get; }
    }
}
