using System;
using System.Collections.Generic;
using System.ComponentModel;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Maps.CustomMap), typeof(Maps.iOS.mymaprender))]
namespace Maps.iOS
{

    public class mymaprender : Xamarin.Forms.Maps.iOS.MapRenderer
    {
        UIView customPinView;
        List<CustomPin> customPins;


        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = null;
                nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                customPins = formsMap.CustomPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Id.ToString());
            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Id.ToString());
                annotationView.Image = UIImage.FromFile("pin.png");
                annotationView.CalloutOffset = new CoreGraphics.CGPoint(0, 0);
                // annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.InfoDark);
                ((CustomMKAnnotationView)annotationView).Id = customPin.Id.ToString();
                ((CustomMKAnnotationView)annotationView).Url = customPin.Url;
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            if (!string.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            }
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();


            customPinView.Frame = new CoreGraphics.CGRect(0, 0, 200, 84);
            var lb = new UILabel(new CoreGraphics.CGRect(0, 0, 200, 84));
            lb.Text = "StreetView";
            lb.BackgroundColor = UIColor.White;
            //     UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(TapThat);
            // lb.GestureRecognizers=new UIGestureRecognizer()
            //image.Image = UIImage.FromFile("xamarin.png");
            customPinView.AddSubview(lb);
            customPinView.Center = new CoreGraphics.CGPoint(0, -(e.View.Frame.Height + 75));
            e.View.AddSubview(customPinView);

            //if (customView.Id == "Xamarin")
            //{
            //customPinView.Frame = new CoreGraphics.CGRect(0, 0, 200, 84);
            //var image = new UIImageView(new CoreGraphics.CGRect(0, 0, 200, 84));
            //image.Image = UIImage.FromFile("xamarin.png");
            //customPinView.AddSubview(image);
            //customPinView.Center = new CoreGraphics.CGPoint(0, -(e.View.Frame.Height + 75));
            //e.View.AddSubview(customPinView);
            //}
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }


    public class CustomMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id)
        {
        }
    }
}
