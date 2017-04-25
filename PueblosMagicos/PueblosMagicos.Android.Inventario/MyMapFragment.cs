using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;

namespace PueblosMagicos.Android.Inventario
{
   public class MyMapFragment : Fragment, IOnMapReadyCallback
   {
      private View rootView;
      GoogleMap myMap;
      MapView mMapView;

      private MapFragment mapFragment;
      public override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         // Create your fragment here
      }

      public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
      {
         try
         {
            rootView = inflater.Inflate(Resource.Layout.mymap_fragment, container, false);
            MapsInitializer.Initialize(Activity);
            mMapView = (MapView)rootView.FindViewById(Resource.Id.map);
            mMapView.OnCreate(savedInstanceState);
            mMapView.GetMapAsync(this);
         }
         catch (InflateException e)
         {
         }
         return rootView;
      }

      public override void OnPause()
      {
         base.OnPause();
         mMapView.OnPause();
      }

      public override void OnDestroy()
      {
         base.OnDestroy();
         mMapView.OnDestroy();
      }

      public override void OnSaveInstanceState(Bundle outState)
      {
         base.OnSaveInstanceState(outState);
         mMapView.OnSaveInstanceState(outState);
      }

      public override void OnLowMemory()
      {
         base.OnLowMemory();
         mMapView.OnLowMemory();
      }

      public override void OnResume()
      {
         base.OnResume();
         mMapView.OnResume();
      }

      public override void OnDestroyView()
      {
         base.OnDestroyView();
      }
      public void OnMapReady(GoogleMap googleMap)
      {

      }
   }
}