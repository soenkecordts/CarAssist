using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CarAssist.CustomRenderer;
using CarAssist.iOS.CustomRenderer;
using CoreLocation;
using Foundation;
using MapKit;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CarAssist.iOS.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer
    {
        MKPolylineRenderer _polylineRenderer;


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName)
                Update();
        }

        MKMapView _nativeMap;
        CustomMap _formsMap;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _nativeMap = Control as MKMapView;
                if (_nativeMap != null)
                {
                    _nativeMap.RemoveOverlays(_nativeMap.Overlays);
                    _nativeMap.OverlayRenderer = null;
                    _polylineRenderer = null;
                }
            }

            if (e.NewElement != null)
            {
                _formsMap = (CustomMap)e.NewElement;
                _nativeMap = Control as MKMapView;

                _nativeMap.OverlayRenderer = GetOverlayRenderer;

                Update();
            }
        }

        MKPolyline _routeOverlay;

        void Update()
        {
            CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[_formsMap.PolylineCoordinates.Count];

            int index = 0;
            foreach (var position in _formsMap.PolylineCoordinates)
            {
                coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
                index++;
            }

            if (_nativeMap.Overlays?.Length > 0 && _routeOverlay != null)
                _nativeMap.RemoveOverlay(_routeOverlay);

            _routeOverlay = MKPolyline.FromCoordinates(coords);
            _nativeMap.AddOverlay(_routeOverlay);
        }

        MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
        {
            if (_polylineRenderer == null && !Equals(overlayWrapper, null))
            {
                var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
                _polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline)
                {
                    FillColor = UIColor.Blue,
                    StrokeColor = UIColor.Red,
                    LineWidth = 3,
                    Alpha = 0.4f
                };
            }
            return _polylineRenderer;
        }
    }
}