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

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "Mapa-Estacionamientos", ParentActivity = typeof(SeleccionarModuloActivity))]
    public class EstacionamientosActivity : Activity, IOnMapReadyCallback
    {
        private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
        private Button SenalMapaPopupBtn, SenalMapaSigBtn;
        private Color selectedColor, deselectedColor;

        private GoogleMap _map;
        private MapFragment _mapFragment;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;
        private ImageButton btnInicio; //

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Estacionamientos);

            //MenuLateral
            //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

            InitMapFragment();

            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_estacionamientos_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_estacionamientos_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_estacionamientos_icon);
            tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_estacionamientos_icon);
            SenalMapaPopupBtn = FindViewById<Button>(Resource.Id.estacionamientosMapaPopupBtn);
            SenalMapaSigBtn = FindViewById<Button>(Resource.Id.siguienteEstacionamientosBtn);
            btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

            selectedColor = Color.ParseColor("#ffffff"); //The color u want    
            deselectedColor = Color.ParseColor("#e9b7a0");

            deselectAll();
            tab1Button.SetColorFilter(selectedColor);

            SenalMapaSigBtn.Click += showTab2;
            SenalMapaSigBtn.Visibility = ViewStates.Gone;
            SenalMapaPopupBtn.Click += delegate
            {
                SenalMapaPopupBtn.Visibility = ViewStates.Gone;
                SenalMapaSigBtn.Visibility = ViewStates.Visible;
            };

            btnInicio.Click += delegate //
            {
                var intent = new Intent(this, typeof(MenuHomeActivity));
                StartActivity(intent);
            }; //

            tab2Button.Click += showTab2;

            tab3Button.Click += delegate
            {
                deselectAll();
                tab3Button.SetColorFilter(selectedColor);

                StartActivity(typeof(EstacionamientosHorariosActivity));
            };

            tab4Button.Click += delegate
            {
                deselectAll();
                tab4Button.SetColorFilter(selectedColor);

                StartActivity(typeof(EstacionamientosTextosActivity));
            };

        }

        private bool SetupMapIfNeeded()
        {
            if (_map != null)
            {
                LatLng clientLocation;
                clientLocation = new LatLng(GlobalVariables.LatitudEstacionamiento, GlobalVariables.LongitudEstacionamiento);
                var marker = new MarkerOptions().SetPosition(clientLocation).Draggable(true);
                _map.AddMarker(marker);
                _map.MarkerDragEnd += (s, e) =>
                {
                    GlobalVariables.LatitudEstacionamiento = e.Marker.Position.Latitude;
                    GlobalVariables.LongitudEstacionamiento = e.Marker.Position.Longitude;
                };
                // We create an instance of CameraUpdate, and move the map to it.
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
                fragTx.Add(Resource.Id.mapEstacionamientos, _mapFragment, "map");
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

            StartActivity(typeof(EstacionamientosFotosActivity));
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
                    //StartActivity(typeof(CableadosActivity));
                    break;
            }
            //mDrawerLayout.CloseDrawer(mLeftDrawer);
        }

    }
}
