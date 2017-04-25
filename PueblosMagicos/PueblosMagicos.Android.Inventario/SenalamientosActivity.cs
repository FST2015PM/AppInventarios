using Android.App;
using AndroidApp = Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Locations;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "SenalamientosActivity", Theme = "@style/MyTheme.ListFont", UiOptions = UiOptions.SplitActionBarWhenNarrow, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
   public class SenalamientosActivity : Activity, IOnMapReadyCallback
   {
      const int ConnectionFailureResolutionRequest = 9000;
      public const string SAMPLE_CATEGORY = "mono.apidemo.sample";
      private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
      private Button SenalMapaPopupBtn, SenalMapaSigBtn;
      private Color selectedColor, deselectedColor;
      DateTime lastRefresh = DateTime.UtcNow;

      private GoogleMap _map;
      private MapFragment _mapFragment;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.Senalamientos);

         //MenuLateral
         mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
         
         mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_icon);
         tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_icon);
         SenalMapaPopupBtn = FindViewById<Button>(Resource.Id.SenalMapaPopupBtn);
         SenalMapaSigBtn = FindViewById<Button>(Resource.Id.SenalMapaSigBtn);
         
         selectedColor = Color.ParseColor("#303030"); //The color u want    
         deselectedColor = Color.ParseColor("#ffffff");

         deselectAll();
         tab1Button.SetColorFilter(selectedColor);

         SenalMapaSigBtn.Click += showTab2;
         
         SenalMapaPopupBtn.Click += delegate
         {
             SenalMapaPopupBtn.Visibility = ViewStates.Gone;
         };

         tab2Button.Click += showTab2;

         tab3Button.Click += delegate
         {
             deselectAll();
             tab3Button.SetColorFilter(selectedColor);

             var intent = new Intent(this, typeof(SenalamientosOrientacionActivity));
             StartActivity(intent);
         };

         tab4Button.Click += delegate
         {
             deselectAll();
             tab4Button.SetColorFilter(selectedColor);

             var intent = new Intent(this, typeof(SenalamientosTextosActivity));
             StartActivity(intent);
         };

         InitMapFragment();
      }
  
      protected override void OnResume()
      {
         base.OnResume();
        deselectAll();
        tab1Button.SetColorFilter(selectedColor);
        SetupMapIfNeeded();

      }
    private void InitMapFragment()
      {
         _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
         if (_mapFragment == null)
         {
            GoogleMapOptions mapOptions = new GoogleMapOptions()
                .InvokeMapType(GoogleMap.MapTypeNormal)
                .InvokeZoomControlsEnabled(false)
                .InvokeCompassEnabled(true);

            AndroidApp.FragmentTransaction fragTx = FragmentManager.BeginTransaction();
            _mapFragment = MapFragment.NewInstance(mapOptions);
            fragTx.Add(Resource.Id.map, _mapFragment, "map");
            fragTx.Commit();
         }
         _mapFragment.GetMapAsync(this);
      }

      private void SetupMapIfNeeded()
      {
         if (_map == null)
         {
            if (_map != null)
            {
               MarkerOptions markerOpt1 = new MarkerOptions();
               markerOpt1.SetPosition(new LatLng(GlobalVariables.Latitud, GlobalVariables.Longitud));
               markerOpt1.Draggable(true);
               _map.AddMarker(markerOpt1);
               _map.MarkerDragEnd += (s, e) =>
               {
                  GlobalVariables.Latitud = e.Marker.Position.Latitude;
                  GlobalVariables.Longitud = e.Marker.Position.Longitude;
                  GlobalVariables.LatitudCoordinates = LocationConverter.getLatitudeAsDMS(GlobalVariables.Latitud, 2);
                  GlobalVariables.LongitudCoordinates = LocationConverter.getLongitudeAsDMS(GlobalVariables.Longitud, 2);
               };
               // We create an instance of CameraUpdate, and move the map to it.
               CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(new LatLng(GlobalVariables.Latitud, GlobalVariables.Longitud), 12);
               _map.MoveCamera(cameraUpdate);
            }
         }
      }

      public void OnMapReady(GoogleMap map)
      {

      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);
         tab4Button.SetColorFilter(deselectedColor);
      }

      void showTab2(object sender, EventArgs e)
      {
          deselectAll();
          tab2Button.SetColorFilter(selectedColor);

          var intent = new Intent(this, typeof(SenalamientosFotosActivity));
          StartActivity(intent);
      }

      private void OnMenuLateralItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
          var listView = sender as ListView;
          var posicion = e.Position;
          switch (posicion)
          {
              case 0:
                  StartActivity(typeof(SenalamientosMapActivity));
                  break;
              case 3:
                  StartActivity(typeof(MercadosActivity));
                  break;
              case 4:
                  StartActivity(typeof(CajerosActivity));
                  break;
              case 6:
                  StartActivity(typeof(OficinasActivity));
                  break;
          }
          mDrawerLayout.CloseDrawer(mLeftDrawer);
      }

   }
}