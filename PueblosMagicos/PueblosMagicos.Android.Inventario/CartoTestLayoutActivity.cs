using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Carto.Ui;
using Carto.Layers;
using Carto.DataSources;
using Carto.Utils;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "CartoTestLayoutActivity")]
   public class CartoTestLayoutActivity : Activity
   {
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         // Register license BEFORE creating MapView (done in SetContentView)
         MapView.RegisterLicense("XTUMwQ0ZRQ2tWdWxXNWdWWUxRRmtlOU1mRlZQQUhaeTFQUUlVUk9tbDdxUVJlcEpIanRyYzd5NWdMQXpQSlVBPQoKYXBwVG9rZW49YTBjOTZkNWQtZmIzMS00NmM0LWE1MjQtNjMxNmRmOGFhYzI3CnBhY2thZ2VOYW1lPXB1ZWJsb3NtYWdpY29zLmFuZHJvaWQuaW52ZW50YXJpbwpvbmxpbmVMaWNlbnNlPTEKcHJvZHVjdHM9c2RrLWFuZHJvaWQtNC4qCndhdGVybWFyaz1jYXJ0b2RiCg==", this);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.Main);

         // Get our map from the layout resource. 
         var mapView = FindViewById<MapView>(Resource.Id.mapView);

         // Online vector base layer with default style
         var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

         // Add online base layer  
         mapView.Layers.Add(baseLayer);
      }
   }
}