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
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Text;
using PueblosMagicos.Android.Inventario.Services;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "AgenciasTextosActivity", Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
   public class AgenciasTextosActivity : Activity
   {
      DataService data;
      private ImageButton tab1Button, tab2Button, tab3Button, btnInicio; //
      private Button buttonRegresar, buttonFinalizar;
      private EditText editTextAgenciaContacto, editTextAgenciaProducto, editTextAgenciaNombre;
      private Color selectedColor, deselectedColor;
      private LinearLayout textView, linearAgenciasFinalizarBtn, linearAgenciasRegresarBtn, linearAgenciasList, linearAgenciasContacto, linearAgenciasProductos, linearAgenciasNombre;
      private ListView list, subList;
      private TextView textViewAgenciaContactoCont, textViewAgenciaProductoCont, textViewAgenciaNombreCont;
      private FrameLayout containerAgenciasTxtList, containerAgenciasTxtSubList;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.AgenciasTextos);
         data = new DataService();

         //MenuLateral
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //


         buttonRegresar = this.FindViewById<Button>(Resource.Id.AgenciasRegresarBtn);
         buttonFinalizar = this.FindViewById<Button>(Resource.Id.AgenciasFinalizarBtn);
         editTextAgenciaContacto = this.FindViewById<EditText>(Resource.Id.editTextAgenciaContacto);
         editTextAgenciaProducto = this.FindViewById<EditText>(Resource.Id.editTextAgenciaProducto);
         editTextAgenciaNombre = this.FindViewById<EditText>(Resource.Id.editTextAgenciaNombre);

         textView = this.FindViewById<LinearLayout>(Resource.Id.linearTextView);
         linearAgenciasFinalizarBtn = this.FindViewById<LinearLayout>(Resource.Id.linearAgenciasFinalizarBtn);
         linearAgenciasRegresarBtn = this.FindViewById<LinearLayout>(Resource.Id.linearAgenciasRegresarBtn);
         linearAgenciasList = this.FindViewById<LinearLayout>(Resource.Id.linearAgenciasList);
         linearAgenciasContacto = this.FindViewById<LinearLayout>(Resource.Id.linearAgenciasContacto);
         linearAgenciasProductos = this.FindViewById<LinearLayout>(Resource.Id.linearAgenciasProductos);
         linearAgenciasNombre = this.FindViewById<LinearLayout>(Resource.Id.linearAgenciasNombre);


         list = this.FindViewById<ListView>(Resource.Id.listViewAgenciasTxtList);
         textViewAgenciaContactoCont = this.FindViewById<TextView>(Resource.Id.textViewAgenciaContactoCont);
         textViewAgenciaProductoCont = this.FindViewById<TextView>(Resource.Id.textViewAgenciaProductoCont);
         textViewAgenciaNombreCont = this.FindViewById<TextView>(Resource.Id.textViewAgenciaNombreCont);


         containerAgenciasTxtList = FindViewById<FrameLayout>(Resource.Id.containerAgenciasTxtList);
         containerAgenciasTxtSubList = FindViewById<FrameLayout>(Resource.Id.containerAgenciasTxtSubList);

         if (!GlobalVariables.agenciasTxtMenuListAdapter.Any())
         {
            GlobalVariables.agenciasTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Tipo de Servicio", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
         }


         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.agenciasTxtMenuListAdapter);
         list.ItemClick += OnListItemClick;

         subList = this.FindViewById<ListView>(Resource.Id.listViewAgenciasTxtSubList);

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#b7ddf5");

         editTextAgenciaNombre.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
         editTextAgenciaContacto.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next; //
         editTextAgenciaProducto.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;

         editTextAgenciaContacto.Text = GlobalVariables.AgenciaContact;
         editTextAgenciaProducto.Text = GlobalVariables.AgenciaProducts;
         editTextAgenciaNombre.Text = GlobalVariables.AgenciaNombre;

         showView(true);
         buttonFinalizar.Visibility = ViewStates.Gone; //
         buttonRegresar.Text = "Regresar"; //
         activaBtnGuardar(); //

         deselectAll();
         tab3Button.SetColorFilter(selectedColor);

         buttonRegresar.Click += showViewBack;

         buttonFinalizar.Click += showFinalizarActivity;

         tab1Button.Click += showTab1;
         btnInicio.Click += delegate //
         {
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         };

         //Contar el número de carácteres del EditText
         editTextAgenciaContacto.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextAgenciaContacto.Length();
            textViewAgenciaContactoCont.Text = numChar.ToString();
            GlobalVariables.AgenciaContact = editTextAgenciaContacto.Text;
            activaBtnGuardar(); //
         };

         editTextAgenciaProducto.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextAgenciaProducto.Length();
            textViewAgenciaProductoCont.Text = numChar.ToString();
            GlobalVariables.AgenciaProducts = editTextAgenciaProducto.Text;
            activaBtnGuardar(); //
         };
         editTextAgenciaNombre.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextAgenciaNombre.Length();
            textViewAgenciaProductoCont.Text = numChar.ToString();
            GlobalVariables.AgenciaNombre = editTextAgenciaNombre.Text;
            activaBtnGuardar(); //
         };

         tab2Button.Click += delegate
         {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(AgenciasHorariosActivity));
            StartActivity(intent);
         };
      }

      private void activaBtnGuardar() //
      {
         int numCharNombre = 300 - editTextAgenciaNombre.Length();
         int numCharContacto = 300 - editTextAgenciaContacto.Length();
         int numCharProducto = 300 - editTextAgenciaProducto.Length();
         string tipoServicio = GlobalVariables.agenciasTxtMenuListAdapter.ElementAt(0).SubHeading;

         if ((numCharNombre < 300) && (numCharContacto < 300) && (numCharProducto < 300) && (tipoServicio.Trim() != ""))
            buttonFinalizar.Visibility = ViewStates.Visible;
         else buttonFinalizar.Visibility = ViewStates.Gone;
      }

      public void changeTextBtnRegresarTipo() //
      {
         var sparseArray = GlobalVariables.agenciasTipoDeServicio.ToArray();
         string text = "Regresar";
         if (sparseArray != null)
            for (int x = 0; x < sparseArray.Count(); x++)
            {
               if (sparseArray[x])
                  text = "Guardar";
            }
         buttonRegresar.Text = text;
      }

      private void showFinalizarActivity(object sender, EventArgs e)
      {
         string tipoAgencia = GlobalVariables.agenciasTxtMenuListAdapter.ElementAt(0).SubHeading;

         if (tipoAgencia.Trim() == "" || tipoAgencia == null)
            Toast.MakeText(ApplicationContext, "Seleccione el tipo de servicio", ToastLength.Long).Show();
         else
         {
            DataTables.Agencias agencia = new DataTables.Agencias()
            {
               Id = Guid.NewGuid().ToString(),
               Latitud = GlobalVariables.LatitudAgencias,
               Longitud = GlobalVariables.LongitudAgencias,
               Type = tipoAgencia,
               Name = GlobalVariables.AgenciaNombre,
               Address = "",
               Contact = GlobalVariables.AgenciaContact,
               Products = GlobalVariables.AgenciaProducts,
               ServiceDays = "",
               ServiceHours = "",
               Enviado = ""
            };
            data.SaveData(agencia);

            GlobalVariables.agenciaNuevaCaptura = true;
            Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         }
      }

      private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         var listView = sender as ListView;
         var position = e.Position;
         var subListAdapter = new ArrayAdapter<String>(this, GlobalVariables.SimpleListItemChecked, OpcionesMenus.TipoAgencia);

         subList.Adapter = subListAdapter;
         subList.ChoiceMode = ChoiceMode.Single;
         subList.ItemClick += OnSubListItemClick;
         showView(false);

         //Activar los elementos seleccionados
         int initialSelection = 100;
         if (GlobalVariables.agenciasTipoDeServicio[0] == true)
            initialSelection = 0;
         else if (GlobalVariables.agenciasTipoDeServicio[1] == true)
            initialSelection = 1;
         if (initialSelection != 100)
            subList.SetItemChecked(initialSelection, true);
         else buttonRegresar.Text = "Regresar";
      }

      public void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         MenusTableItem item = new MenusTableItem();
         var position = e.Position;
         GlobalVariables.agenciasTipoDeServicio[position] = true;
         if (position == 0)
         {
            GlobalVariables.agenciasTipoDeServicio[1] = false;
         }
         else
         {
            GlobalVariables.agenciasTipoDeServicio[0] = false;
         }
         item.SubHeading = OpcionesMenus.TipoAgencia[position];
         changeTextBtnRegresarTipo();
         GlobalVariables.agenciasTxtMenuListAdapter.ElementAt(0).SubHeading = item.SubHeading;
         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.agenciasTxtMenuListAdapter);
      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);

      }

      private void showTab1(object sender, EventArgs e)
      {
         deselectAll();
         tab1Button.SetColorFilter(selectedColor);

         StartActivity(typeof(AgenciasActivity));
      }

      private void showViewBack(object sender, EventArgs e)
      {
         showView(true);
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
         linearAgenciasList.Visibility = state1;
         linearAgenciasContacto.Visibility = state1;
         linearAgenciasProductos.Visibility = state1;
         linearAgenciasNombre.Visibility = state1;
         linearAgenciasFinalizarBtn.Visibility = state1;
         containerAgenciasTxtList.Visibility = state1;
         editTextAgenciaContacto.Visibility = state1;
         editTextAgenciaProducto.Visibility = state1;
         editTextAgenciaNombre.Visibility = state1;

         linearAgenciasRegresarBtn.Visibility = state2;
         containerAgenciasTxtSubList.Visibility = state2;
      }


      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab3Button.SetColorFilter(selectedColor);

         int numChar = 300 - editTextAgenciaContacto.Length();
         textViewAgenciaContactoCont.Text = numChar.ToString();

         int numCharProducts = 300 - editTextAgenciaProducto.Length();
         textViewAgenciaProductoCont.Text = numCharProducts.ToString();

         int numCharNombre = 300 - editTextAgenciaNombre.Length();
         textViewAgenciaNombreCont.Text = numCharNombre.ToString();

         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.agenciasTxtMenuListAdapter);
         activaBtnGuardar();
      }

      private void ShowDetailsSubList(int playId)
      {
         _currentPlayId = playId;

         // We can display everything in-place with fragments.
         // Have the list highlight this item and show the data.
         //subList.SetItemChecked(playId, true);            
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
      public int _currentPlayId { get; set; }
   }
}