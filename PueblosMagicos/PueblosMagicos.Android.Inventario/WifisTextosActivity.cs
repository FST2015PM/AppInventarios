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
using AndroidUtil = Android.Util;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "WifiTextosActivity", Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
   public class WifisTextosActivity : Activity
   {
      DataService data;
      private ImageButton tab1Button, tab2Button, btnInicio; //
      private Button buttonFinalizar, buttonRegresar;
      private TextView textViewWifiProveedorCont, textViewWifiVelocidadDeSubida, textViewWifiVelocidadDeBajada;
      private EditText editTextWifiProveedor, editTextWifiVelocidadDeSubida, editTextWifiVelocidadDeBajada;
      private Switch switchWifiFunciona;
      private LinearLayout linearWifiTxtFinalizarBtn, linearTextViewProveedor,
          linearTxtSpace, linearTxtSpaceSwitch2, linearTxtSpaceSwitch3, linearTxtSpaceSwitch4,
          linearWifiTxtRegresarBtn, linearTextViewVelocidadDeSubida, linearTextViewVelocidadDeBajada,
          linearSwitchFunciona;
      private FrameLayout containerWifiTxtList, containerWifiTxtSubList;
      private Color selectedColor, deselectedColor;
      private ListView list, subList;
      private AndroidUtil.SparseBooleanArray sparseArray;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.WifiTextos);
         data = new DataService();

         //MenuLateral
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_Wifi_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_Wifi_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

         buttonFinalizar = FindViewById<Button>(Resource.Id.btnFinalizarWifi);
         buttonRegresar = FindViewById<Button>(Resource.Id.btnRegresarWifi);

         linearWifiTxtFinalizarBtn = FindViewById<LinearLayout>(Resource.Id.linearWifiTxtFinalizarBtn);
         linearTextViewProveedor = FindViewById<LinearLayout>(Resource.Id.linearTextViewProveedor);


         linearSwitchFunciona = FindViewById<LinearLayout>(Resource.Id.linearSwitchFunciona);

         linearTxtSpaceSwitch2 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch2);
         linearTxtSpaceSwitch3 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch3);
         linearTxtSpaceSwitch4 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch4);
         linearTextViewProveedor = FindViewById<LinearLayout>(Resource.Id.linearTextViewProveedor);
         linearTextViewVelocidadDeSubida = FindViewById<LinearLayout>(Resource.Id.linearTextViewVelocidadDeSubida);
         linearTextViewVelocidadDeBajada = FindViewById<LinearLayout>(Resource.Id.linearTextViewVelocidadDeBajada);

         containerWifiTxtList = FindViewById<FrameLayout>(Resource.Id.containerWifiTxtList);
         linearWifiTxtRegresarBtn = FindViewById<LinearLayout>(Resource.Id.linearWifiTxtRegresarBtn);
         containerWifiTxtSubList = FindViewById<FrameLayout>(Resource.Id.containerWifiTxtSubList);

         textViewWifiProveedorCont = this.FindViewById<TextView>(Resource.Id.textViewWifiProveedorCont);

         editTextWifiProveedor = this.FindViewById<EditText>(Resource.Id.editTextWifiProveedor);
         editTextWifiVelocidadDeSubida = this.FindViewById<EditText>(Resource.Id.editTextWifiVelocidadDeSubida);
         editTextWifiVelocidadDeBajada = this.FindViewById<EditText>(Resource.Id.editTextWifiVelocidadDeBajada);

         switchWifiFunciona = this.FindViewById<Switch>(Resource.Id.switchWifiFunciona);


         list = this.FindViewById<ListView>(Resource.Id.listViewWifiTxtList);
         if (!GlobalVariables.WifiTxtMenuListAdapter.Any())
         {
            GlobalVariables.WifiTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Tipo de acceso", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
         }

         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.WifiTxtMenuListAdapter);
         list.ItemClick += OnListItemClick;

         subList = this.FindViewById<ListView>(Resource.Id.listViewWifiTxtSubList);

         editTextWifiProveedor.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next; //
         editTextWifiVelocidadDeSubida.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextWifiVelocidadDeBajada.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;

         editTextWifiProveedor.Text = GlobalVariables.WifiProveedor;
         editTextWifiVelocidadDeSubida.Text = GlobalVariables.WifiSubida;
         editTextWifiVelocidadDeBajada.Text = GlobalVariables.WifiBajada;


         switchWifiFunciona.Checked = GlobalVariables.WifiFunciona;


         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#ebacce");

         deselectAll();
         tab2Button.SetColorFilter(selectedColor);
         showView(true);
         buttonFinalizar.Visibility = ViewStates.Gone; //
         buttonRegresar.Text = "Regresar"; //
         activaBtnGuardar(); //

         buttonFinalizar.Click += showBtnFinalizar;
         buttonRegresar.Click += showViewBack;

         btnInicio.Click += delegate //
            {
               var intent = new Intent(this, typeof(MenuHomeActivity));
               StartActivity(intent);
            };

         //Contar el número de carácteres del EditText
         editTextWifiProveedor.TextChanged += (object sender, TextChangedEventArgs e) =>
      {
         int numChar = 300 - editTextWifiProveedor.Length();
         textViewWifiProveedorCont.Text = numChar.ToString();
         GlobalVariables.WifiProveedor = editTextWifiProveedor.Text;
         activaBtnGuardar(); //
      };

         switchWifiFunciona.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
         {
            GlobalVariables.WifiFunciona = e.IsChecked;
         };
         editTextWifiVelocidadDeSubida.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.WifiSubida = editTextWifiVelocidadDeSubida.Text;
            activaBtnGuardar(); //
         };
         editTextWifiVelocidadDeBajada.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.WifiBajada = editTextWifiVelocidadDeBajada.Text;
            activaBtnGuardar(); //
         };


         tab1Button.Click += delegate
         {
            deselectAll();
            tab1Button.SetColorFilter(selectedColor);
            var intent = new Intent(this, typeof(WifisActivity));
            StartActivity(intent);
         };

      }

      private void activaBtnGuardar() //
      {
         int numCharProveedor = 300 - editTextWifiProveedor.Length();
         int numCharSubida = 300 - editTextWifiVelocidadDeSubida.Length();
         int numCharBajada = 300 - editTextWifiVelocidadDeBajada.Length();
         string tipoAcceso = GlobalVariables.WifiTxtMenuListAdapter.ElementAt(0).SubHeading;

         if ((numCharProveedor < 300) && (numCharSubida < 300) && (numCharBajada < 300) && (tipoAcceso.Trim() != ""))
            buttonFinalizar.Visibility = ViewStates.Visible;
         else buttonFinalizar.Visibility = ViewStates.Gone;
      }

      public void changeTextBtnRegresarCondicion() //
      {
         var sparseArray = GlobalVariables.WifiCondicion.ToArray();
         string text = "Regresar";
         if (sparseArray != null)
            for (int x = 0; x < sparseArray.Count(); x++)
            {
               if (sparseArray[x])
                  text = "Guardar";
            }
         buttonRegresar.Text = text;
      }

      private void showViewBack(object sender, EventArgs e)
      {
         showView(true);
      }

      private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         var listView = sender as ListView;
         var position = e.Position;
         var subListAdapter = new ArrayAdapter<String>(this, GlobalVariables.SimpleListItemChecked, OpcionesMenus.CondicionesServicio);

         subList.Adapter = subListAdapter;
         subList.ChoiceMode = ChoiceMode.Single;
         subList.ItemClick += OnSubListItemClick;
         sparseArray = FindViewById<ListView>(Resource.Id.listViewWifiTxtSubList).CheckedItemPositions;
         showView(false);

         //Activar los elementos seleccionados
         int initialSelection = 100;
         if (GlobalVariables.WifiCondicion[0] == true)
            initialSelection = 0;
         else if (GlobalVariables.WifiCondicion[1] == true)
            initialSelection = 1;
         if (initialSelection != 100)
            subList.SetItemChecked(initialSelection, true);
         else buttonRegresar.Text = "Regresar";
      }

      private void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         MenusTableItem item = new MenusTableItem();
         var position = e.Position;
         GlobalVariables.WifiCondicion[position] = true;
         if (position == 0)
         {
            GlobalVariables.WifiCondicion[1] = false;
            GlobalVariables.WifiIsFormal = true;
         }
         else
         {
            GlobalVariables.WifiCondicion[0] = false;
            GlobalVariables.WifiIsFormal = false;
         }
         item.SubHeading = OpcionesMenus.CondicionesServicio[position];
         changeTextBtnRegresarCondicion();
         GlobalVariables.WifiTxtMenuListAdapter.ElementAt(0).SubHeading = item.SubHeading;
         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.WifiTxtMenuListAdapter);
         activaBtnGuardar();
      }

      private void showView(bool showPrincipal)
      {
         var state1 = ViewStates.Visible;
         var state2 = ViewStates.Gone;

         if (!showPrincipal)
         {
            state1 = ViewStates.Gone;
            state2 = ViewStates.Visible;
         }

         linearWifiTxtFinalizarBtn.Visibility = state1;
         linearTextViewProveedor.Visibility = state1;


         linearSwitchFunciona.Visibility = state1;

         //  linearTxtSpaceSwitch2.Visibility = state1;
         //linearTxtSpaceSwitch4.Visibility = state1;


         linearTextViewVelocidadDeSubida.Visibility = state1;
         linearTextViewVelocidadDeBajada.Visibility = state1;

         editTextWifiProveedor.Visibility = state1;
         editTextWifiVelocidadDeSubida.Visibility = state1;
         editTextWifiVelocidadDeBajada.Visibility = state1;

         containerWifiTxtList.Visibility = state1;

         linearWifiTxtRegresarBtn.Visibility = state2;
         containerWifiTxtSubList.Visibility = state2;
      }
      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);

      }

      private void showBtnFinalizar(object sender, EventArgs e)
      {
         string condicionWifi = GlobalVariables.WifiTxtMenuListAdapter.ElementAt(0).SubHeading;

         if (condicionWifi.Trim() == "" || condicionWifi == null)
            Toast.MakeText(ApplicationContext, "Seleccione tipo de acceso", ToastLength.Long).Show();
         else
         {
            Wifi Wifi = new Wifi()
            {
               Id = Guid.NewGuid().ToString(),
               Latitud = GlobalVariables.LatitudWifi,
               Longitud = GlobalVariables.LongitudWifi,
               Provider = GlobalVariables.WifiProveedor,
               UpSpeed = GlobalVariables.WifiSubida,
               DownSpeed = GlobalVariables.WifiBajada,
               Enviado = ""
            };
            data.SaveData(Wifi);

            GlobalVariables.wifiNuevaCaptura = true;
            Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         }
      }

      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab2Button.SetColorFilter(selectedColor);

         int numChar = 300 - editTextWifiProveedor.Length();
         textViewWifiProveedorCont.Text = numChar.ToString();
         GlobalVariables.WifiProveedor = editTextWifiProveedor.Text;
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