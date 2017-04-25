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
    [Activity(Label = "OficinasTextosActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class OficinasTextosActivity : Activity
    {
        DataService data;
        private ImageButton tab1Button, tab2Button, tab3Button;
        private ImageView buttonFinalizar;
        private TextView textViewOficinaNombreCont, textViewOficinaContactoCont;
        private EditText editTextOficinaNombre, editTextOficinaNoSalas, editTextOficinaAforo, editTextOficinaContacto;
        private Color selectedColor, deselectedColor;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.OficinasTextos);
            data = new DataService();

            //MenuLateral
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

            mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            mLeftDrawer.ItemClick += OnMenuLateralItemClick;

            // Create your application here
            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_oficinas_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_oficinas_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_oficinas_icon);
            buttonFinalizar = this.FindViewById<ImageButton>(Resource.Id.btnFinalizarOficinas);
            textViewOficinaNombreCont = this.FindViewById<TextView>(Resource.Id.textViewOficinaNombreCont);
            textViewOficinaContactoCont = this.FindViewById<TextView>(Resource.Id.textViewOficinaContactoCont);
            editTextOficinaNombre = this.FindViewById<EditText>(Resource.Id.editTextOficinaNombre);
            editTextOficinaNoSalas = this.FindViewById<EditText>(Resource.Id.editTextOficinaNoSalas);
            editTextOficinaAforo = this.FindViewById<EditText>(Resource.Id.editTextOficinaAforo);
            editTextOficinaContacto = this.FindViewById<EditText>(Resource.Id.editTextOficinaContacto);

            editTextOficinaNombre.Text = GlobalVariables.oficinaTxtNombre;
            editTextOficinaNoSalas.Text = GlobalVariables.oficinaTxtNoSalas;
            editTextOficinaAforo.Text = GlobalVariables.oficinaTxtAforo;
            editTextOficinaContacto.Text = GlobalVariables.oficinaTxtContacto;

            selectedColor = Color.ParseColor("#303030"); //The color u want    
            deselectedColor = Color.ParseColor("#ffffff");

            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            buttonFinalizar.Click += showBtnFinalizar;

            //Contar el número de carácteres del EditText
            editTextOficinaNombre.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                int numChar = 300 - editTextOficinaNombre.Length();
                textViewOficinaNombreCont.Text = numChar.ToString();
                GlobalVariables.oficinaTxtNombre = editTextOficinaNombre.Text;
            };

            editTextOficinaNoSalas.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                GlobalVariables.oficinaTxtNoSalas = editTextOficinaNoSalas.Text;
            };

            editTextOficinaAforo.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                GlobalVariables.oficinaTxtAforo = editTextOficinaAforo.Text;
            };

            editTextOficinaContacto.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                int numChar = 300 - editTextOficinaNombre.Length();
                textViewOficinaContactoCont.Text = numChar.ToString();
                GlobalVariables.oficinaTxtContacto = editTextOficinaContacto.Text;
            };

            tab1Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);
                var intent = new Intent(this, typeof(OficinasActivity));
                StartActivity(intent);

            };

            tab2Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);
                var intent = new Intent(this, typeof(OficinasFotosActivity));
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

            Oficinas oficina = new Oficinas()
            {
                Latitud = GlobalVariables.LatitudOficina,
                Longitud = GlobalVariables.LongitudOficina,
                Name = GlobalVariables.oficinaTxtNombre,
                Contact = GlobalVariables.oficinaTxtContacto,
                Manager = ""
            };
            data.SaveData(oficina);

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
            tab3Button.SetColorFilter(selectedColor);

            int numChar = 300 - editTextOficinaNombre.Length();
            textViewOficinaNombreCont.Text = numChar.ToString();
            GlobalVariables.oficinaTxtNombre = editTextOficinaNombre.Text;

            GlobalVariables.oficinaTxtNoSalas = editTextOficinaNoSalas.Text;
            GlobalVariables.oficinaTxtAforo = editTextOficinaAforo.Text;

            int numCharContacto = 300 - editTextOficinaNombre.Length();
            textViewOficinaContactoCont.Text = numCharContacto.ToString();
            GlobalVariables.oficinaTxtContacto = editTextOficinaContacto.Text;
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