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
using Android.Gms.Maps;

namespace PueblosMagicos.Android.Inventario
{
   public class MyOnMapReady: Java.Lang.Object, IOnMapReadyCallback
   {
      public GoogleMap Map { get; set; }
      public event EventHandler MapReady;
      public void OnMapReady(GoogleMap googleMap)
      {
         Map = googleMap;
         var handler = MapReady;
         if (handler != null)
            handler(this, EventArgs.Empty);
      }
   }
}