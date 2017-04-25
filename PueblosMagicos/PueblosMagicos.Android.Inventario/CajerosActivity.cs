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

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "CajerosActivity", Theme = "@style/MyTheme.ListFont", UiOptions = UiOptions.SplitActionBarWhenNarrow, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class CajerosActivity : Activity, IOnMapReadyCallback
    {
        private ImageButton tab1Button, tab2Button;
        private Button cajerosMapaPopupBtn, SiguienteMapaBtn;
        private Color selectedColor, deselectedColor;

        private GoogleMap _map;
        private MapFragment _mapFragment;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Cajeros);

            //MenuLateral
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            mLeftDrawer.ItemClick += OnMenuLateralItemClick;


            InitMapFragment();

            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_cajeros_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_cajeros_icon);

            cajerosMapaPopupBtn = FindViewById<Button>(Resource.Id.cajerosMapaPopupBtn);
            SiguienteMapaBtn = FindViewById<Button>(Resource.Id.siguienteMapaBtn);

            selectedColor = Color.ParseColor("#303030"); //The color u want    
            deselectedColor = Color.ParseColor("#ffffff");

            deselectAll();
            tab1Button.SetColorFilter(selectedColor);

            SiguienteMapaBtn.Click += showTab2;

            cajerosMapaPopupBtn.Click += delegate
            {
                cajerosMapaPopupBtn.Visibility = ViewStates.Gone;
            };

            tab2Button.Click += delegate
            {
                deselectAll();
                tab2Button.SetColorFilter(selectedColor);

                StartActivity(typeof(CajerosTextosActivity));
            };
        }

        private bool SetupMapIfNeeded()
        {
            if (_map != null)
            {
                LatLng clientLocation;
                clientLocation = new LatLng(GlobalVariables.LatitudCajero, GlobalVariables.LongitudCajero);
                var marker = new MarkerOptions().SetPosition(clientLocation).Draggable(true);
                _map.AddMarker(marker);
                _map.MarkerDragEnd += (s, e) =>
                {
                    GlobalVariables.LatitudCajero = e.Marker.Position.Latitude;
                    GlobalVariables.LongitudCajero = e.Marker.Position.Longitude;
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
                fragTx.Add(Resource.Id.mapCajeros, _mapFragment, "map");
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
        }

        void showTab2(object sender, EventArgs e)
        {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            StartActivity(typeof(CajerosTextosActivity));
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
