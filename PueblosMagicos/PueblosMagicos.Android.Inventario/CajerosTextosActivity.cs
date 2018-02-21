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
      private ImageButton tab1Button, tab2Button, btnInicio;
      private Button buttonFinalizar;
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
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_cajeros_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_cajeros_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //
         buttonFinalizar = this.FindViewById<Button>(Resource.Id.btnFinalizarCajeros);
         textViewBancosCont = this.FindViewById<TextView>(Resource.Id.textViewBancosCont);
         editTextCajasBanco = this.FindViewById<EditText>(Resource.Id.editTextCajasBanco);
         editTextCajasNoCajeros = this.FindViewById<EditText>(Resource.Id.editTextCajasNoCajeros);
         switchCajasServicio = this.FindViewById<Switch>(Resource.Id.switchCajasServicio);

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#f0aad0");

         editTextCajasBanco.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextCajasNoCajeros.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;

         editTextCajasBanco.Text = GlobalVariables.cajerosTxtBanco;
         editTextCajasNoCajeros.Text = GlobalVariables.cajerosTxtNoCajeros;
         switchCajasServicio.Checked = GlobalVariables.cajerosEnServicio;

         deselectAll();
         tab2Button.SetColorFilter(selectedColor);
         buttonFinalizar.Visibility = ViewStates.Gone; //
         activaBtnGuardar(); //
         buttonFinalizar.Click += showBtnFinalizar;

         //Contar el número de carácteres del EditText
         editTextCajasBanco.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextCajasBanco.Length();
            textViewBancosCont.Text = numChar.ToString();
            GlobalVariables.cajerosTxtBanco = editTextCajasBanco.Text;
            activaBtnGuardar();
         };

         editTextCajasNoCajeros.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.cajerosTxtNoCajeros = editTextCajasNoCajeros.Text;
            activaBtnGuardar();
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

         btnInicio.Click += delegate //
         {
            var intent = new Intent(this, typeof(MenuHomeActivity));
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
         int numCharBanco = 300 - editTextCajasBanco.Length();
         int numCharCajeros = 300 - editTextCajasNoCajeros.Length();

         if ((numCharBanco < 300) && (numCharCajeros < 300))
            buttonFinalizar.Visibility = ViewStates.Visible;
         else buttonFinalizar.Visibility = ViewStates.Gone;

      }

      private void showBtnFinalizar(object sender, EventArgs e)
      {

         Cajeros cajero = new Cajeros()
         {
            Id = Guid.NewGuid().ToString(),
            Latitud = GlobalVariables.LatitudCajero,
            Longitud = GlobalVariables.LongitudCajero,
            Bank = GlobalVariables.cajerosTxtBanco,
            atmUnits = GlobalVariables.cajerosTxtNoCajeros,
            inService = GlobalVariables.cajerosEnServicio,
            Enviado = ""
         };
         data.SaveData(cajero);

         GlobalVariables.cajeroNuevaCaptura = true;
         Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
         var intent = new Intent(this, typeof(MenuHomeActivity));
         StartActivity(intent);
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