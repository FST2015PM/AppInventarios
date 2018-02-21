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
using System.Threading.Tasks;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "Menu", ParentActivity = typeof(SeleccionarModuloActivity), ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
   public class MenuHomeActivity : Activity, IOnMapReadyCallback
   {
      DataService data;
      private GoogleMap _map;
      private MapFragment _mapFragment;
      SecurityService securityService;
      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      bool showSignals = true;
      bool showMarkets = true;
      bool showATM = true;
      bool showOffices = true;
      bool showAgencies = true;
      bool showParkings = true;
      bool showWiFis = true;
      bool showCables = true;

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
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "", ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, isHeader = true, Activado = true });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Señalamientos turísticos", ImageResourceId = Resource.Drawable.icSenalamientos, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Mercados", ImageResourceId = Resource.Drawable.icMercados, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cajeros automáticos", ImageResourceId = Resource.Drawable.icCajeros, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Oficinas de congresos", ImageResourceId = Resource.Drawable.icCongresos, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Agencias y tour operadores", ImageResourceId = Resource.Drawable.icAgencias, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Estacionamientos", ImageResourceId = Resource.Drawable.icEstacionamientos, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "WiFi", ImageResourceId = Resource.Drawable.icWiFi, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cableado subterráneo", ImageResourceId = Resource.Drawable.icCableado, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Sincronizar con la plataforma", ImageResourceId = Resource.Drawable.icCuestionario, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
            //GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Bitacora", ImageResourceId = Resource.Drawable.icFachadas, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         }

         mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         mLeftDrawer.ItemClick += OnMenuLateralItemClick;
         FindViewById<Button>(Resource.Id.mapMenuHomeAddMarker).Click += GoToMenu;
         FindViewById<Button>(Resource.Id.mapMenuHomeShowMenu).Click += ShootMenu;
      }

      void ShootMenu(object sender, EventArgs e)
      {
         try
         {
            mDrawerLayout.OpenDrawer(mLeftDrawer);
         }
         catch (Exception ex)
         {
            string errorMessage = ex.Message;
         }

      }

      void GoToMenu(object sender, EventArgs e)
      {
         try
         {
            StartActivity(typeof(SeleccionarModuloActivity));
         }
         catch (Exception ex)
         {
            string errorMessage = ex.Message;
         }

      }
      public override void OnBackPressed()
      {
         AlertDialog.Builder builder = new AlertDialog.Builder(this);
         AlertDialog alertDialog = builder.Create();

         alertDialog.SetTitle("Pueblos Magicos");
         alertDialog.SetMessage("¿Desea Salir de la Aplicación? ");
         alertDialog.SetButton("No", (s, ev) =>
         {
         });

         alertDialog.SetButton2("Si", (s, ev) =>
         {
            Intent salida = new Intent(Intent.ActionMain);
            this.FinishAffinity();
            this.Finish();
            Process.KillProcess(Process.MyPid());
            System.Environment.Exit(0);
            this.MoveTaskToBack(true);
         });

         alertDialog.Show();
      }
      private bool SetupMapIfNeeded(bool _signals = true, bool _markets = true, bool _atms = true, bool _offices = true, bool _agencies = true, bool _parkings = true, bool _wifis = true, bool _cable = true)
      {
         if (_map != null)
         {
            showSignals = _signals;
            showMarkets = _markets;
            showATM = _atms;
            showOffices = _offices;
            showAgencies = _agencies;
            showWiFis = _wifis;
            showParkings = _parkings;
            showCables = _cable;

            _map.Clear();

            LatLng clientLocation;
            clientLocation = new LatLng(GlobalVariables.Latitud, GlobalVariables.Longitud);
            var marker = new MarkerOptions().SetPosition(clientLocation).Draggable(false);
            _map.AddMarker(marker);

            if (showSignals)
            {
               var senalamientosIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.SenalamientosIcon);
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
            }

            if (showMarkets)
            {
               var mercadosIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.MercadoIco);
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
            }

            if (showATM)
            {
               var cajeroIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.iconCajeros);
               LatLng clientLocationCajero;
               MarkerOptions markerCajero;
               foreach (var cajero in data.AllCajeros())
               {
                  clientLocationCajero = new LatLng(cajero.Latitud, cajero.Longitud);
                  markerCajero = new MarkerOptions().SetPosition(clientLocationCajero).Draggable(false).SetIcon(cajeroIcon);
                  _map.AddMarker(markerCajero);
                  clientLocationCajero = null;
                  markerCajero = null;
               }
            }

            if (showOffices)
            {
               var officeIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.iconOficinas);
               LatLng clientLocationOffice;
               MarkerOptions markerOficina;
               foreach (var oficina in data.AllOffices())
               {
                  clientLocationOffice = new LatLng(oficina.Latitud, oficina.Longitud);
                  markerOficina = new MarkerOptions().SetPosition(clientLocationOffice).Draggable(false).SetIcon(officeIcon);
                  _map.AddMarker(markerOficina);
                  clientLocationOffice = null;
                  markerOficina = null;
               }
            }

            if (showWiFis)
            {
               var wifiIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.iconWifi);
               LatLng wifiLocationOffice;
               MarkerOptions markerWiFi;
               foreach (var wifi in data.AllWifi())
               {
                  wifiLocationOffice = new LatLng(wifi.Latitud, wifi.Longitud);
                  markerWiFi = new MarkerOptions().SetPosition(wifiLocationOffice).Draggable(false).SetIcon(wifiIcon);
                  _map.AddMarker(markerWiFi);
                  wifiLocationOffice = null;
                  markerWiFi = null;
               }
            }

            if (showParkings)
            {
               var parkIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.iconEstacionamientos);
               LatLng parkLocationOffice;
               MarkerOptions markPark;
               foreach (var parking in data.AllEstacionamientos())
               {
                  parkLocationOffice = new LatLng(parking.Latitud, parking.Longitud);
                  markPark = new MarkerOptions().SetPosition(parkLocationOffice).Draggable(false).SetIcon(parkIcon);
                  _map.AddMarker(markPark);
                  parkLocationOffice = null;
                  markPark = null;
               }
            }

            if (showAgencies)
            {
               var agencyIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.iconAgencias);
               LatLng agencyLocationOffice;
               MarkerOptions markagency;
               foreach (var agencying in data.AllAgencias())
               {
                  agencyLocationOffice = new LatLng(agencying.Latitud, agencying.Longitud);
                  markagency = new MarkerOptions().SetPosition(agencyLocationOffice).Draggable(false).SetIcon(agencyIcon);
                  _map.AddMarker(markagency);
                  agencyLocationOffice = null;
                  markagency = null;
               }
            }

            //Cableado no se ha determinado como deberia de mostrarse.
            if (showCables)
            {
               List<LatLng> pointsList = new List<LatLng>();
               foreach (var cableado in data.AllCableados())
               {
                  pointsList.Clear();
                  string[] pointsArray = cableado.pointsArray.Split(';');
                  foreach (string point in pointsArray)
                  {
                     pointsList.Add(new LatLng(System.Convert.ToDouble(point.Split(',')[0].Replace(",", ""))
                       , System.Convert.ToDouble(point.Split(',')[1].Replace(",", ""))));

                  }
                  var polylineoption = new PolylineOptions();
                  polylineoption.InvokeColor(Color.Red);
                  polylineoption.Geodesic(true);

                  polylineoption.Add(pointsList.ToArray());
                  RunOnUiThread(() =>
                  _map.AddPolyline(polylineoption));
               }
            }

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
            case 1:
               SetupMapIfNeeded(_signals: !showSignals);
               break;
            case 2:
               SetupMapIfNeeded(_markets: !showMarkets);
               break;
            case 3:
               SetupMapIfNeeded(_atms: !showATM);
               break;
            case 4:
               SetupMapIfNeeded(_offices: !showOffices);
               break;
            case 5:
               SetupMapIfNeeded(_agencies: !showAgencies);
               break;
            case 6:
               SetupMapIfNeeded(_parkings: !showParkings);
               break;
            case 7:
               SetupMapIfNeeded(_wifis: !showWiFis);
               break;
            case 9:
               securityService = securityService ?? new SecurityService();
               var resultado = Task<bool>.Run(() => securityService.SendData(this));
               if (resultado.Result)
               {
                  RunOnUiThread(() =>
                  {
                     Toast.MakeText(this, "Sincronizado exitosamente.", ToastLength.Long).Show();
                  });
               }
               else
               {
                  RunOnUiThread(() =>
                  {
                     Toast.MakeText(this, "Vuelva a sincronizar.", ToastLength.Long).Show();
                  });
               }
               StartActivity(typeof(LogViewerActivity));
               break;
            case 11:
               //StartActivity(typeof(FachadasActivity));
               break;
            case 8:
               SetupMapIfNeeded(_cable: !showCables);
               break;
            default:
               break;
         }
         mDrawerLayout.CloseDrawer(mLeftDrawer);
      }

   }
}