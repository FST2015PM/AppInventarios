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
using Android.Gms.Maps.Model;
using System.IO;
using PueblosMagicos.Inventario.Android;
using PueblosMagicos.Android.Inventario;

namespace PueblosMagicos.Inventario.Android
{
    [Activity(Label = "MapActivity", ParentActivity = typeof(MenuHomeActivity))]
    public class MapActivity : Activity, IOnMapReadyCallback
    {
        private static readonly LatLng Passchendaele = new LatLng(50.897778, 3.013333);
        private static readonly LatLng VimyRidge = new LatLng(50.379444, 2.773611);
        private GoogleMap _map;
        private MapFragment _mapFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //ActionBar.SetTitle(Resource.String.ApplicationName);

            SetContentView(Resource.Layout.geolocalizacion_fragment);

            InitMapFragment();
            // Create your application here
        }
        protected override void OnResume()
        {
            base.OnResume();
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
                    .InvokeCompassEnabled(false);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map2, _mapFragment, "map");
                fragTx.Commit();
            }
            _mapFragment.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap map)
        {

        }

        private void SetupMapIfNeeded()
        {
            if (_map == null)
            {
                //_map = _mapFragment.Map;
                if (_map != null)
                {
                    //var databasePath = Path.Combine(FilesDir.AbsolutePath, "nfcinsurance.sqlite");
                    //LatLng clientLocation;
                    //using (var conn = new SQLite.SQLiteConnection(databasePath))
                    //{
                    //    var previousRow = conn.Table<NFCInsuranceData>().OrderByDescending(o => o.Id).FirstOrDefault(f => f.Id > 0);
                    //    clientLocation = new LatLng(System.Convert.ToDouble(previousRow.Latitud), System.Convert.ToDouble(previousRow.Longitud));
                    //    _map.AddMarker(new MarkerOptions().SetPosition(clientLocation).SetTitle("Estas aqui."));

                    //    // We create an instance of CameraUpdate, and move the map to it.
                    //    CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(clientLocation, 15);
                    //    _map.MoveCamera(cameraUpdate);
                    //    conn.Close();
                    //}
                }
            }
        }
    }
}