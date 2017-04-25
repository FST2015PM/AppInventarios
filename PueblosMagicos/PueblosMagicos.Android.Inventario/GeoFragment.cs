
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
using Android.Gms.Maps.Model;

namespace PueblosMagicos.Android.Inventario
{
    public class GeoFragment : Fragment, IOnMapReadyCallback
    {
        private GoogleMap googleMap;
        private MapView mapView;
        private MapFragment _mapFragment;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static GeoFragment NewInstance()
        {
            var fragment = new GeoFragment { Arguments = new Bundle() };
            return fragment;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.geolocalizacion_fragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
           mapView = (MapView)view.FindViewById(Resource.Id.map);
           mapView.OnCreate(savedInstanceState);
           mapView.OnResume();
           mapView.GetMapAsync(this);
        }


        void IOnMapReadyCallback.OnMapReady(GoogleMap map)
        {
           googleMap = map;

           LatLng clientLocation = new LatLng(GlobalVariables.Latitud, GlobalVariables.Longitud);
           googleMap.AddMarker(new MarkerOptions().SetPosition(clientLocation).SetTitle("Estas aqui."));

           // We create an instance of CameraUpdate, and move the map to it.
           CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(clientLocation, 15);
           googleMap.MoveCamera(cameraUpdate);

        }
    }
}