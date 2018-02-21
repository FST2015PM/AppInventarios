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
using Android.Graphics;
using Android.Hardware;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Support.V4.App;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "OrientacionActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
   public class SenalamientosOrientacionActivity : Activity, ISensorEventListener
   {
      SensorManager sensorMgr;
      Sensor accelerometer;
      Sensor magnetometer;
      TextView txtBrujulaGrados;
      TextView txtBrujulaOrientacion;
      TextView txtLatGrados;
      TextView txtLongGrados;

      List<float> gravity = new List<float>();
      List<float> magnet = new List<float>();

      private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
      private Button SenalOrientSigBtn;
      private ImageButton SenalOrientPopupBtn;
      private Color selectedColor, deselectedColor;
      private ImageButton btnInicio; //
      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.SenalamientosOrientacion);

         //MenuLateral
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
         
         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         sensorMgr = (SensorManager)GetSystemService(Context.SensorService);
         accelerometer = sensorMgr.GetDefaultSensor(SensorType.Accelerometer);
         magnetometer = sensorMgr.GetDefaultSensor(SensorType.MagneticField);

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_icon);
         tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_icon);
         SenalOrientPopupBtn = FindViewById<ImageButton>(Resource.Id.SenalOrientPopupBtn);
         SenalOrientSigBtn = FindViewById<Button>(Resource.Id.SenalOrientSigBtn);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

         //Controles de ubicacion.
         txtBrujulaGrados = this.FindViewById<TextView>(Resource.Id.txtBrujulaGrados);
         txtBrujulaOrientacion = this.FindViewById<TextView>(Resource.Id.txtBrujulaOrientacion);
         txtLatGrados = this.FindViewById<TextView>(Resource.Id.txtLatGrados);
         txtLongGrados = this.FindViewById<TextView>(Resource.Id.txtLongGrados);
         txtLatGrados.Text = GlobalVariables.LatitudCoordinates;
         txtLongGrados.Text = GlobalVariables.LongitudCoordinates;

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#96c88e");

         deselectAll();
         tab3Button.SetColorFilter(selectedColor);

         //SenalOrientSigBtn.Visibility = ViewStates.Gone;

         SenalOrientPopupBtn.Click += delegate
         {
            SenalOrientPopupBtn.Visibility = ViewStates.Gone;
         };

         btnInicio.Click += delegate //
         {
             var intent = new Intent(this, typeof(MenuHomeActivity));
             StartActivity(intent);
         }; //

         SenalOrientSigBtn.Click += showTab4;

         tab1Button.Click += delegate
         {
            deselectAll();
            tab1Button.SetColorFilter(selectedColor);

            StartActivity(typeof(SenalamientosMapActivity));
         };

         tab2Button.Click += delegate
         {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(SenalamientosFotosActivity));
            StartActivity(intent);
         };

         tab4Button.Click += showTab4;
      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);
         tab4Button.SetColorFilter(deselectedColor);
      }

      private void showTab4(object sender, EventArgs e)
      {
         deselectAll();
         tab4Button.SetColorFilter(selectedColor);

         var intent = new Intent(this, typeof(SenalamientosTextosActivity));
         StartActivity(intent);
      }

      public override void OnBackPressed()
      {
         Finish();
      }

      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab3Button.SetColorFilter(selectedColor);

         if (magnetometer != null && accelerometer != null)
         {
            sensorMgr.RegisterListener(this, accelerometer, SensorDelay.Ui);
            sensorMgr.RegisterListener(this, magnetometer, SensorDelay.Ui);
         }
         else
         {
            //Doesn't have required sensors
         }
      }

      protected override void OnPause()
      {
         base.OnPause();
         sensorMgr.UnregisterListener(this);
      }

      public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
      {

      }

      float azimuth;
      public void OnSensorChanged(SensorEvent e)
      {

         if (e.Sensor.Type == SensorType.MagneticField)
         {
            magnet.Clear();
            magnet.AddRange(e.Values);
            float degree = (float)Math.Round(e.Values[0]);
            txtBrujulaGrados.Text = Convert.ToString(degree) + "\u00B0";
            txtBrujulaOrientacion.Text = CalculateDirection(degree);
         }
         if (e.Sensor.Type == SensorType.Accelerometer)
         {
            gravity.Clear();
            gravity.AddRange(e.Values);
         }

         if (magnet.Count > 0 && gravity.Count > 0)
         {
            float[] R = new float[9];
            float[] I = new float[9];
            bool worked = SensorManager.GetRotationMatrix(R, I, gravity.ToArray(), magnet.ToArray());

            if (worked)
            {
               float[] orientation = new float[3];
               SensorManager.GetOrientation(R, orientation);

               //Temp compass for demo purposes
               ImageView compassImageView = FindViewById<ImageView>(Resource.Id.compass);

               azimuth = lowPassFilter(orientation[0] * 180 / (float)Math.PI, azimuth);
               compassImageView.Rotation = -azimuth; //Points in magnetic north.
            }
         }
      }

      //Controls amount of smoothing.  0 < a < 1.  Smaller a, more smoothing but slower response
      const float ALPHA = 0.07f;
      private float lowPassFilter(float newInput, float output)
      {
         return (float)Math.Round(output + ALPHA * (newInput - output), 1);
      }

      private string CalculateDirection(float degree)
      {
         if (degree < 54 || degree > 322)
            return "NE";
         if (degree > 235 && degree < 322)
            return "NW";
         if (degree > 148 && degree < 235)
            return "SW";
         if (degree > 54 && degree < 148)
            return "SE";
         if (degree == 322)
            return "N";
         if (degree == 235)
            return "W";
         if (degree == 54)
            return "E";
         if (degree == 148)
            return "S";

         return "NE"; //Default :)

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