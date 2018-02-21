using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.Threading.Tasks;
using System.Threading;
using Android.Locations;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Provider;
using PueblosMagicos.Android.Inventario.Services;
using Android.Views;
using Java.IO;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using System.Collections.Generic;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(MainLauncher = true, Label = "PueblosMagicos.Inventario.Android")]
   public class MainActivity : Activity, ILocationListener
   {
      DataService dataService;
      SecurityService security = new SecurityService();
      LocationManager locMgr;
      public EditText editTextCorreo;
      public EditText editTextNip;
      TextView errorLegendMain;
      private File _dir;
      private File _file;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         dataService = new DataService();
         dataService.InitializeDB();

         RequestWindowFeature(WindowFeatures.NoTitle);

         if (locMgr == null)
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

         if (locMgr.IsProviderEnabled(LocationManager.GpsProvider) && 
            locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
         && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
         {
            locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 50, 1, this);
            return;
         }
         else
         {
            showGPSDisabledAlertToUser();
            return;
         }

      }
      private void showGPSDisabledAlertToUser()
      {
         AlertDialog.Builder builder = new AlertDialog.Builder(this);
         AlertDialog alertDialog = builder.Create();

         alertDialog.SetTitle("Pueblos Magicos");
         alertDialog.SetMessage("Active su GPS para continuar.");

         alertDialog.SetButton("OK", (s, ev) =>
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
      protected override void OnPause()
      {
         base.OnPause();
         locMgr.RemoveUpdates(this);
      }

      protected override void OnResume()
      {
         base.OnResume();
         locMgr = GetSystemService(Context.LocationService) as LocationManager;
      }

      void OnLoginClick(object sender, EventArgs e)
      {

         var session = Task.Run(() => security.ValidLogin(editTextCorreo.Text, editTextNip.Text));
         if (!(session.Result))
         {
            errorLegendMain.Text = "Error de login";
            return;
         }
         dataService.SaveData(GlobalVariables.LoggedSession);
         // TODO
         StartActivity(typeof(SeleccionarModuloActivity));
      }
      void OnOlvideNipClick(object sender, EventArgs e)
      {
         // TODO
         var intent = new Intent(this, typeof(RecuperarNipActivity));
         StartActivity(intent);
      }
      public void OnProviderDisabled(string provider)
      {
         //Log.Debug(tag, provider + " disabled by user");
      }
      public void OnProviderEnabled(string provider)
      {
         //Log.Debug(tag, provider + " enabled by user");
      }
      public void OnStatusChanged(string provider, Availability status, Bundle extras)
      {
         //Log.Debug(tag, provider + " availability has changed to " + status.ToString());
      }
      public async void OnLocationChanged(Location location)
      {
         GlobalVariables.Latitud = location.Latitude;
         GlobalVariables.Longitud = location.Longitude;

         GlobalVariables.LatitudSenalamiento = location.Latitude;
         GlobalVariables.LongitudSenalamiento = location.Longitude;

         GlobalVariables.LatitudMercado = location.Latitude;
         GlobalVariables.LongitudMercado = location.Longitude;

         GlobalVariables.LatitudCajero = location.Latitude;
         GlobalVariables.LongitudCajero = location.Longitude;

         GlobalVariables.LatitudOficina = location.Latitude;
         GlobalVariables.LongitudOficina = location.Longitude;

         GlobalVariables.LatitudEstacionamiento = location.Latitude;
         GlobalVariables.LongitudEstacionamiento = location.Longitude;

         GlobalVariables.LatitudFachada = location.Latitude;
         GlobalVariables.LongitudFachada = location.Longitude;

         GlobalVariables.LatitudWifi = location.Latitude;
         GlobalVariables.LongitudWifi = location.Longitude;

         GlobalVariables.LatitudAgencias = location.Latitude;
         GlobalVariables.LongitudAgencias = location.Longitude;

         GlobalVariables.LatitudCableado = location.Latitude;
         GlobalVariables.LongitudCableado = location.Longitude;

         GlobalVariables.LatitudCoordinates = LocationConverter.getLatitudeAsDMS(location.Latitude, 2);
         GlobalVariables.LongitudCoordinates = LocationConverter.getLongitudeAsDMS(location.Longitude, 2);

         _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosSignal");
         if (!_dir.Exists())
         {
            _dir.Mkdirs();
         }

         _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosMarket");
         if (!_dir.Exists())
         {
            _dir.Mkdirs();
         }

         _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosOffice");
         if (!_dir.Exists())
         {
            _dir.Mkdirs();
         }

         _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosParking");
         if (!_dir.Exists())
         {
            _dir.Mkdirs();
         }

         _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosFacade");
         if (!_dir.Exists())
         {
            _dir.Mkdirs();
         }

         if (dataService.EsSesionExistente())
         {
            StartActivity(typeof(MenuHomeActivity));
         }
         else
         {
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.loginButton).Click += OnLoginClick;
            FindViewById<TextView>(Resource.Id.textOlvideNIP).Click += OnOlvideNipClick;
            editTextCorreo = (EditText)FindViewById(Resource.Id.inputCorreo);
            editTextNip = (EditText)FindViewById(Resource.Id.inputNIP);
            errorLegendMain = FindViewById<TextView>(Resource.Id.ErrorLegendMain);
         }
      }

   }
}
