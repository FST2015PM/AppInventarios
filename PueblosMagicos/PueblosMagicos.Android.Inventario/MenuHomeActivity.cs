using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using AndroidApp = Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Content.PM;
using PueblosMagicos.Android.Inventario.Services;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "Menu", ParentActivity = typeof(SeleccionarModuloActivity), ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
   public class MenuHomeActivity : Activity, IOnMapReadyCallback
   {
      DataService data;
      private GoogleMap _map;
      private MapFragment _mapFragment;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;

      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.MenuHome);
         data = new DataService();
         InitMapFragment();

         //MenuLateral
         mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
         if (!GlobalVariables.menuLateralListAdapter.Any())
         {
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Señalamientos turísticos", ImageResourceId = Resource.Drawable.icSenalamientos, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cableado subterráneo", ImageResourceId = Resource.Drawable.icCableado, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Estacionamientos", ImageResourceId = Resource.Drawable.icEstacionamientos, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Mercados", ImageResourceId = Resource.Drawable.icMercados, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cajeros automáticos", ImageResourceId = Resource.Drawable.icCajeros, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "WiFi", ImageResourceId = Resource.Drawable.icWiFi, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Oficinas de congresos", ImageResourceId = Resource.Drawable.icCongresos, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Agencias y tour operadores", ImageResourceId = Resource.Drawable.icAgencias, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Fachadas restauradas", ImageResourceId = Resource.Drawable.icFachadas, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            //GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cuestionario", ImageResourceId = Resource.Drawable.icCuestionario, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
         }

         mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         mLeftDrawer.ItemClick += OnMenuLateralItemClick;

      }
      public override void OnBackPressed()
      {
         AlertDialog.Builder builder = new AlertDialog.Builder(this);
         AlertDialog alertDialog = builder.Create();

         //title
         alertDialog.SetTitle("Pueblos Magicos");

         //icon
         //alertDialog.SetIcon(Resource.Drawable.Icon);

         //question or message
         alertDialog.SetMessage("¿Desea Salir de la Aplicación? ");

         //Buttons
         alertDialog.SetButton("No", (s, ev) =>
         {
            //Do something
            // Toast.MakeText(this, "Error", ToastLength.Long).Show();
         });

         alertDialog.SetButton2("Si", (s, ev) =>
         {
            Process.KillProcess(Process.MyPid());
         });

         alertDialog.Show();
      }
      private bool SetupMapIfNeeded()
      {
         if (_map != null)
         {
            LatLng clientLocation;
            clientLocation = new LatLng(GlobalVariables.Latitud, GlobalVariables.Longitud);
            var marker = new MarkerOptions().SetPosition(clientLocation).Draggable(false);
            _map.AddMarker(marker);

            var senalamientosIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.senalamiento);
            LatLng clientLocationSenal;
            MarkerOptions markerSenal;
            foreach (var signal in data.AllSignals())
            {
               clientLocationSenal = new LatLng(signal.Latitud, signal.Longitud);
               markerSenal = new MarkerOptions().SetPosition(clientLocationSenal).Draggable(false).SetIcon(senalamientosIcon);
               _map.AddMarker(markerSenal);
               clientLocationSenal = null;
               markerSenal = null;
            }

            var mercadosIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.mercados);
            LatLng clientLocationMercado;
            MarkerOptions markerMercado;
            foreach (var market in data.AllMarkets())
            {
               clientLocationMercado = new LatLng(market.Latitud, market.Longitud);
               markerMercado = new MarkerOptions().SetPosition(clientLocationMercado).Draggable(false).SetIcon(mercadosIcon);
               _map.AddMarker(markerMercado);
               clientLocationMercado = null;
               markerMercado = null;
            }

            var cajeroIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.cajeros);
            LatLng clientLocationCajero;
            clientLocationCajero = new LatLng(GlobalVariables.LatitudCajero, GlobalVariables.LongitudCajero);
            var markerCajero = new MarkerOptions().SetPosition(clientLocationCajero).Draggable(false).SetIcon(cajeroIcon);
            _map.AddMarker(markerCajero);

            var officeIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.oficinas);
            LatLng clientLocationOffice;
            clientLocationOffice = new LatLng(GlobalVariables.LatitudOficina, GlobalVariables.LongitudOficina);
            var markerOficina = new MarkerOptions().SetPosition(clientLocationOffice).Draggable(false).SetIcon(officeIcon);
            _map.AddMarker(markerOficina);

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(clientLocation, 15);
            _map.MoveCamera(cameraUpdate);
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
            fragTx.Add(Resource.Id.mapMenuHome, _mapFragment, "map");
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
         SetupMapIfNeeded();
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
               var intent = new Intent(this, typeof(MercadosActivity));
               StartActivity(intent);
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