namespace CarAssist.Connection
{
    public class BluetoothInfo
    {
        public BluetoothInfo(string name, string address, string deviceClass, string type)
        {
            Name = name;
            Address = address;
            Type = type;
            DeviceClass = deviceClass;
        }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string DeviceClass { get; private set; }
        public string Type { get; private set; }

        public override string ToString()
        {
            return $"{Name}\n{Address}\n{DeviceClass}\n{Type}";
        }
    }
}
