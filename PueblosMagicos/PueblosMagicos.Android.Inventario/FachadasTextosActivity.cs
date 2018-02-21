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
using PueblosMagicos.Android.Inventario.DataTables;

namespace PueblosMagicos.Android.Inventario
{
   //, Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
   [Activity(Label = "FachadasTextos")]
   public class FachadasTextosActivity : Activity
   {
      DataService data;
      private ImageButton tab1Button, tab2Button, tab3Button, tab4Button, btnInicio;
      private Button buttonGuardar, buttonFinalizar;

      private TimePicker timePickerApertura, timePickerCierre;
      private FrameLayout containerFachadaList, containerFachadaSubList, containerFachadaGuardarList;
      private TextView textViewApertura, textViewCancelar, textViewCierre, textViewEspacio, textViewEditar, textViewAgregar, textViewEditarCancelar,
          textViewEditarOk, textViewSinHorarios, textViewFachadasEspacio, textViewFachadaNumeroCalleCont;
      private LinearLayout linearBtnGuardar, linearBtnFinalizar, linearApertura, linearHorariosAgregar, linearBtnSiguiente, linearHorariosEditar, linearFachadasHorariosNumero,
          linearTxtSpaceSwitch1, linearSwitchComercio, linearTxtSpaceSwitch2, linearSwitchAnuncio;
      private EditText editTextFachadaNumeroCalle;
      private Switch switchFachadaComercio, switchFachadaAnuncio;
      private Color selectedColor, deselectedColor;
      private ListView list, guardarList;
      List<MenusTableItem> recuperarListAdapter = new List<MenusTableItem>();
      public static List<MenusEditItem> recuperarEditarListAdapter = new List<MenusEditItem>();
      private string[,] recuperarHorarios = new string[20, 3];
      private bool editar = false, eliminar = false;

      private int posicionActual = 0;

