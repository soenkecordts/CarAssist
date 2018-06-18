using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps.Model;

using CarAssist.CustomRenderer;
using CarAssist.Droid.CustomRenderer;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CarAssist.Droid.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer
    {
        public CustomMapRenderer(Context context) : base(context) { }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName)
                Update();
        }

        CustomMap _formsMap;
        PolylineOptions _polylineOptions;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _formsMap = (CustomMap)e.NewElement;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);

            _polylineOptions = new PolylineOptions();
            _polylineOptions.InvokeColor(0x66FF0000);
        }

        private void Update()
        {
            foreach (var position in _formsMap.PolylineCoordinates)
            {
                _polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }
            NativeMap.AddPolyline(_polylineOptions);
        }
    }
}