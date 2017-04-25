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
using Android.Text;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using PueblosMagicos.Android.Inventario.Services;
using PueblosMagicos.Android.Inventario.DataTables;

namespace PueblosMagicos.Android.Inventario
{
   [Activity(Label = "MercadosNotasActivity", Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
   public class MercadosTextosActivity : Activity
   {
      DataService data;
      public const int SimpleListItemChecked = 17367045;
      public const int SimpleExpandableListItem1 = 17367046;
      private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
      private ImageView buttonRegresar, buttonSiguiente;
      private FrameLayout containerMercTxtList, containerMercTxtSubList, containerMercTxtSubList2;
      private TextView MercadosNom_count, MercadosDes_count;
      private LinearLayout linearBtnRegresar, linearMercTxtNombre, linearMercTxtDesc, linearBtnSiguiente, linearMercTxtNoCom;
      private EditText editTextMercTxtNombre, editTextMercTxtDesc, editTextMercTxtNoCom, MercadosNomText, MercadosDesText;
      private Color selectedColor, deselectedColor;
      private ListView list;
      private int mercado = 0;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;

      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.MercadosTextos);
         data = new DataService();

         //MenuLateral
         mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_mercados_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_mercados_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_mercados_icon);
         tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_mercados_icon);

         buttonRegresar = this.FindViewById<ImageView>(Resource.Id.MercadosTxtRegresarBtn);
         buttonSiguiente = this.FindViewById<ImageView>(Resource.Id.MercadosTxtSiguienteBtn);

         MercadosNom_count = this.FindViewById<TextView>(Resource.Id.textViewMercTxtContNombre);
         MercadosDes_count = this.FindViewById<TextView>(Resource.Id.textViewMercTxtContDesc);

         editTextMercTxtNombre = this.FindViewById<EditText>(Resource.Id.editTextMercTxtNombre);
         editTextMercTxtDesc = this.FindViewById<EditText>(Resource.Id.editTextMercTxtDesc);
         editTextMercTxtNoCom = this.FindViewById<EditText>(Resource.Id.editTextMercTxtNoCom);

         linearBtnRegresar = this.FindViewById<LinearLayout>(Resource.Id.linearBtnTxtRegresar);
         linearBtnSiguiente = this.FindViewById<LinearLayout>(Resource.Id.linearBtnTxtSiguiente);
         linearMercTxtNombre = this.FindViewById<LinearLayout>(Resource.Id.linearMercTxtNombre);
         linearMercTxtDesc = this.FindViewById<LinearLayout>(Resource.Id.linearMercTxtDesc);
         linearMercTxtNoCom = this.FindViewById<LinearLayout>(Resource.Id.linearMercTxtNoCom);
         containerMercTxtList = this.FindViewById<FrameLayout>(Resource.Id.containerMercTxtList);
         containerMercTxtSubList = this.FindViewById<FrameLayout>(Resource.Id.containerMercTxtSubList);
         containerMercTxtSubList2 = this.FindViewById<FrameLayout>(Resource.Id.containerMercTxtSubList2);

         list = this.FindViewById<ListView>(Resource.Id.listViewMercadosTxtList);

         if (!GlobalVariables.mercadoTxtMenuListAdapter.Any())
         {
            GlobalVariables.mercadoTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Tipo de mercado", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
            GlobalVariables.mercadoTxtMenuListAdapter.Add(new MenusTableItem() { Heading = "Condiciones de servicio", SubHeading = " ", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
         }

         editTextMercTxtNombre.Text = GlobalVariables.mercadoTxtNombre;
         editTextMercTxtDesc.Text = GlobalVariables.mercadoTxtDesc;
         editTextMercTxtNoCom.Text = GlobalVariables.mercadoTxtNoCom;

         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.mercadoTxtMenuListAdapter);
         list.ItemClick += OnSubListItemClick;

         selectedColor = Color.ParseColor("#303030"); //The color u want    
         deselectedColor = Color.ParseColor("#ffffff");

         showView();
         deselectAll();
         tab4Button.SetColorFilter(selectedColor);

         buttonRegresar.Click += showViewBack;

         //Contar el número de carácteres del Nombre
         editTextMercTxtNombre.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextMercTxtNombre.Length();
            MercadosNom_count.Text = numChar.ToString();
            GlobalVariables.mercadoTxtNombre = editTextMercTxtNombre.Text;
         };

