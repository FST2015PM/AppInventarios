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
      private ImageButton tab1Button, tab2Button, tab3Button, btnInicio; //
      private Button buttonFinalizar;
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
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_oficinas_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_oficinas_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_oficinas_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //
         buttonFinalizar = FindViewById<Button>(Resource.Id.btnFinalizarOficinas);
         textViewOficinaNombreCont = this.FindViewById<TextView>(Resource.Id.textViewOficinaNombreCont);
         textViewOficinaContactoCont = this.FindViewById<TextView>(Resource.Id.textViewOficinaContactoCont);
         editTextOficinaNombre = this.FindViewById<EditText>(Resource.Id.editTextOficinaNombre);
         editTextOficinaNoSalas = this.FindViewById<EditText>(Resource.Id.editTextOficinaNoSalas);
         editTextOficinaAforo = this.FindViewById<EditText>(Resource.Id.editTextOficinaAforo);
         editTextOficinaContacto = this.FindViewById<EditText>(Resource.Id.editTextOficinaContacto);

         editTextOficinaNombre.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextOficinaNoSalas.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextOficinaAforo.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextOficinaContacto.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;

         editTextOficinaNombre.Text = GlobalVariables.oficinaTxtNombre;
         editTextOficinaNoSalas.Text = GlobalVariables.oficinaTxtNoSalas;
         editTextOficinaAforo.Text = GlobalVariables.oficinaTxtAforo;
         editTextOficinaContacto.Text = GlobalVariables.oficinaTxtContacto;

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#ca9def");

         deselectAll();
         tab3Button.SetColorFilter(selectedColor);

         buttonFinalizar.Visibility = ViewStates.Gone;
         activaBtnGuardar();

         buttonFinalizar.Click += showBtnFinalizar;

         btnInicio.Click += delegate //
            {
               var intent = new Intent(this, typeof(MenuHomeActivity));
               StartActivity(intent);
            };
         //Contar el número de carácteres del EditText
         editTextOficinaNombre.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextOficinaNombre.Length();
            textViewOficinaNombreCont.Text = numChar.ToString();
            GlobalVariables.oficinaTxtNombre = editTextOficinaNombre.Text;
            activaBtnGuardar();
         };

         editTextOficinaNoSalas.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.oficinaTxtNoSalas = editTextOficinaNoSalas.Text;
            activaBtnGuardar();
         };

         editTextOficinaAforo.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.oficinaTxtAforo = editTextOficinaAforo.Text;
            activaBtnGuardar();
         };

         editTextOficinaContacto.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextOficinaContacto.Length();
            textViewOficinaContactoCont.Text = numChar.ToString();
            GlobalVariables.oficinaTxtContacto = editTextOficinaContacto.Text;
            activaBtnGuardar();
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

      private void activaBtnGuardar() //
      {

         int numCharNombre = 300 - editTextOficinaNombre.Length();
         int numCharSalas = 300 - editTextOficinaNoSalas.Length();
         int numCharAforo = 300 - editTextOficinaAforo.Length();
         int numCharContacto = 300 - editTextOficinaContacto.Length();

         if ((numCharNombre < 300) && (numCharSalas < 300) && (numCharAforo < 300) && (numCharContacto < 300))
            buttonFinalizar.Visibility = ViewStates.Visible;
         else buttonFinalizar.Visibility = ViewStates.Gone;
      }

      private void showBtnFinalizar(object sender, EventArgs e)
      {

         Oficinas oficina = new Oficinas()
         {
            Id = Guid.NewGuid().ToString(),
            Latitud = GlobalVariables.LatitudOficina,
            Longitud = GlobalVariables.LongitudOficina,
            Name = GlobalVariables.oficinaTxtNombre,
            Contact = GlobalVariables.oficinaTxtContacto,
            Aforo = GlobalVariables.oficinaTxtAforo,
            FotoName = GlobalVariables.OfficesPhotoName,
            Enviado = ""
         };
         data.SaveData(oficina);

         GlobalVariables.oficinaNuevaCaptura = true;
         Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
         var intent = new Intent(this, typeof(MenuHomeActivity));
         StartActivity(intent);
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

         int numCharContacto = 300 - editTextOficinaContacto.Length();
         textViewOficinaContactoCont.Text = numCharContacto.ToString();
         GlobalVariables.oficinaTxtContacto = editTextOficinaContacto.Text;

         activaBtnGuardar();
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