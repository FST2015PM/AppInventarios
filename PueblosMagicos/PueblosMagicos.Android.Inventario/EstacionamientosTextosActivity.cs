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
   [Activity(Label = "EstacionamientosTextosActivity", Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
   public class EstacionamientosTextosActivity : Activity
   {
      DataService data;
      private ImageButton tab1Button, tab2Button, tab3Button, tab4Button, btnInicio;
      private Button buttonFinalizar, buttonRegresar;
      private TextView textViewEstacionamientoNombreCont, textViewEstacionamientoContactoCont, textViewEstacionamientoAdicionalesCont;
      private EditText editTextEstacionamientoNombre, editTextEstacionamientoCapacidadAutos, editTextEstacionamientoTarifa, editTextEstacionamientoContacto, editTextEstacionamientoAdicionales;
      private Switch switchEstacionamientoTiempoLibre, switchEstacionamiento24Hrs, switchEstacionamientoAutoservicio;
      private LinearLayout linearEstacionamientosTxtFinalizarBtn, linearTextViewNombre, linearEstacionamientoCapacidadAutos, linearEstacionamientoTarifa,
          linearTxtSpaceSwitch1, linearTxtSpaceSwitch2, linearTxtSpaceSwitch3, linearTxtSpaceSwitch4,
          linearTextViewContacto, linearTextViewAdicionales, linearEstacionamientosTxtRegresarBtn,
          linearSwitchTiempoLibre, linearSwitch24Hrs, linearSwitchAutoservicio;
      private FrameLayout containerEstacionamientosTxtList, containerEstacionamientosTxtSubList;
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
         SetContentView(Resource.Layout.EstacionamientosTextos);
         data = new DataService();

         //MenuLateral
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_estacionamientos_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_estacionamientos_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_estacionamientos_icon);
         tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_estacionamientos_icon);
         buttonFinalizar = FindViewById<Button>(Resource.Id.btnFinalizarEstacionamientos);
         buttonRegresar = FindViewById<Button>(Resource.Id.btnRegresarEstacionamientos);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

         linearEstacionamientosTxtFinalizarBtn = FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosTxtFinalizarBtn);
         linearTextViewNombre = FindViewById<LinearLayout>(Resource.Id.linearTextViewNombre);
         linearEstacionamientoCapacidadAutos = FindViewById<LinearLayout>(Resource.Id.linearEstacionamientoCapacidadAutos);
         linearEstacionamientoTarifa = FindViewById<LinearLayout>(Resource.Id.linearEstacionamientoTarifa);
         linearTxtSpaceSwitch1 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch1);
         linearSwitchTiempoLibre = FindViewById<LinearLayout>(Resource.Id.linearSwitchTiempoLibre);
         linearSwitch24Hrs = FindViewById<LinearLayout>(Resource.Id.linearSwitch24Hrs);
         linearSwitchAutoservicio = FindViewById<LinearLayout>(Resource.Id.linearSwitchAutoservicio);
         linearTxtSpaceSwitch2 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch2);
         linearTxtSpaceSwitch3 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch3);
         linearTxtSpaceSwitch4 = FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch4);
         linearTextViewContacto = FindViewById<LinearLayout>(Resource.Id.linearTextViewContacto);
         linearTextViewAdicionales = FindViewById<LinearLayout>(Resource.Id.linearTextViewAdicionales);
         containerEstacionamientosTxtList = FindViewById<FrameLayout>(Resource.Id.containerEstacionamientosTxtList);
         linearEstacionamientosTxtRegresarBtn = FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosTxtRegresarBtn);
         containerEstacionamientosTxtSubList = FindViewById<FrameLayout>(Resource.Id.containerEstacionamientosTxtSubList);

         textViewEstacionamientoNombreCont = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientoNombreCont);
         textViewEstacionamientoContactoCont = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientoContactoCont);
         textViewEstacionamientoAdicionalesCont = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientoAdicionalesCont);
         editTextEstacionamientoNombre = this.FindViewById<EditText>(Resource.Id.editTextEstacionamientoNombre);
         editTextEstacionamientoCapacidadAutos = this.FindViewById<EditText>(Resource.Id.editTextEstacionamientoCapacidadAutos);
         editTextEstacionamientoTarifa = this.FindViewById<EditText>(Resource.Id.editTextEstacionamientoTarifa);
         editTextEstacionamientoContacto = this.FindViewById<EditText>(Resource.Id.editTextEstacionamientoContacto);
         editTextEstacionamientoAdicionales = this.FindViewById<EditText>(Resource.Id.editTextEstacionamientoAdicionales);
         switchEstacionamientoTiempoLibre = this.FindViewById<Switch>(Resource.Id.switchEstacionamientoTiempoLibre);
         switchEstacionamiento24Hrs = this.FindViewById<Switch>(Resource.Id.switchEstacionamiento24Hrs);
         switchEstacionamientoAutoservicio = this.FindViewById<Switch>(Resource.Id.switchEstacionamientoAutoservicio);

         list = this.FindViewById<ListView>(Resource.Id.listViewEstacionamientosTxtList);
         if (!GlobalVariables.estacionamientoTxtMenuListAdapter.Any())
         {
            GlobalVariables.estacionamientoTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Condiciones de servicio", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
         }

         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.estacionamientoTxtMenuListAdapter);
         list.ItemClick += OnListItemClick;

         subList = this.FindViewById<ListView>(Resource.Id.listViewEstacionamientosTxtSubList);

         editTextEstacionamientoNombre.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextEstacionamientoCapacidadAutos.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextEstacionamientoTarifa.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextEstacionamientoContacto.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextEstacionamientoAdicionales.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;


         editTextEstacionamientoNombre.Text = GlobalVariables.EstacionamientoName;
         editTextEstacionamientoCapacidadAutos.Text = GlobalVariables.EstacionamientoCarCapacity;
         editTextEstacionamientoTarifa.Text = GlobalVariables.EstacionamientoFee;
         editTextEstacionamientoContacto.Text = GlobalVariables.EstacionamientoContact;
         editTextEstacionamientoAdicionales.Text = GlobalVariables.EstacionamientoAmenities;

         switchEstacionamientoTiempoLibre.Checked = GlobalVariables.EstacionamientoIsFreeTime;
         switchEstacionamiento24Hrs.Checked = GlobalVariables.EstacionamientoIs24h;
         switchEstacionamientoAutoservicio.Checked = GlobalVariables.EstacionamientoIsSelfService;

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#e9b7a0");

         deselectAll();
         tab4Button.SetColorFilter(selectedColor);
         showView(true);
         buttonFinalizar.Visibility = ViewStates.Gone; //
         buttonRegresar.Text = "Regresar"; //
         activaBtnGuardar(); //

         buttonFinalizar.Click += showBtnFinalizar;
         buttonRegresar.Click += showViewBack;

         //Contar el número de carácteres del EditText
         editTextEstacionamientoNombre.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextEstacionamientoNombre.Length();
            textViewEstacionamientoNombreCont.Text = numChar.ToString();
            GlobalVariables.EstacionamientoName = editTextEstacionamientoNombre.Text;
            activaBtnGuardar(); //
         };

         editTextEstacionamientoCapacidadAutos.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.EstacionamientoCarCapacity = editTextEstacionamientoCapacidadAutos.Text;
            activaBtnGuardar(); //
         };

         editTextEstacionamientoTarifa.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.EstacionamientoFee = editTextEstacionamientoTarifa.Text;
            activaBtnGuardar(); //
         };

         editTextEstacionamientoContacto.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextEstacionamientoContacto.Length();
            textViewEstacionamientoContactoCont.Text = numChar.ToString();
            GlobalVariables.EstacionamientoContact = editTextEstacionamientoContacto.Text;
            activaBtnGuardar(); //
         };

         editTextEstacionamientoAdicionales.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextEstacionamientoAdicionales.Length();
            textViewEstacionamientoAdicionalesCont.Text = numChar.ToString();
            GlobalVariables.EstacionamientoAmenities = editTextEstacionamientoAdicionales.Text;
            activaBtnGuardar(); //
         };

         switchEstacionamientoTiempoLibre.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
         {
            GlobalVariables.EstacionamientoIsFreeTime = e.IsChecked;
         };

         switchEstacionamiento24Hrs.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
         {
            GlobalVariables.EstacionamientoIs24h = e.IsChecked;
         };

         switchEstacionamientoAutoservicio.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
         {
            GlobalVariables.EstacionamientoIsSelfService = e.IsChecked;
         };

         tab1Button.Click += delegate
         {
            deselectAll();
            tab1Button.SetColorFilter(selectedColor);
            var intent = new Intent(this, typeof(EstacionamientosActivity));
            StartActivity(intent);

         };

         tab2Button.Click += delegate
         {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);
            var intent = new Intent(this, typeof(EstacionamientosFotosActivity));
            StartActivity(intent);

         };

         tab3Button.Click += delegate
         {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);
            var intent = new Intent(this, typeof(EstacionamientosHorariosActivity));
            StartActivity(intent);
         };

         btnInicio.Click += delegate //
         {
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         };
      }

      private void activaBtnGuardar() //
      {
         int numCharNombre = 300 - editTextEstacionamientoNombre.Length();
         int numCharCapacidad = 300 - editTextEstacionamientoCapacidadAutos.Length();
         int numCharTarifa = 300 - editTextEstacionamientoTarifa.Length();
         int numCharContacto = 300 - editTextEstacionamientoContacto.Length();
         int numCharAdicional = 300 - editTextEstacionamientoAdicionales.Length();

         string condicionServicio = GlobalVariables.estacionamientoTxtMenuListAdapter.ElementAt(0).SubHeading;

         if ((numCharNombre < 300) && (numCharContacto < 300) && (numCharCapacidad < 300) && (numCharTarifa < 300) && (condicionServicio.Trim() != ""))
            buttonFinalizar.Visibility = ViewStates.Visible;
         else buttonFinalizar.Visibility = ViewStates.Gone;
      }

      public void changeTextBtnRegresarCondicion() //
      {
         var sparseArray = GlobalVariables.estacionamientoCondicion.ToArray();
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
         sparseArray = FindViewById<ListView>(Resource.Id.listViewEstacionamientosTxtSubList).CheckedItemPositions;
         showView(false);

         //Activar los elementos seleccionados
         int initialSelection = 100;
         if (GlobalVariables.estacionamientoCondicion[0] == true)
            initialSelection = 0;
         else if (GlobalVariables.estacionamientoCondicion[1] == true)
            initialSelection = 1;
         if (initialSelection != 100)
            subList.SetItemChecked(initialSelection, true);
         else buttonRegresar.Text = "Regresar";
      }

      private void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         MenusTableItem item = new MenusTableItem();
         var position = e.Position;
         GlobalVariables.estacionamientoCondicion[position] = true;
         if (position == 0)
         {
            GlobalVariables.estacionamientoCondicion[1] = false;
            GlobalVariables.EstacionamientoIsFormal = true;
         }
         else
         {
            GlobalVariables.estacionamientoCondicion[0] = false;
            GlobalVariables.EstacionamientoIsFormal = false;
         }
         item.SubHeading = OpcionesMenus.CondicionesServicio[position];
         changeTextBtnRegresarCondicion();
         GlobalVariables.estacionamientoTxtMenuListAdapter.ElementAt(0).SubHeading = item.SubHeading;
         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.estacionamientoTxtMenuListAdapter);
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

         linearEstacionamientosTxtFinalizarBtn.Visibility = state1;
         linearTextViewNombre.Visibility = state1;
         linearEstacionamientoCapacidadAutos.Visibility = state1;
         linearEstacionamientoTarifa.Visibility = state1;
         linearTxtSpaceSwitch1.Visibility = state1;
         linearSwitchTiempoLibre.Visibility = state1;
         linearSwitchAutoservicio.Visibility = state1;
         linearSwitch24Hrs.Visibility = state1;
         linearTxtSpaceSwitch2.Visibility = state1;
         linearTxtSpaceSwitch4.Visibility = state1;
         linearTextViewContacto.Visibility = state1;
         linearTextViewAdicionales.Visibility = state1;
         editTextEstacionamientoNombre.Visibility = state1;
         editTextEstacionamientoCapacidadAutos.Visibility = state1;
         editTextEstacionamientoTarifa.Visibility = state1;
         editTextEstacionamientoContacto.Visibility = state1;
         editTextEstacionamientoAdicionales.Visibility = state1;
         containerEstacionamientosTxtList.Visibility = state1;

         linearEstacionamientosTxtRegresarBtn.Visibility = state2;
         containerEstacionamientosTxtSubList.Visibility = state2;
      }
      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);
         tab4Button.SetColorFilter(deselectedColor);
      }

      private void showBtnFinalizar(object sender, EventArgs e)
      {
         string condicionEstacionamiento = GlobalVariables.estacionamientoTxtMenuListAdapter.ElementAt(0).SubHeading;

         if (condicionEstacionamiento.Trim() == "" || condicionEstacionamiento == null)
            Toast.MakeText(ApplicationContext, "Seleccione la condición del servicio", ToastLength.Long).Show();
         else
         {
            Estacionamientos estacionamiento = new Estacionamientos()
            {
               Id = Guid.NewGuid().ToString(),
               Latitud = GlobalVariables.LatitudEstacionamiento,
               Longitud = GlobalVariables.LongitudEstacionamiento,
               Name = GlobalVariables.EstacionamientoName,
               CarCapacity = GlobalVariables.EstacionamientoCarCapacity,
               Fee = GlobalVariables.EstacionamientoFee,
               FreeTime = GlobalVariables.EstacionamientoIsFreeTime,
               ServiceDays = "",
               ServiceHours = "",
               Is24h = GlobalVariables.EstacionamientoIs24h,
               IsSelfServices = GlobalVariables.EstacionamientoIsSelfService,
               IsFormal = GlobalVariables.EstacionamientoIsFormal,
               Contact = GlobalVariables.EstacionamientoContact,
               Amenities = GlobalVariables.EstacionamientoAmenities,
               FotoName = GlobalVariables.ParkingPhotoName,
               Enviado = ""
            };
            data.SaveData(estacionamiento);

            GlobalVariables.estacionamientoNuevaCaptura = true;
            Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         }
      }

      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab4Button.SetColorFilter(selectedColor);

         int numChar = 300 - editTextEstacionamientoNombre.Length();
         textViewEstacionamientoNombreCont.Text = numChar.ToString();
         GlobalVariables.EstacionamientoName = editTextEstacionamientoNombre.Text;


         GlobalVariables.EstacionamientoCarCapacity = editTextEstacionamientoCapacidadAutos.Text;
         GlobalVariables.EstacionamientoFee = editTextEstacionamientoTarifa.Text;

         int numCharContacto = 300 - editTextEstacionamientoContacto.Length();
         textViewEstacionamientoContactoCont.Text = numCharContacto.ToString();
         GlobalVariables.EstacionamientoContact = editTextEstacionamientoContacto.Text;

         int numCharAdicionales = 300 - editTextEstacionamientoAdicionales.Length();
         textViewEstacionamientoAdicionalesCont.Text = numCharAdicionales.ToString();
         GlobalVariables.EstacionamientoAmenities = editTextEstacionamientoAdicionales.Text;
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