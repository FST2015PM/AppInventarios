using Android.App;
using AndroidApp = Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android;
using Android.Content;
using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PueblosMagicos.Android.Inventario.DataTables;
using PueblosMagicos.Android.Inventario.Services;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "CableadoActivity", ParentActivity = typeof(SeleccionarModuloActivity))]
   public class CableadoActivity : Activity, IOnMapReadyCallback
   {
      DataService data;
      private ImageButton tab1Button;
      private ImageButton btnInicio;
      private Button SiguienteMapaBtn;
      private Button trazarLineaBtnCableado;
      private Color selectedColor, deselectedColor;
      private Button limpiarLineaBtnCableado;
      private Button guardarBtnCableado;
      private GoogleMap _map;
      private MapFragment _mapFragment;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      List<LatLng> points;
      protected override void OnCreate(Bundle bundle)
      {

         base.OnCreate(bundle);
         data = new DataService();

         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.Cableado);

         InitMapFragment();

         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_cableado_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicioCableado);

         SiguienteMapaBtn = FindViewById<Button>(Resource.Id.siguienteMapaBtnCableado);
         trazarLineaBtnCableado = FindViewById<Button>(Resource.Id.trazarLineaBtnCableado);
         limpiarLineaBtnCableado = FindViewById<Button>(Resource.Id.limpiarLineaBtnCableado);
         guardarBtnCableado = FindViewById<Button>(Resource.Id.guardarBtnCableado);

         selectedColor = Color.ParseColor("#ffffff");
         deselectedColor = Color.ParseColor("#f0aad0");

         deselectAll();
         tab1Button.SetColorFilter(selectedColor);

         SiguienteMapaBtn.Click += showTab2;

         btnInicio.Click += delegate
         {
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         };

         trazarLineaBtnCableado.Click += delegate
         {
            var polylineoption = new PolylineOptions();
            polylineoption.InvokeColor(Color.Red);
            polylineoption.Geodesic(true);

            polylineoption.Add(points.ToArray());
            RunOnUiThread(() =>
            _map.AddPolyline(polylineoption));
         };

         limpiarLineaBtnCableado.Click += delegate
         {
            SetupMapIfNeeded(false, true);
         };

         guardarBtnCableado.Click += delegate
         {

            Cableados cableado = new Cableados()
            {
               Id = Guid.NewGuid().ToString(),
               pointsArray = String.Join(";", points.Select(n => n.Latitude.ToString() + "," + n.Longitude.ToString()).ToList().ToArray()),
               Enviado = "0"
            };
            data.SaveData(cableado);

            GlobalVariables.cableadoNuevaCaptura = true;
            Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         };
      }


      private bool SetupMapIfNeeded(bool soloAñadirMarcador = false, bool clearMarkers = false)
      {
         if (_map != null)
         {
            if (clearMarkers)
            {
               points = null;
               _map.Clear();
            }

            LatLng clientLocation;
            clientLocation = new LatLng(GlobalVariables.LatitudCableado, GlobalVariables.LongitudCableado);
            var marker = new MarkerOptions().SetPosition(clientLocation).Draggable(true);
            _map.AddMarker(marker);
            if (soloAñadirMarcador == false)
            {
               if (points == null)
                  points = new List<LatLng>();

               _map.MarkerDragEnd += (s, e) =>
               {
                  GlobalVariables.LatitudCableado = e.Marker.Position.Latitude;
                  GlobalVariables.LongitudCableado = e.Marker.Position.Longitude;
                  points.Add(new LatLng(e.Marker.Position.Latitude, e.Marker.Position.Longitude));
               };

               CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(clientLocation, 15);
               _map.MoveCamera(cameraUpdate);
            }
            return true;
         }
         return false;
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
            fragTx.Add(Resource.Id.mapCableado, _mapFragment, "map");
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

         _map.MyLocationEnabled = false;
      }

      public void OnMapReady(GoogleMap map)
      {
         _map = map;
         SetupMapIfNeeded();
      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
      }

      void showTab2(object sender, EventArgs e)
      {
         //Aqui añadiremos otro marcador para generar la polilinea
         SetupMapIfNeeded(true);
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
            case 1:
               StartActivity(typeof(MercadosActivity));
               break;
            case 2:
               StartActivity(typeof(CajerosActivity));
               break;
            case 3:
               StartActivity(typeof(OficinasActivity));
               break;
            case 4:
               StartActivity(typeof(AgenciasActivity));
               break;
            case 5:
               StartActivity(typeof(EstacionamientosActivity));
               break;
            case 6:
               StartActivity(typeof(FachadasActivity));
               break;
            case 7:
               StartActivity(typeof(WifisActivity));
               break;
            case 8:
               break;
         }
      }

   }
}