         //Contar el número de carácteres de Descripcion
         editTextMercTxtDesc.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextMercTxtDesc.Length();
            MercadosDes_count.Text = numChar.ToString();
            GlobalVariables.mercadoTxtDesc = editTextMercTxtDesc.Text;
         };

         editTextMercTxtNoCom.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            GlobalVariables.mercadoTxtNoCom = editTextMercTxtNoCom.Text;
         };

         buttonSiguiente.Click += showTab1;

         tab1Button.Click += delegate
         {
            deselectAll();
            tab1Button.SetColorFilter(selectedColor);
            var intent = new Intent(this, typeof(MercadosActivity));
            StartActivity(intent);

         };

         tab2Button.Click += delegate
         {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(MercadosFotosActivity));
            StartActivity(intent);
         };

         tab3Button.Click += delegate
         {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            // Aquí van las notas
            var intent = new Intent(this, typeof(MercadosHorariosActivity));
            StartActivity(intent);
         };

      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);
         tab4Button.SetColorFilter(deselectedColor);
      }

      private void showView()
      {
         linearBtnSiguiente.Visibility = ViewStates.Visible;
         linearMercTxtNombre.Visibility = ViewStates.Visible;
         linearMercTxtDesc.Visibility = ViewStates.Visible;
         linearMercTxtNoCom.Visibility = ViewStates.Visible;
         editTextMercTxtNombre.Visibility = ViewStates.Visible;
         editTextMercTxtDesc.Visibility = ViewStates.Visible;
         editTextMercTxtNoCom.Visibility = ViewStates.Visible;
         containerMercTxtList.Visibility = ViewStates.Visible;

         linearBtnRegresar.Visibility = ViewStates.Gone;
         //linearBtnRegresar2.Visibility = ViewStates.Gone;
         containerMercTxtSubList.Visibility = ViewStates.Gone;
         containerMercTxtSubList2.Visibility = ViewStates.Gone;
      }

      private void showTab1(object sender, EventArgs e)
      {
         string type = "";
         //{ "Artesanal", "Gastronómico", "Tradicional" }
         if (GlobalVariables.mercadoTxtTipo[0])
            type = "Artesanal";
         if (GlobalVariables.mercadoTxtTipo[1])
            type = "Gastronómico";
         if (GlobalVariables.mercadoTxtTipo[2])
            type = "Tradicional";

         Mercados mercado = new Mercados()
         {
            Created = DateTime.Now.ToString("yyyy-MM-dd"),
            Description = GlobalVariables.mercadoTxtDesc,
            Latitud = GlobalVariables.LatitudMercado,
            Longitud = GlobalVariables.LongitudMercado,
            ServiceDays = "",
            ServiceHours = "",
            ShopNum = GlobalVariables.mercadoTxtNoCom,
            Type = type
         };
         data.SaveData(mercado);

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

      public void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
         //
         var listView = sender as ListView;
         var position = e.Position;
         var subListAdapter = new ArrayAdapter<String>(this, SimpleListItemChecked, ((position == 0) ? OpcionesMenus.TipoMercado : OpcionesMenus.CondicionesServicioMercado));
         var subList = this.FindViewById<ListView>(Resource.Id.listViewMercadosTxtSubList2);
         mercado = position;

         subList.Adapter = subListAdapter;
         subList.ItemClick += OnSubListItemClick2;
         subList.ChoiceMode = ChoiceMode.Single;

         linearBtnSiguiente.Visibility = ViewStates.Gone;
         linearMercTxtNombre.Visibility = ViewStates.Gone;
         linearMercTxtDesc.Visibility = ViewStates.Gone;
         linearMercTxtNoCom.Visibility = ViewStates.Gone;
         editTextMercTxtNombre.Visibility = ViewStates.Gone;
         editTextMercTxtDesc.Visibility = ViewStates.Gone;
         editTextMercTxtNoCom.Visibility = ViewStates.Gone;
         containerMercTxtList.Visibility = ViewStates.Gone;
         containerMercTxtSubList.Visibility = ViewStates.Gone;
         linearBtnRegresar.Visibility = ViewStates.Gone;

         linearBtnRegresar.Visibility = ViewStates.Visible;
         containerMercTxtSubList2.Visibility = ViewStates.Visible;

         //Activar los elementos seleccionados
         int initialSelection = 100;
         switch (mercado)
         {
            case 0:
               if (GlobalVariables.mercadoTxtTipo[0] == true)
                  initialSelection = 0;
               else if (GlobalVariables.mercadoTxtTipo[1] == true)
                  initialSelection = 1;
               else if (GlobalVariables.mercadoTxtTipo[2] == true)
                  initialSelection = 2;
               break;
            case 1:
               if (GlobalVariables.mercadoTxtCondicion[0] == true)
                  initialSelection = 0;
               else if (GlobalVariables.mercadoTxtCondicion[1] == true)
                  initialSelection = 1;
               break;
         }
         if (initialSelection != 100)
            subList.SetItemChecked(initialSelection, true);
      }

      public void OnSubListItemClick2(object sender, AdapterView.ItemClickEventArgs e)
      {
         MenusTableItem item = new MenusTableItem();
         var position = e.Position;
         switch (mercado)
         {
            case 0:
               GlobalVariables.mercadoTxtTipo[position] = true;
               if (position == 0)
               { GlobalVariables.mercadoTxtTipo[1] = false; GlobalVariables.mercadoTxtTipo[2] = false; }
               else if (position == 1)
               { GlobalVariables.mercadoTxtTipo[0] = false; GlobalVariables.mercadoTxtTipo[2] = false; }
               else
               { GlobalVariables.mercadoTxtTipo[0] = false; GlobalVariables.mercadoTxtTipo[1] = false; }
               item.SubHeading = OpcionesMenus.TipoMercado[position];
               break;
            case 1:
               GlobalVariables.mercadoTxtCondicion[position] = true;
               if (position == 0)
                  GlobalVariables.mercadoTxtCondicion[1] = false;
               else
                  GlobalVariables.mercadoTxtCondicion[0] = false;
               item.SubHeading = OpcionesMenus.CondicionesServicioMercado[position];
               break;
         }
         GlobalVariables.mercadoTxtMenuListAdapter.ElementAt(mercado).SubHeading = item.SubHeading;
         list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.mercadoTxtMenuListAdapter);
      }

      private void showViewBack(object sender, EventArgs e)
      {
         showView();
      }
      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab4Button.SetColorFilter(selectedColor);

         int numCharNombre = 300 - editTextMercTxtNombre.Length();
         MercadosNom_count.Text = numCharNombre.ToString();

         int numCharDesc = 300 - editTextMercTxtDesc.Length();
         MercadosDes_count.Text = numCharDesc.ToString();
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