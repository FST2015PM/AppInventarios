using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace MapDemo
{
   [Activity(Label = "MapDemo", MainLauncher = true, Icon = "@drawable/icon")]
   public class MainActivity : Activity, IOnMapReadyCallback
   {

      private GoogleMap _map;
      private MapFragment _mapFragment;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);
         SetContentView(Resource.Layout.Main);
         InitMapFragment();
         SetupMapIfNeeded();
      }

      private bool SetupMapIfNeeded()
      {
         if (_map == null)
         {
            if (_map != null)
            {
               LatLng clientLocation;
               clientLocation = new LatLng(51.5033640, -0.1276250);
               _map.AddMarker(new MarkerOptions().SetPosition(clientLocation));
               // We create an instance of CameraUpdate, and move the map to it.
               CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(clientLocation, 12);
               _map.MoveCamera(cameraUpdate);
               return true;
            }
            return false;
         }
         return true;
      }

      private void InitMapFragment()
      {
         _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
         if (_mapFragment == null)
         {
            GoogleMapOptions mapOptions = new GoogleMapOptions()
                .InvokeMapType(GoogleMap.MapTypeSatellite)
                .InvokeZoomControlsEnabled(false)
                .InvokeCompassEnabled(true);

            FragmentTransaction fragTx = FragmentManager.BeginTransaction();
            _mapFragment = MapFragment.NewInstance(mapOptions);
            fragTx.Add(Resource.Id.mapWithOverlay, _mapFragment, "map");
            fragTx.Commit();
         }
         _mapFragment.GetMapAsync(this);
      }

      protected override void OnResume()
      {
         base.OnResume();
         if (SetupMapIfNeeded())
         {
            _map.MyLocationEnabled = true;
         }
      }

      protected override void OnPause()
      {
         base.OnPause();

         // Pause the GPS - we won't have to worry about showing the 
         // location.
         _map.MyLocationEnabled = false;
      }

      public void OnMapReady(GoogleMap map)
      {
         _map = map;
      }


   }
}

