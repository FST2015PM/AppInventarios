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
using Android.Support.V4.Widget;
using Android.Content.PM;
using Android.Text;
using PueblosMagicos.Android.Inventario.Services;
using PueblosMagicos.Android.Inventario.DataTables;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "CajerosTextosActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class CajerosTextosActivity : Activity
    {

        DataService data;
        private ImageButton tab1Button, tab2Button;
        private ImageView buttonFinalizar;
        private TextView textViewBancosCont;
        private EditText editTextCajasBanco, editTextCajasNoCajeros;
        private Switch switchCajasServicio;
        private Color selectedColor, deselectedColor;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.CajerosTextos);
            data = new DataService();

            //MenuLateral
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

            mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            mLeftDrawer.ItemClick += OnMenuLateralItemClick;

            // Create your application here
            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_cajeros_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_cajeros_icon);
            buttonFinalizar = this.FindViewById<ImageButton>(Resource.Id.btnFinalizarCajeros);
            textViewBancosCont = this.FindViewById<TextView>(Resource.Id.textViewBancosCont);
            editTextCajasBanco = this.FindViewById<EditText>(Resource.Id.editTextCajasBanco);
            editTextCajasNoCajeros = this.FindViewById<EditText>(Resource.Id.editTextCajasNoCajeros);
            switchCajasServicio = this.FindViewById<Switch>(Resource.Id.switchCajasServicio);

            selectedColor = Color.ParseColor("#303030"); //The color u want    
            deselectedColor = Color.ParseColor("#ffffff");

            editTextCajasBanco.Text = GlobalVariables.cajerosTxtBanco;
            editTextCajasNoCajeros.Text = GlobalVariables.cajerosTxtNoCajeros;
            switchCajasServicio.Checked = GlobalVariables.cajerosEnServicio;

            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            buttonFinalizar.Click += showBtnFinalizar;

            //Contar el número de carácteres del EditText
            editTextCajasBanco.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                int numChar = 300 - editTextCajasBanco.Length();
                textViewBancosCont.Text = numChar.ToString();
                GlobalVariables.cajerosTxtBanco = editTextCajasBanco.Text;
            };

            editTextCajasNoCajeros.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                GlobalVariables.cajerosTxtNoCajeros = editTextCajasNoCajeros.Text;
            };

            switchCajasServicio.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                GlobalVariables.cajerosEnServicio = e.IsChecked;
            };

            tab1Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);
                var intent = new Intent(this, typeof(CajerosActivity));
                StartActivity(intent);

            };
        }

        private void deselectAll()
        {
            tab1Button.SetColorFilter(deselectedColor);
            tab2Button.SetColorFilter(deselectedColor);
        }

        private void showBtnFinalizar(object sender, EventArgs e)
        {

            Cajeros cajero = new Cajeros()
            {
                Latitud = GlobalVariables.LatitudCajero,
                Longitud = GlobalVariables.LongitudCajero,
                Bank = GlobalVariables.cajerosTxtBanco,
                atmUnits = GlobalVariables.cajerosTxtNoCajeros,
                inService = GlobalVariables.cajerosEnServicio,
            };

            data.SaveData(cajero);

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alertDialog = builder.Create();

            //title
            alertDialog.SetTitle("Pueblos Magicos");

            //icon
            //alertDialog.SetIcon(Resource.Drawable.Icon);

            //question or message
            alertDialog.SetMessage("¿Desea sincronizar? ");

            //Buttons
            alertDialog.SetButton("No", (s, ev) =>
            {
                //Do something
                // Toast.MakeText(this, "Error", ToastLength.Long).Show();
            });

            alertDialog.SetButton2("Si", (s, ev) =>
            {
                //Do something
                Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
            });

            alertDialog.Show();
        }

        protected override void OnResume()
        {
            base.OnResume();
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            int numChar = 300 - editTextCajasBanco.Length();
            textViewBancosCont.Text = numChar.ToString();
            GlobalVariables.cajerosTxtBanco = editTextCajasBanco.Text;
            GlobalVariables.cajerosTxtNoCajeros = editTextCajasNoCajeros.Text;
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