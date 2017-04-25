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

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "MercadosHorariosActivity", Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MercadosHorariosActivity : Activity
    {
        private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
        private ImageView buttonGuardar, buttonRegresar, buttonSiguiente;
        private TimePicker timePickerApertura, timePickerCierre;
        private FrameLayout containerMercadoList, containerMercadoSubList, containerMercadoGuardarList;
        private TextView textViewApertura, textViewCancelar, textViewCierre, textViewEspacio, textViewEditar, textViewAgregar, textViewEditarCancelar, textViewEditarOk, textViewSinHorarios;
        private LinearLayout linearBtnGuardar, linearBtnRegresar, linearApertura, linearHorarios, linearBtnSiguiente, linearHorariosEditar;
        private Color selectedColor, deselectedColor;
        private ListView list, guardarList;
        List<MenusTableItem> recuperarListAdapter = new List<MenusTableItem>();
        public static List<MenusEditItem> recuperarEditarListAdapter = new List<MenusEditItem>();
        private int[,] recuperarDiasSemana = new int[20, 7];
        private int[,] recuperarHorarios = new int[20, 7];
        private string diasLaborales;
        private string[] diasApertura = { "Entre semana", "Fines de semana", "Toda la semana" };
        private bool editar = false;
        private int posicionActual = 0;
        TimeZoneInfo timeZone;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.MercadosHorarios);

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

            buttonGuardar = this.FindViewById<ImageView>(Resource.Id.MercadosGuardarBtn);
            buttonRegresar = this.FindViewById<ImageView>(Resource.Id.MercadosRegresarBtn);
            buttonSiguiente = this.FindViewById<ImageView>(Resource.Id.MercadosSiguienteBtn);

            containerMercadoList = this.FindViewById<FrameLayout>(Resource.Id.containerMercadoHorList);
            containerMercadoSubList = this.FindViewById<FrameLayout>(Resource.Id.containerMercadoHorSubList);
            containerMercadoGuardarList = this.FindViewById<FrameLayout>(Resource.Id.containerMercadoHorGuaList);

            textViewApertura = this.FindViewById<TextView>(Resource.Id.textViewApertura);
            textViewCancelar = this.FindViewById<TextView>(Resource.Id.textViewCancelar);
            textViewCierre = this.FindViewById<TextView>(Resource.Id.textViewCierre);
            textViewEspacio = this.FindViewById<TextView>(Resource.Id.textViewEspacio);
            textViewEditar = this.FindViewById<TextView>(Resource.Id.textViewEditar);
            textViewAgregar = this.FindViewById<TextView>(Resource.Id.textViewAgregar);
            textViewEditarOk = this.FindViewById<TextView>(Resource.Id.textViewEditarOk);
            textViewEditarCancelar = this.FindViewById<TextView>(Resource.Id.textViewEditarCancelar);
            textViewSinHorarios = this.FindViewById<TextView>(Resource.Id.textViewSinHorarios);

            timePickerApertura = this.FindViewById<TimePicker>(Resource.Id.timePickerApertura);
            timePickerCierre = this.FindViewById<TimePicker>(Resource.Id.timePickerCierre);

            timePickerApertura.SetIs24HourView(Java.Lang.Boolean.True);
            timePickerCierre.SetIs24HourView(Java.Lang.Boolean.True);

            linearBtnGuardar = this.FindViewById<LinearLayout>(Resource.Id.linearBtnGuardar);
            linearBtnRegresar = this.FindViewById<LinearLayout>(Resource.Id.linearBtnRegresar);
            linearBtnSiguiente = this.FindViewById<LinearLayout>(Resource.Id.linearBtnSiguiente);
            linearHorarios = this.FindViewById<LinearLayout>(Resource.Id.linearHorarios);
            linearApertura = this.FindViewById<LinearLayout>(Resource.Id.linearApertura);
            linearHorariosEditar = this.FindViewById<LinearLayout>(Resource.Id.linearHorariosEditar);

            var sparseArray = FindViewById<ListView>(Resource.Id.listViewMercadoHorSubList).CheckedItemPositions;

            textViewSinHorarios.Visibility = ViewStates.Gone;

            list = this.FindViewById<ListView>(Resource.Id.listViewMercadoHorList);
            guardarList = this.FindViewById<ListView>(Resource.Id.listViewMercadoHorGuaList);

            if (!GlobalVariables.mercadoHorDiasListAdapter.Any())
                GlobalVariables.mercadoHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });

            if (!GlobalVariables.mercadoHorMenuListAdapter.Any())
                textViewSinHorarios.Visibility = ViewStates.Visible;

            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.mercadoHorMenuListAdapter);

            guardarList.ItemClick += OnListItemGuardarClick;

            selectedColor = Color.ParseColor("#303030"); //The color u want    
            deselectedColor = Color.ParseColor("#ffffff");
            timeZone = TimeZoneInfo.Local;

            GuardarCancelarButtons();
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            buttonSiguiente.Click += showTab4;
            buttonRegresar.Click += showViewBack;

            textViewAgregar.Click += delegate
            {
                posicionActual = GlobalVariables.mercadoHorMenuListAdapter.Count();
                showViewAgregar();

                editar = false;
                GlobalVariables.mercadoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
                GlobalVariables.mercadoHorDiasListAdapter.Clear();
                GlobalVariables.mercadoHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
                list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.mercadoHorDiasListAdapter);
                list.ItemClick += OnListItemClick;
            };

            textViewEditar.Click += delegate
            {
                if (!GlobalVariables.mercadoHorMenuListAdapter.Any())
                    Toast.MakeText(ApplicationContext, "No existen elementos para editar", ToastLength.Long).Show();
                else
                {
                    recuperarListAdapter = GlobalVariables.mercadoHorMenuListAdapter.ToList();
                    recuperarEditarListAdapter = GlobalVariables.editarListAdapter.ToList();
                    guardarDiasHorarios();
                    linearHorariosEditar.Visibility = ViewStates.Visible;
                    linearHorarios.Visibility = ViewStates.Gone;
                    GlobalVariables.editarListAdapter.Clear();
                    
                    foreach (MenusTableItem item in GlobalVariables.mercadoHorMenuListAdapter)
                    {
                        TextView tempText = new TextView(this);
                        TextView tempSubText = new TextView(this);
                        tempText.Text = item.Heading;
                        tempSubText.Text = item.SubHeading;
                        GlobalVariables.editarListAdapter.Add(new MenusEditItem() { Text = tempText, SubHeading = tempSubText });
                    }
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.editarListAdapter);
                }
            };

            textViewEditarOk.Click += delegate
            {
                fillMenuListAdapter();
                linearHorariosEditar.Visibility = ViewStates.Gone;
                linearHorarios.Visibility = ViewStates.Visible;
                GlobalVariables.mercadoHorMenuListAdapter.ForEach(delegate(MenusTableItem item)
                {
                    item.ImageResourceId = Resource.Drawable.MercadosEditarEliminarVacioIco;
                    item.ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco;
                });

                guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.mercadoHorMenuListAdapter);
                if (!GlobalVariables.mercadoHorMenuListAdapter.Any())
                    textViewSinHorarios.Visibility = ViewStates.Visible;
                GlobalVariables.mercadoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
            };

            textViewEditarCancelar.Click += delegate
            {
                fillMenuListAdapter();
                GlobalVariables.mercadoHorMenuListAdapter = recuperarListAdapter.ToList();
                GlobalVariables.editarListAdapter.Clear();
                foreach (MenusTableItem item in GlobalVariables.mercadoHorMenuListAdapter)
                {
                    TextView tempText = new TextView(this);
                    TextView tempSubText = new TextView(this);
                    tempText.Text = item.Heading;
                    tempSubText.Text = item.SubHeading;
                    GlobalVariables.editarListAdapter.Add(new MenusEditItem() { Text = tempText, SubHeading = tempSubText });
                }
                restaurarDiasHorarios();
                linearHorariosEditar.Visibility = ViewStates.Gone;
                linearHorarios.Visibility = ViewStates.Visible;
                GlobalVariables.mercadoHorMenuListAdapter.ForEach(delegate(MenusTableItem item)
                {
                    item.ImageResourceId = Resource.Drawable.MercadosEditarEliminarVacioIco;
                    item.ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco;
                });

                guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.mercadoHorMenuListAdapter);
                GlobalVariables.mercadoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
            };

            buttonGuardar.Click += delegate
            {
                string diasMercado = GlobalVariables.mercadoHorDiasListAdapter.ElementAt(0).SubHeading;
               
                if (diasMercado == "" || diasMercado == null)
                    Toast.MakeText(ApplicationContext, "Seleccione los días de apertura", ToastLength.Long).Show();
                else
                {
                    GuardarCancelarButtons();

                    int hourApertura = timePickerApertura.CurrentHour.IntValue() ;
                    int minuteApertura = timePickerApertura.CurrentMinute.IntValue();
                    int hourCierre = timePickerCierre.CurrentHour.IntValue();
                    int minuteCierre = timePickerCierre.CurrentMinute.IntValue();
                    string timeApertura = string.Format("{0}:{1}", hourApertura, minuteApertura.ToString().PadLeft(2, '0'));
                    string timeCierre = string.Format("{0}:{1}", hourCierre, minuteCierre.ToString().PadLeft(2, '0'));
                    if (editar) {
                        linearHorarios.Visibility = ViewStates.Gone;
                        linearHorariosEditar.Visibility = ViewStates.Visible;
                        GlobalVariables.editarListAdapter.ElementAt(posicionActual).Text.Text = timeApertura + " a " + timeCierre;
                        GlobalVariables.editarListAdapter.ElementAt(posicionActual).SubHeading.Text = diasMercado;
                        guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.editarListAdapter);
                        editar = false;
                    }
                    else
                    {
                        GlobalVariables.mercadoHorMenuListAdapter.Add(new MenusTableItem() { Heading = timeApertura + " a " + timeCierre, SubHeading = diasMercado, ImageResourceId = Resource.Drawable.MercadosEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
                        posicionActual = GlobalVariables.mercadoHorMenuListAdapter.Count() - 1;
                        guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.mercadoHorMenuListAdapter);
                    }
                    
                    textViewSinHorarios.Visibility = ViewStates.Gone;
                    
                    GlobalVariables.mercadosHorHorarios[posicionActual, 0] = hourApertura;
                    GlobalVariables.mercadosHorHorarios[posicionActual, 1] = minuteApertura;
                    GlobalVariables.mercadosHorHorarios[posicionActual, 2] = hourCierre;
                    GlobalVariables.mercadosHorHorarios[posicionActual, 3] = minuteCierre;
                    GlobalVariables.mercadoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                    reiniciaDiasSeleccionados();
                }
            };

            textViewCancelar.Click += delegate
            {
                GuardarCancelarButtons();
                if (!editar)
                {
                    fillMenuListAdapter();
                }
                else
                {
                    linearHorarios.Visibility = ViewStates.Gone;
                    linearHorariosEditar.Visibility = ViewStates.Visible;
                    restaurarDiasHorarios();
                }
                GlobalVariables.mercadoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
            };

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

            tab4Button.Click += delegate
            {
                deselectAll();
                tab4Button.SetColorFilter(selectedColor);

                var intent = new Intent(this, typeof(MercadosTextosActivity));
                StartActivity(intent);
            };

        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var position = e.Position;

            linearApertura.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            buttonSiguiente.Visibility = ViewStates.Gone;
            containerMercadoList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            textViewCierre.Visibility = ViewStates.Gone;
            textViewEspacio.Visibility = ViewStates.Gone;
            containerMercadoList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            timePickerApertura.Visibility = ViewStates.Gone;
            timePickerCierre.Visibility = ViewStates.Gone;
            linearBtnSiguiente.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            containerMercadoGuardarList.Visibility = ViewStates.Gone;

            linearBtnRegresar.Visibility = ViewStates.Visible;
            buttonRegresar.Visibility = ViewStates.Visible;
            containerMercadoSubList.Visibility = ViewStates.Visible;

            //Activar los elementos seleccionados
            InicializarDiasSeleccionados();
        }

        private void InicializarDiasSeleccionados()
        {
            var subListAdapter = new ArrayAdapter<String>(this, GlobalVariables.SimpleListItemChecked, OpcionesMenus.DiasMercados);
            var subList = this.FindViewById<ListView>(Resource.Id.listViewMercadoHorSubList);

            subList.Adapter = subListAdapter;
            subList.ChoiceMode = ChoiceMode.Multiple;
            subList.ItemClick += OnSubListItemClick;

            //Activar los elementos seleccionados
            int initialSelection = 0;
            foreach (bool item in GlobalVariables.mercadoHorDiasSemana)
            {
                subList.SetItemChecked(initialSelection, item);
                initialSelection++;
            }
        }

        public void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
        }

        public void OnListItemGuardarClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        private void GuardarCancelarButtons() 
        {
            linearApertura.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            containerMercadoGuardarList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            textViewCierre.Visibility = ViewStates.Gone;
            textViewEspacio.Visibility = ViewStates.Gone;
            containerMercadoList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            timePickerApertura.Visibility = ViewStates.Gone;
            timePickerCierre.Visibility = ViewStates.Gone;

            linearHorariosEditar.Visibility = ViewStates.Gone;
            linearBtnRegresar.Visibility = ViewStates.Gone;
            buttonRegresar.Visibility = ViewStates.Gone;
            containerMercadoSubList.Visibility = ViewStates.Gone;
            containerMercadoList.Visibility = ViewStates.Gone;

            linearHorarios.Visibility = ViewStates.Visible;
            buttonSiguiente.Visibility = ViewStates.Visible;
            linearBtnSiguiente.Visibility = ViewStates.Visible;

            }

        private void deselectAll()
        {
            tab1Button.SetColorFilter(deselectedColor);
            tab2Button.SetColorFilter(deselectedColor);
            tab3Button.SetColorFilter(deselectedColor);
            tab4Button.SetColorFilter(deselectedColor);
        }

        private void showTab4(object sender, EventArgs e)
        {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(MercadosTextosActivity));
            StartActivity(intent);
        }

        private void showViewBack(object sender, EventArgs e)
        {
            showViewAgregar();
        }
        private void showViewAgregar()
        {
            linearApertura.Visibility = ViewStates.Visible;
            linearBtnGuardar.Visibility = ViewStates.Visible;
            buttonGuardar.Visibility = ViewStates.Visible;
            containerMercadoList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Visible;
            textViewCancelar.Visibility = ViewStates.Visible;
            textViewCierre.Visibility = ViewStates.Visible;
            textViewEspacio.Visibility = ViewStates.Visible;
            containerMercadoList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Visible;
            timePickerApertura.Visibility = ViewStates.Visible;
            timePickerCierre.Visibility = ViewStates.Visible;

            linearBtnRegresar.Visibility = ViewStates.Gone;
            buttonRegresar.Visibility = ViewStates.Gone;
            containerMercadoSubList.Visibility = ViewStates.Gone;
            containerMercadoGuardarList.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            buttonSiguiente.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            linearBtnSiguiente.Visibility = ViewStates.Gone;

            AgregarSubHeadingDias();
           
        }

        private void AgregarSubHeadingDias()
        {
            diasLaborales = " ";
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewMercadoHorSubList).CheckedItemPositions;
            if (sparseArray != null)
            {
                if (sparseArray.ValueAt(0) && sparseArray.ValueAt(1) && sparseArray.ValueAt(2) && sparseArray.ValueAt(3) && sparseArray.ValueAt(4) && sparseArray.ValueAt(5) && sparseArray.ValueAt(6))
                    diasLaborales = diasApertura[2];
                else if (sparseArray.ValueAt(1) && sparseArray.ValueAt(2) && sparseArray.ValueAt(3) && sparseArray.ValueAt(4) && sparseArray.ValueAt(5))
                    diasLaborales = diasApertura[0];
                else if (sparseArray.ValueAt(0) && sparseArray.ValueAt(6))
                    diasLaborales = diasApertura[1];               
                else
                {
                    int seleccionados = 0;
                    for (int x = 0; x < sparseArray.Size(); x++)
                    {
                        if (sparseArray.ValueAt(x))
                        {
                            diasLaborales+= " " + OpcionesMenus.DiasMercados[x];
                            seleccionados++;
                        }
                        GlobalVariables.mercadoHorDiasSemana[x] = sparseArray.ValueAt(x);
                    };
                }

                for (int x = 0; x < sparseArray.Size(); x++)
                {
                    if (sparseArray.ValueAt(x))
                        GlobalVariables.mercadoHorDias[posicionActual, x] = 1;
                    else
                        GlobalVariables.mercadoHorDias[posicionActual, x] = 0;

                    GlobalVariables.mercadoHorDiasSemana[x] = sparseArray.ValueAt(x);
                };
            }
            MenusTableItem item = new MenusTableItem();
            item.SubHeading = diasLaborales;
            GlobalVariables.mercadoHorDiasListAdapter.ElementAt(0).SubHeading = item.SubHeading;
            list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.mercadoHorDiasListAdapter);
            list.ItemClick += OnListItemClick;
        }

        private void reiniciaDiasSeleccionados()
        {
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewMercadoHorSubList).CheckedItemPositions;
                if (sparseArray != null)
                    sparseArray.Clear();
        }

        protected override void OnResume()
        {
            base.OnResume();
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);
            GlobalVariables.mercadoHorMenuListAdapter.ForEach(delegate(MenusTableItem item)
            {
                item.ImageResourceId = Resource.Drawable.MercadosEditarEliminarVacioIco;
                item.ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco;
            });
            GuardarCancelarButtons();
            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.mercadoHorMenuListAdapter);

        }

        private void fillMenuListAdapter(){
            GlobalVariables.mercadoHorMenuListAdapter.Clear();
            foreach (MenusEditItem item in GlobalVariables.editarListAdapter)
                {
                    GlobalVariables.mercadoHorMenuListAdapter.Add(new MenusTableItem() { Heading = item.Text.Text, SubHeading = item.SubHeading.Text, ImageResourceId = Resource.Drawable.MercadosEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
                }
                guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.mercadoHorMenuListAdapter);
            }

        internal void btnRemoveHorarioClick(int position)
        {
            posicionActual = position;
            GlobalVariables.editarListAdapter.RemoveAt(posicionActual);
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.editarListAdapter);
            int[,] tempDiasArray = new int[GlobalVariables.mercadoHorDias.GetLength(0), 7];
            int[,] tempHorarioArray = new int[GlobalVariables.mercadosHorHorarios.GetLength(0), GlobalVariables.mercadosHorHorarios.GetLength(1)];
            for (int item = 0; item < GlobalVariables.mercadoHorDias.GetLength(0); item++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.mercadoHorDias.GetLength(1); subIndice++)
                {
                    if (item == posicionActual) { }
                    else if (item < posicionActual)
                    {
                        tempDiasArray[item, subIndice] = GlobalVariables.mercadoHorDias[item, subIndice];
                    }
                    else tempDiasArray[item - 1, subIndice] = GlobalVariables.mercadoHorDias[item, subIndice];
                }

                for (int subIndice = 0; subIndice < GlobalVariables.mercadosHorHorarios.GetLength(1); subIndice++)
                {
                    if (item == posicionActual) { }
                    else if (item < posicionActual)
                    {
                        tempHorarioArray[item, subIndice] = GlobalVariables.mercadosHorHorarios[item, subIndice];
                    }
                    else
                    {
                        tempHorarioArray[item - 1, subIndice] = GlobalVariables.mercadosHorHorarios[item, subIndice];
                    }
                }
            }
            GlobalVariables.mercadoHorDias = tempDiasArray;
            GlobalVariables.mercadosHorHorarios = tempHorarioArray;
            linearHorarios.Visibility = ViewStates.Gone;
            linearHorariosEditar.Visibility = ViewStates.Visible;
        }

        internal void btnEditHorarioClick(int position)
        {
            posicionActual = position;

            guardarDiasHorarios();

            GlobalVariables.mercadoHorDiasListAdapter.Clear();
            GlobalVariables.mercadoHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
            
            for (int cont = 0; cont < GlobalVariables.mercadoHorDiasSemana.Length; cont++)
            {
                if (GlobalVariables.mercadoHorDias[posicionActual, cont] == 0)
                    GlobalVariables.mercadoHorDiasSemana[cont] = false;
                else
                    GlobalVariables.mercadoHorDiasSemana[cont] = true;
            }
            InicializarDiasSeleccionados();
            timePickerApertura.CurrentHour = (Java.Lang.Integer)(GlobalVariables.mercadosHorHorarios[posicionActual, 0]);
            timePickerApertura.CurrentMinute = (Java.Lang.Integer)(GlobalVariables.mercadosHorHorarios[posicionActual, 1]);
            timePickerCierre.CurrentHour = (Java.Lang.Integer)(GlobalVariables.mercadosHorHorarios[posicionActual, 2]);
            timePickerCierre.CurrentMinute = (Java.Lang.Integer)(GlobalVariables.mercadosHorHorarios[posicionActual, 3]);

            showViewAgregar();
            AgregarSubHeadingDias();
            linearHorariosEditar.Visibility = ViewStates.Gone;
            editar = true;
        }

        private void guardarDiasHorarios()
        {
            for (int indice = 0; indice < GlobalVariables.mercadoHorDias.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.mercadoHorDias.GetLength(1); subIndice++)
                {
                    recuperarDiasSemana[indice, subIndice] = GlobalVariables.mercadoHorDias[indice, subIndice];
                }
            }

            for (int indice = 0; indice < GlobalVariables.mercadosHorHorarios.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.mercadosHorHorarios.GetLength(1); subIndice++)
                {
                    recuperarHorarios[indice, subIndice] = GlobalVariables.mercadosHorHorarios[indice, subIndice];
                }
            }
        }



        private void restaurarDiasHorarios()
        {
            for (int indice = 0; indice < GlobalVariables.mercadoHorDias.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.mercadoHorDias.GetLength(1); subIndice++)
                {
                    GlobalVariables.mercadoHorDias[indice, subIndice] = recuperarDiasSemana[indice, subIndice];
                }
            }

            for (int indice = 0; indice < GlobalVariables.mercadosHorHorarios.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.mercadosHorHorarios.GetLength(1); subIndice++)
                {
                    GlobalVariables.mercadosHorHorarios[indice, subIndice] = recuperarHorarios[indice, subIndice];
                }
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