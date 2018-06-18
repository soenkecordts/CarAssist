using System.Collections.Generic;
using System.ComponentModel;
using CarAssist.CustomRenderer;
using CarAssist.UWP.CustomRenderer;

using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CarAssist.UWP.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer
    {
        CustomMap _formsMap;
        MapControl _nativeMap;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName)
                Update();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _formsMap = (CustomMap)e.NewElement;
                _nativeMap = Control as MapControl;
            }
        }

        private void Update()
        {
            if (_formsMap == null || 
                _nativeMap == null || 
                _formsMap.PolylineCoordinates == null ||
                _formsMap?.PolylineCoordinates.Count == 0)
                return;

            var coordinates = new List<BasicGeoposition>();

            if(_nativeMap.MapElements.Count > 0)
                _nativeMap.MapElements.RemoveAt(0);

            foreach (var position in _formsMap.PolylineCoordinates)
            {
                coordinates.Add(new BasicGeoposition() { Latitude = position.Latitude, Longitude = position.Longitude });
            }

            var polyline = new MapPolyline
            {
                StrokeColor = Windows.UI.Color.FromArgb(128, 255, 0, 0),
                StrokeThickness = 5,
                Path = new Geopath(coordinates)
            };

            _nativeMap.MapElements.Add(polyline);

        }
    }
}
