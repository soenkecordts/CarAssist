using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarAssist.CustomRenderer
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            PolylineCoordinates = new ObservableCollection<Position>();
            PolylineCoordinates.CollectionChanged += (sender, e) => 
            {                
                OnPropertyChanged("PolylineCoordinates");
            };
        }

        public ObservableCollection<Position> PolylineCoordinates
        {
            get { return (ObservableCollection<Position>)GetValue(PolylineCoordinatesProperty); }
            set { SetValue(PolylineCoordinatesProperty, value); }
        } 

        public static readonly BindableProperty PolylineCoordinatesProperty = 
            BindableProperty.Create(nameof(PolylineCoordinates), typeof(ObservableCollection<Position>), typeof(CustomMap), null);
    }
}
