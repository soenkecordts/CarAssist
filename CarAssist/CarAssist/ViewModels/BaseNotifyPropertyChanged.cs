using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarAssist.ViewModels
{
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {

        protected void OnPropertyChanged<T>(ref T valueOld, T valueNew, [CallerMemberName]string propertyName = null)
        {
            if (valueOld == null || !valueOld.Equals(valueNew))
            {
                valueOld = valueNew;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
