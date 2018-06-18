using CarAssist.Obd;
using CarAssist.ViewModels;

namespace CarAssist.Models
{
    public class OBDValue : BaseNotifyPropertyChanged
    {
        public OBDValue(string description, PIDCode pidCode, string value = "")
        {
            Description = description;
            PIDCode = pidCode;
            Value = value;
        }
        public string Description { get; set; }
        public PIDCode PIDCode { get; set; }

        string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                OnPropertyChanged(ref _value, value);
            }
        }

        public string Image
        {
            get
            {
                return $"PID_{PIDCode}.png";
            }
        }
    }
}