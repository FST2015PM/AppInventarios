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
using PueblosMagicos.Android.Inventario;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "AgenciasActivity", Theme = "@style/MyTheme.ListFont", UiOptions = UiOptions.SplitActionBarWhenNarrow, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class AgenciasActivity : Activity, IOnMapReadyCallback
    {
        private ImageButton tab1Button, tab2Button, tab3Button;
        private Button agenciasMapaPopupBtn, siguienteAgenciasBtn;
        private Color selectedColor, deselectedColor;
        private ImageButton btnInicio; //

        private GoogleMap _map;
        private MapFragment _mapFragment;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Agencias);

            //MenuLateral
            //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            //mLeftDrawer.ItemClick += OnMenuLateralItemClick;


            InitMapFragment();

            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_agencias_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_agencias_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_agencias_icon);
            btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

            agenciasMapaPopupBtn = FindViewById<Button>(Resource.Id.agenciasMapaPopupBtn);
            siguienteAgenciasBtn = FindViewById<Button>(Resource.Id.siguienteAgenciasBtn);

            selectedColor = Color.ParseColor("#ffffff"); //The color u want    
            deselectedColor = Color.ParseColor("#b7ddf5");

            deselectAll();
            tab1Button.SetColorFilter(selectedColor);

            siguienteAgenciasBtn.Click += showTab2;

            siguienteAgenciasBtn.Visibility = ViewStates.Gone;

            agenciasMapaPopupBtn.Click += delegate
            {
                agenciasMapaPopupBtn.Visibility = ViewStates.Gone;
                siguienteAgenciasBtn.Visibility = ViewStates.Visible;
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

               StartActivity(typeof(AgenciasTextosActivity));
            };
        }

        private bool SetupMapIfNeeded()
        {
            if (_map != null)
            {
                LatLng clientLocation;
                clientLocation = new LatLng(GlobalVariables.LatitudAgencias, GlobalVariables.LongitudAgencias);
                var marker = new MarkerOptions().SetPosition(clientLocation).Draggable(true);
                _map.AddMarker(marker);
                _map.MarkerDragEnd += (s, e) =>
                {
                    GlobalVariables.LatitudAgencias = e.Marker.Position.Latitude;
                    GlobalVariables.LongitudAgencias = e.Marker.Position.Longitude;
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
                fragTx.Add(Resource.Id.mapAgencias, _mapFragment, "map");
                fragTx.Commit();
            }
            _mapFragment.GetMapAsync(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            deselectAll();
            tab1Button.SetColorFilter(selectedColor);
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
        }

        void showTab2(object sender, EventArgs e)
        {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            StartActivity(typeof(AgenciasHorariosActivity));
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
