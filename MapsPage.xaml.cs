using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps
{
    public partial class MapsPage : ContentPage
    {
        public MapsPage()
        {
            InitializeComponent();
            var mp = new CustomMap();

            var pin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(37.79752, -122.40183),
                Label = "YMCA",
                Address = "394 Pacific Ave, San Francisco CA",
                Id = "Xamarin",
                Url = "http://ymca.org",
                streetview = "Abhishek"
            };
            var pin2 = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(37.79500, -122.40000),
                Label = "YMCA San Francisco Office",
                Address = "394 Pacific Ave, San Francisco CA",
                Id = "Xamarin",
                Url = "https://www.google.com/maps/@33.2249104,-117.2909954,3a,90y,-4.81h,88.84t/data=!3m6!1e1!3m4!1sAF1QipNJoAGctctHWEOcli2MR1SkS2dGySR7OxaHMlmT!2e10!7i13312!8i6656",
                streetview = "my home"
            };

            mp.CustomPins = new List<CustomPin> { pin, pin2 };
            foreach (var item in mp.CustomPins)
            {
                mp.Pins.Add(item);

            }



            mp.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));

            Content = mp;
        }
    }

    public class CustomPin : Pin
    {
        public string Url { get; set; }
        public string streetview { get; set; }

    }
}
