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
   [Activity(Label = "NotasActivity", Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
   public class SenalamientosTextosActivity : Activity
   {
      DataService data;
      public const int SimpleListItemChecked = 17367045;
      public const int SimpleExpandableListItem1 = 17367046;
      private ImageButton tab1Button, tab2Button, tab3Button, tab4Button, btnInicio; //
      private Button buttonRegresar;
      private EditText editText;
      private FrameLayout frameLayoutList, frameLayoutSubList;
      private Color selectedColor, deselectedColor;
      private LinearLayout textView, linearSenalTxtFinalizarBtn;
      private Button buttonSiguiente;
      private ListView list;
      private TextView sms_count;
      private int senalamiento = 0;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.SenalamientosTextos);
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
         tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

         buttonRegresar = this.FindViewById<Button>(Resource.Id.regresar_btn);
         editText = this.FindViewById<EditText>(Resource.Id.editTextSenalCom);
         frameLayoutList = this.FindViewById<FrameLayout>(Resource.Id.containerList);
         frameLayoutSubList = this.FindViewById<FrameLayout>(Resource.Id.containerSubList);
         textView = this.FindViewById<LinearLayout>(Resource.Id.linearTextView);
         linearSenalTxtFinalizarBtn = this.FindViewById<LinearLayout>(Resource.Id.linearSenalTxtFinalizarBtn);
         //linear1 = this.FindViewById<LinearLayout>(Resource.Id.linear1);
         buttonSiguiente = this.FindViewById<Button>(Resource.Id.SenalTxtFinalizarBtn);
         list = this.FindViewById<ListView>(Resource.Id.listViewSenalTxtList);
         sms_count = this.FindViewById<TextView>(Resource.Id.textViewcomentariosCont);

         if (!GlobalVariables.senalTxtMenuListAdapter.Any())
         {
            GlobalVariables.senalTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Tipo de Señalamiento", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
            GlobalVariables.senalTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Posición del Señalamiento", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
            GlobalVariables.senalTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Visibilidad del Señalamiento", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
         }

         editText.Text = GlobalVariables.senalTxtComentario;

         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.senalTxtMenuListAdapter);
         list.ItemClick += OnListItemClick;

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#96c88e");

         editText.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;

         showView();
         buttonSiguiente.Visibility = ViewStates.Gone;
         buttonRegresar.Text = "Regresar";
         activaBtnGuardar();
         deselectAll();
         tab4Button.SetColorFilter(selectedColor);

         buttonRegresar.Click += showViewBack;

         buttonSiguiente.Click += showFinalizarActivity;

         tab1Button.Click += showTab1;

         btnInicio.Click += delegate //
         {
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         };

         //Contar el número de carácteres del EditText
         editText.TextChanged += (object sender, TextChangedEventArgs e) =>
      {
         int numChar = 300 - editText.Length();
         sms_count.Text = numChar.ToString();
         GlobalVariables.senalTxtComentario = editText.Text;
         activaBtnGuardar();
      };

         tab2Button.Click += delegate
         {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(SenalamientosFotosActivity));
            StartActivity(intent);
         };

         tab3Button.Click += delegate
         {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(SenalamientosOrientacionActivity));
            StartActivity(intent);
         };
      }

      private void activaBtnGuardar()
      {
         int numCharComentario = 300 - editText.Length();

         string tipoSenal = GlobalVariables.senalTxtMenuListAdapter.ElementAt(0).SubHeading;
         string posicionSenal = GlobalVariables.senalTxtMenuListAdapter.ElementAt(1).SubHeading;
         string visibilidadSenal = GlobalVariables.senalTxtMenuListAdapter.ElementAt(2).SubHeading;
         if ((numCharComentario < 300) && (tipoSenal.Trim() != "") && (posicionSenal.Trim() != "") && (visibilidadSenal.Trim() != ""))
            buttonSiguiente.Visibility = ViewStates.Visible;
         else buttonSiguiente.Visibility = ViewStates.Gone;

      }

      public void changeTextBtnRegresarTipo()
      {
         var sparseArray = GlobalVariables.senalTxtTipo.ToArray();
         string text = "Regresar";
         if (sparseArray != null)
            for (int x = 0; x < sparseArray.Count(); x++)
            {
               if (sparseArray[x])
                  text = "Guardar";
            }
         buttonRegresar.Text = text;
      }

      public void changeTextBtnRegresarPosicion()
      {
         var sparseArray = GlobalVariables.senalTxtPosicion.ToArray();
         string text = "Regresar";
         if (sparseArray != null)
            for (int x = 0; x < sparseArray.Count(); x++)
            {
               if (sparseArray[x])
                  text = "Guardar";
            }
         buttonRegresar.Text = text;
      }

      public void changeTextBtnRegresarVisibilidad()
      {
         var sparseArray = GlobalVariables.senalTxtVisibilidad.ToArray();
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
         string type = "";
         string position = "";
         string visible = "";

         if (GlobalVariables.senalTxtTipo[0])
            type = "Turístico";
         if (GlobalVariables.senalTxtTipo[1])
            type = "Tránsito";

         if (GlobalVariables.senalTxtPosicion[0])
            position = "A nivel del suelo";
         if (GlobalVariables.senalTxtPosicion[1])
            position = "Elevado";

         if (GlobalVariables.senalTxtVisibilidad[0])
            visible = "Totalmente visible";
         if (GlobalVariables.senalTxtVisibilidad[1])
            visible = "Parcialmente visible";
         if (GlobalVariables.senalTxtVisibilidad[2])
            visible = "No visible";

         DataTables.Senalamientos signal = new DataTables.Senalamientos()
         {
            Id = Guid.NewGuid().ToString(),
            Latitud = GlobalVariables.LatitudSenalamiento,
            Longitud = GlobalVariables.LongitudSenalamiento,
            Type = type,
            Position = position,
            Visible = visible, 
            FotoName = GlobalVariables.SignalPhotoName, 
            Enviado = ""
         };
         data.SaveData(signal);

         GlobalVariables.senalNuevaCaptura = true;
         Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
         var intent = new Intent(this, typeof(MenuHomeActivity));
         StartActivity(intent);
      }

      private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {


         var listView = sender as ListView;
         var position = e.Position;
         var subListAdapter = new ArrayAdapter<String>(this, SimpleListItemChecked, ((position == 0) ? OpcionesMenus.TipoSenalamiento : (position == 1) ? OpcionesMenus.PosicionSenalamiento : OpcionesMenus.VisibilidadSenalamiento));
         var subList = this.FindViewById<ListView>(Resource.Id.listViewSenalTxtSubList);
         senalamiento = position;

         subList.Adapter = subListAdapter;
         subList.ItemClick += OnSubListItemClick;
         subList.ChoiceMode = ChoiceMode.Single;

         buttonRegresar.Visibility = ViewStates.Visible;
         frameLayoutSubList.Visibility = ViewStates.Visible;
         //linear1.Visibility = ViewStates.Visible;

         editText.Visibility = ViewStates.Gone;
         frameLayoutList.Visibility = ViewStates.Gone;
         textView.Visibility = ViewStates.Gone;
         linearSenalTxtFinalizarBtn.Visibility = ViewStates.Gone;

         //Activar los elementos seleccionados
         int initialSelection = 100;
         switch (senalamiento)
         {
            case 0:
               if (GlobalVariables.senalTxtTipo[0] == true)
                  initialSelection = 0;
               else if (GlobalVariables.senalTxtTipo[1] == true)
                  initialSelection = 1;
               break;
            case 1:
               if (GlobalVariables.senalTxtPosicion[0] == true)
                  initialSelection = 0;
               else if (GlobalVariables.senalTxtPosicion[1] == true)
                  initialSelection = 1;
               break;
            case 2:
               if (GlobalVariables.senalTxtVisibilidad[0] == true)
                  initialSelection = 0;
               else if (GlobalVariables.senalTxtVisibilidad[1] == true)
                  initialSelection = 1;
               else if (GlobalVariables.senalTxtVisibilidad[2] == true)
                  initialSelection = 2;
               break;
         }
         if (initialSelection != 100)
            subList.SetItemChecked(initialSelection, true);
         else buttonRegresar.Text = "Regresar";
      }

      public void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         MenusTableItem item = new MenusTableItem();
         var position = e.Position;
         switch (senalamiento)
         {
            case 0:
               GlobalVariables.senalTxtTipo[position] = true;
               if (position == 0)
                  GlobalVariables.senalTxtTipo[1] = false;
               else
                  GlobalVariables.senalTxtTipo[0] = false;
               item.SubHeading = OpcionesMenus.TipoSenalamiento[position];
               changeTextBtnRegresarTipo();
               break;
            case 1:
               GlobalVariables.senalTxtPosicion[position] = true;
               if (position == 0)
                  GlobalVariables.senalTxtPosicion[1] = false;
               else
                  GlobalVariables.senalTxtPosicion[0] = false;
               item.SubHeading = OpcionesMenus.PosicionSenalamiento[position];
               changeTextBtnRegresarPosicion();
               break;
            case 2:
               GlobalVariables.senalTxtVisibilidad[position] = true;
               if (position == 0)
               { GlobalVariables.senalTxtVisibilidad[1] = false; GlobalVariables.senalTxtVisibilidad[2] = false; }
               else if (position == 1)
               { GlobalVariables.senalTxtVisibilidad[0] = false; GlobalVariables.senalTxtVisibilidad[2] = false; }
               else
               { GlobalVariables.senalTxtVisibilidad[0] = false; GlobalVariables.senalTxtVisibilidad[1] = false; }
               item.SubHeading = OpcionesMenus.VisibilidadSenalamiento[position];
               changeTextBtnRegresarVisibilidad();
               break;
         }
         GlobalVariables.senalTxtMenuListAdapter.ElementAt(senalamiento).SubHeading = item.SubHeading;
         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.senalTxtMenuListAdapter);
         activaBtnGuardar();
      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);
         tab4Button.SetColorFilter(deselectedColor);
      }

      private void showTab1(object sender, EventArgs e)
      {
         deselectAll();
         tab1Button.SetColorFilter(selectedColor);

         StartActivity(typeof(SenalamientosMapActivity));
      }

      private void showViewBack(object sender, EventArgs e)
      {
         showView();
      }

      private void showView()
      {
         editText.Visibility = ViewStates.Visible;
         frameLayoutList.Visibility = ViewStates.Visible;
         textView.Visibility = ViewStates.Visible;
         linearSenalTxtFinalizarBtn.Visibility = ViewStates.Visible;

         buttonRegresar.Visibility = ViewStates.Gone;
         frameLayoutSubList.Visibility = ViewStates.Gone;
         //linear1.Visibility = ViewStates.Gone;
      }


      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab4Button.SetColorFilter(selectedColor);

         int numChar = 300 - editText.Length();
         sms_count.Text = numChar.ToString();
         GlobalVariables.senalTxtComentario = editText.Text;
         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.senalTxtMenuListAdapter);
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