      DrawerLayout mDrawerLayout;
      List<String> mLeftItems = new List<string>();
      ListView mLeftDrawer;
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);
         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.FachadasTextos);
         data = new DataService();

         //MenuLateral
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         // Create your application here
         tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_fachadas_icon);
         tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_fachadas_icon);
         tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_fachadas_icon);
         btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

         buttonGuardar = FindViewById<Button>(Resource.Id.FachadasGuardarBtn);
         buttonFinalizar = FindViewById<Button>(Resource.Id.FachadasFinalizarBtn);

         containerFachadaGuardarList = this.FindViewById<FrameLayout>(Resource.Id.containerFachadaHorGuaList);

         textViewApertura = this.FindViewById<TextView>(Resource.Id.textViewFachadasApertura);
         textViewCancelar = this.FindViewById<TextView>(Resource.Id.textViewFachadasCancelar);
         textViewEspacio = this.FindViewById<TextView>(Resource.Id.textViewFachadasEspacio);
         textViewEditar = this.FindViewById<TextView>(Resource.Id.textViewFachadasEditar);
         textViewAgregar = this.FindViewById<TextView>(Resource.Id.textViewFachadasAgregar);
         textViewEditarOk = this.FindViewById<TextView>(Resource.Id.textViewFachadasEditarOk);
         textViewEditarCancelar = this.FindViewById<TextView>(Resource.Id.textViewFachadasEditarCancelar);
         textViewSinHorarios = this.FindViewById<TextView>(Resource.Id.textViewFachadasSinHorarios);
         textViewFachadasEspacio = this.FindViewById<TextView>(Resource.Id.textViewFachadasEspacio);
         textViewFachadaNumeroCalleCont = this.FindViewById<TextView>(Resource.Id.textViewFachadaNumeroCalleCont);
         editTextFachadaNumeroCalle = this.FindViewById<EditText>(Resource.Id.editTextFachadaNumeroCalle);
         switchFachadaComercio = this.FindViewById<Switch>(Resource.Id.switchFachadaComercio);
         switchFachadaAnuncio = this.FindViewById<Switch>(Resource.Id.switchFachadaAnuncio);

         linearBtnGuardar = this.FindViewById<LinearLayout>(Resource.Id.linearFachadasBtnGuardar);
         linearBtnFinalizar = this.FindViewById<LinearLayout>(Resource.Id.linearFachadasBtnFinalizar);
         linearHorariosAgregar = this.FindViewById<LinearLayout>(Resource.Id.linearFachadasHorariosAgregar);
         linearApertura = this.FindViewById<LinearLayout>(Resource.Id.linearFachadasApertura);
         linearHorariosEditar = this.FindViewById<LinearLayout>(Resource.Id.linearFachadasHorariosEditar);
         linearFachadasHorariosNumero = this.FindViewById<LinearLayout>(Resource.Id.linearFachadasHorariosNumero);
         linearTxtSpaceSwitch1 = this.FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch1);
         linearSwitchComercio = this.FindViewById<LinearLayout>(Resource.Id.linearSwitchComercio);
         linearTxtSpaceSwitch2 = this.FindViewById<LinearLayout>(Resource.Id.linearTxtSpaceSwitch2);
         linearSwitchAnuncio = this.FindViewById<LinearLayout>(Resource.Id.linearSwitchAnuncio);

         textViewSinHorarios.Visibility = ViewStates.Gone;
         editTextFachadaNumeroCalle.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;

         editTextFachadaNumeroCalle.Text = GlobalVariables.FachadaNumber;
         switchFachadaComercio.Checked = GlobalVariables.FachadaIsCommerce;
         switchFachadaAnuncio.Checked = GlobalVariables.FachadaIsHomologated;

         list = this.FindViewById<ListView>(Resource.Id.listViewFachadaHorGuaList);
         list.ItemClick += OnListItemGuardarClick;
         guardarList = this.FindViewById<ListView>(Resource.Id.listViewFachadaHorGuaList);

         showViewStatus();

         selectedColor = Color.ParseColor("#ffffff"); //The color u want    
         deselectedColor = Color.ParseColor("#b2d4ac");

         deselectAll();
         tab3Button.SetColorFilter(selectedColor);

         showView(false);

         buttonFinalizar.Click += showBtnFinalizar;

         btnInicio.Click += delegate //
         {
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
         };

         editTextFachadaNumeroCalle.TextChanged += (object sender, TextChangedEventArgs e) =>
         {
            int numChar = 300 - editTextFachadaNumeroCalle.Length();
            textViewFachadaNumeroCalleCont.Text = numChar.ToString();
            GlobalVariables.FachadaNumber = editTextFachadaNumeroCalle.Text;
            if (numChar < 300) linearBtnGuardar.Visibility = ViewStates.Visible;
            else linearBtnGuardar.Visibility = ViewStates.Gone;
         };

         switchFachadaComercio.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
         {
            GlobalVariables.FachadaIsCommerce = e.IsChecked;
         };

         switchFachadaAnuncio.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
         {
            if (switchFachadaComercio.Checked)
               GlobalVariables.FachadaIsHomologated = e.IsChecked;
            else
               switchFachadaAnuncio.Checked = false;
         };

         tab1Button.Click += delegate
         {
            deselectAll();
            tab1Button.SetColorFilter(selectedColor);
            var intent = new Intent(this, typeof(FachadasActivity));
            StartActivity(intent);

         };

         tab2Button.Click += delegate
         {
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(FachadasFotosActivity));
            StartActivity(intent);
         };

         textViewAgregar.Click += delegate
         {
            showView(false);
            posicionActual = GlobalVariables.fachadaHorMenuListAdapter.Count();
            linearBtnGuardar.Visibility = ViewStates.Gone;
            linearBtnFinalizar.Visibility = ViewStates.Gone;//
            textViewCancelar.Visibility = ViewStates.Visible;
            inicializarValores();
         };

         textViewEditar.Click += delegate
         {
            if (!GlobalVariables.fachadaEditarListAdapter.Any())
               Toast.MakeText(ApplicationContext, "No existen elementos para editar", ToastLength.Long).Show();
            else
            {
               linearHorariosEditar.Visibility = ViewStates.Visible;
               linearHorariosAgregar.Visibility = ViewStates.Gone;
               list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
               eliminar = false;
            }
            linearBtnGuardar.Visibility = ViewStates.Visible; //
         };

         textViewEditarOk.Click += delegate
         {
            if (eliminar) //
            {
               GlobalVariables.fachadaEditarListAdapter.ElementAt(posicionActual).IsDelete = false; //
               guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true); //
            } //
            else //
            { //
               linearHorariosEditar.Visibility = ViewStates.Gone;
               linearHorariosAgregar.Visibility = ViewStates.Visible;
               list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, false);
               showViewStatus();
            }
            editar = false;
            eliminar = false;

         };

         textViewEditarCancelar.Click += delegate
         {

            if (eliminar)
            {
               GlobalVariables.fachadaHorMenuListAdapter = recuperarListAdapter.ToList();
               GlobalVariables.fachadaEditarListAdapter = recuperarEditarListAdapter.ToList();
               GlobalVariables.fachadasHorHorarios = recuperarHorarios;
               list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
            }
            else
            {
               linearHorariosEditar.Visibility = ViewStates.Gone;
               linearHorariosAgregar.Visibility = ViewStates.Visible;
               list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, false);
            }
            editar = false;
            eliminar = false;
         };

         buttonGuardar.Click += delegate
         {
            string diasfachada = GlobalVariables.FachadaNumber; ;

            if (diasfachada == null || diasfachada.Trim() == "")
               Toast.MakeText(ApplicationContext, "Ingrese el número de calle", ToastLength.Long).Show();
            else
            {
               showView(true);
               if (!GlobalVariables.FachadaIsCommerce) switchFachadaAnuncio.Checked = false;
               if (editar)
               {
                  GlobalVariables.fachadaEditarListAdapter.ElementAt(posicionActual).Text.Text = GlobalVariables.FachadaNumber;
                  GlobalVariables.fachadaEditarListAdapter.ElementAt(posicionActual).SubHeading.Text = GlobalVariables.FachadaIsCommerce ? OpcionesMenus.TipoFachada[1] : OpcionesMenus.TipoFachada[0];
                  GlobalVariables.fachadaEditarListAdapter.ElementAt(posicionActual).SubSubHeading.Text = GlobalVariables.FachadaIsCommerce ? ((GlobalVariables.FachadaIsHomologated) ? OpcionesMenus.TipoFachadaComercio[0] : "") : "";
                  list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
               }
               else
               {
                  TextView tempText = new TextView(this);
                  TextView tempSubText = new TextView(this);
                  TextView tempSubSubText = new TextView(this);
                  tempText.Text = GlobalVariables.FachadaNumber;
                  tempSubText.Text = GlobalVariables.FachadaIsCommerce ? OpcionesMenus.TipoFachada[1] : OpcionesMenus.TipoFachada[0];
                  tempSubSubText.Text = GlobalVariables.FachadaIsCommerce ? ((GlobalVariables.FachadaIsHomologated) ? OpcionesMenus.TipoFachadaComercio[0] : "") : ""; ;
                  GlobalVariables.fachadaEditarListAdapter.Add(new MenusEditItem() { Text = tempText, SubHeading = tempSubText, SubSubHeading = tempSubSubText });
                  posicionActual = GlobalVariables.fachadaEditarListAdapter.Count() - 1;
                  list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, false);
               }

               textViewSinHorarios.Visibility = ViewStates.Gone;
               linearBtnFinalizar.Visibility = ViewStates.Visible;

               GlobalVariables.fachadasHorHorarios[posicionActual, 0] = GlobalVariables.FachadaNumber;
               GlobalVariables.fachadasHorHorarios[posicionActual, 1] = GlobalVariables.FachadaIsCommerce.ToString();
               GlobalVariables.fachadasHorHorarios[posicionActual, 2] = GlobalVariables.FachadaIsHomologated.ToString();
               inicializarValores();
            }
         };

         textViewCancelar.Click += delegate
         {
            showView(true);
            if (editar)
            {
               linearHorariosEditar.Visibility = ViewStates.Visible;
               linearHorariosAgregar.Visibility = ViewStates.Gone;
               list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
            }
            else
            {
               list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, false);
            }
         };

      }

      private void inicializarValores()
      {
         editTextFachadaNumeroCalle.Text = "";
         switchFachadaComercio.Checked = false;
         switchFachadaAnuncio.Checked = false;
         editar = false;
         eliminar = false;
      }
      private void showBtnFinalizar(object sender, EventArgs e)
      {
         if (!GlobalVariables.fachadaEditarListAdapter.Any())
            Toast.MakeText(ApplicationContext, "No existen fachadas para enviar", ToastLength.Long).Show();
         else
         {
            Fachadas fachada = new Fachadas()
            {
               Id = Guid.NewGuid().ToString(),
               Latitud = GlobalVariables.LatitudFachada,
               Longitud = GlobalVariables.LongitudFachada,
               Number = GlobalVariables.FachadaNumber,
               IsCommerce = GlobalVariables.FachadaIsCommerce,
               IsHomologated = GlobalVariables.FachadaIsHomologated,
               Enviado = "",
               FotoName = GlobalVariables.FacadePhotoName
            };
            data.SaveData(fachada);

            GlobalVariables.fachadaNuevaCaptura = true;
            Toast.MakeText(this, "Listo!", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);

         }
      }

      private void fillMenuListAdapter()
      {
         GlobalVariables.fachadaHorMenuListAdapter.Clear();
         foreach (MenusEditItem item in GlobalVariables.fachadaEditarListAdapter)
         {
            GlobalVariables.fachadaHorMenuListAdapter.Add(new MenusTableItem() { Heading = item.Text.Text, SubHeading = item.SubHeading.Text, SubSubHeading = item.SubSubHeading.Text, ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco });
         }
         list.Adapter = new MenuListAdapter(this, GlobalVariables.fachadaHorMenuListAdapter);
      }
      private void OnListItemGuardarClick(object sender, AdapterView.ItemClickEventArgs e)
      {
      }

      private void deselectAll()
      {
         tab1Button.SetColorFilter(deselectedColor);
         tab2Button.SetColorFilter(deselectedColor);
         tab3Button.SetColorFilter(deselectedColor);
      }

      protected override void OnResume()
      {
         base.OnResume();
         deselectAll();
         tab3Button.SetColorFilter(selectedColor);
         int numChar = 300 - editTextFachadaNumeroCalle.Length();
         textViewFachadaNumeroCalleCont.Text = numChar.ToString();
         GlobalVariables.FachadaNumber = editTextFachadaNumeroCalle.Text;
         list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, false);
         showViewStatus();
      }

      private void showViewStatus()
      {
         if (!GlobalVariables.fachadaHorMenuListAdapter.Any())
         {
            showView(false);
            linearBtnFinalizar.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            int numChar = 300 - editTextFachadaNumeroCalle.Length();
            if (numChar < 300)
               linearBtnGuardar.Visibility = ViewStates.Visible;
            else
               linearBtnGuardar.Visibility = ViewStates.Gone;
         }
         else
         {
            showView(true);
            linearBtnFinalizar.Visibility = ViewStates.Visible;
         }
      }

      private void showView(Boolean vistaPrincipal)
      {
         var state1 = ViewStates.Visible;
         var state2 = ViewStates.Gone;

         if (!vistaPrincipal)
         {
            state1 = ViewStates.Gone;
            state2 = ViewStates.Visible;
         }


         //linearBtnFinalizar.Visibility = state1;
         containerFachadaGuardarList.Visibility = state1;
         linearHorariosAgregar.Visibility = state1;

         linearHorariosEditar.Visibility = ViewStates.Gone;
         //linearBtnGuardar.Visibility = state2;
         linearApertura.Visibility = state2;
         linearFachadasHorariosNumero.Visibility = state2;
         editTextFachadaNumeroCalle.Visibility = state2;
         linearTxtSpaceSwitch1.Visibility = state2;
         linearSwitchComercio.Visibility = state2;
         linearTxtSpaceSwitch2.Visibility = state2;
         linearSwitchAnuncio.Visibility = state2;
         textViewFachadasEspacio.Visibility = state2;

         if (editar & vistaPrincipal)
         {
            linearHorariosEditar.Visibility = ViewStates.Visible;
            linearHorariosAgregar.Visibility = ViewStates.Gone;
         }
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

      internal void btnRemoveHorarioClick(int position)
      {
         //cambiar
         posicionActual = position;
         GlobalVariables.fachadaEditarListAdapter.ElementAt(posicionActual).IsDelete = true;
         guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
         eliminar = true;
         //
      }

      internal void btnConfirmRemoveHorarioClick(int position)
      {
         posicionActual = position;
         GlobalVariables.fachadaEditarListAdapter.RemoveAt(posicionActual);
         string[,] tempHorarioArray = new string[GlobalVariables.fachadasHorHorarios.GetLength(0), GlobalVariables.fachadasHorHorarios.GetLength(1)];
         for (int item = 0; item < GlobalVariables.fachadasHorHorarios.GetLength(0); item++)
         {
            for (int subIndice = 0; subIndice < GlobalVariables.fachadasHorHorarios.GetLength(1); subIndice++)
            {
               recuperarHorarios[item, subIndice] = GlobalVariables.fachadasHorHorarios[item, subIndice];
               if (item == posicionActual) { }
               else if (item < posicionActual)
               {
                  tempHorarioArray[item, subIndice] = GlobalVariables.fachadasHorHorarios[item, subIndice];
               }
               else
               {
                  tempHorarioArray[item - 1, subIndice] = GlobalVariables.fachadasHorHorarios[item, subIndice];
               }
            }
         }
         GlobalVariables.fachadasHorHorarios = tempHorarioArray;
         fillMenuListAdapter();
         list.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
         eliminar = false;
         showViewStatus();
      }

      internal void btnEditHorarioClick(int position)
      {
         if (eliminar) //
         {
            GlobalVariables.fachadaEditarListAdapter.ElementAt(posicionActual).IsDelete = false;
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.fachadaEditarListAdapter, (int)GlobalVariables.Modulos.Fachada, true);
         }
         else //
         {
            posicionActual = position;
            showView(false);
            editar = true;
            editTextFachadaNumeroCalle.Text = GlobalVariables.fachadasHorHorarios[posicionActual, 0];
            switchFachadaComercio.Checked = System.Convert.ToBoolean(GlobalVariables.fachadasHorHorarios[posicionActual, 1]);
            switchFachadaAnuncio.Checked = System.Convert.ToBoolean(GlobalVariables.fachadasHorHorarios[posicionActual, 2]);
            linearHorariosAgregar.Visibility = ViewStates.Gone;
            linearHorariosEditar.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Visible;
         }
         eliminar = false;
      }
   }
}