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
   //, Theme = "@style/MyTheme.ListFont", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
    [Activity(Label = "AgenciasHorariosActivity")]
    public class AgenciasHorariosActivity : Activity
    {
        private ImageButton tab1Button, tab2Button, tab3Button, btnInicio;
        private Button buttonGuardar, buttonRegresar, buttonSiguiente;

        private FrameLayout containerAgenciasHorList, containerAgenciasHorSubList, containerAgenciasHorGuaList;
        private TextView textViewApertura, textViewCancelar, textViewCierre, textViewEspacio, textViewEditar,
            textViewAgregar, textViewEditarCancelar, textViewEditarOk, textViewSinHorarios, textTimeApertura,
            textTimeCierre;
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
        private bool eliminar = false; // Línea agregada
        private int posicionActual = 0;
        // Timepicker Dialog
        private const int APERTURA_TIME_DIALOG_ID = 1;
        private const int CIERRE_TIME_DIALOG_ID = 1;
        int hour, hourApertura, hourCierre;
        int minute, minuteApertura, minuteCierre;
        private int activePicker = 0;
        //
        TimeZoneInfo timeZone;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.AgenciasHorarios);

            // Create your application here
            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_agencias_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_agencias_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_agencias_icon);
            btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio);

            buttonGuardar = FindViewById<Button>(Resource.Id.AgenciasGuardarBtn);
            buttonRegresar = FindViewById<Button>(Resource.Id.AgenciasRegresarBtn);
            buttonSiguiente = FindViewById<Button>(Resource.Id.AgenciasSiguienteBtn);

            containerAgenciasHorList = this.FindViewById<FrameLayout>(Resource.Id.containerAgenciasHorList);
            containerAgenciasHorSubList = this.FindViewById<FrameLayout>(Resource.Id.containerAgenciasHorSubList);
            containerAgenciasHorGuaList = this.FindViewById<FrameLayout>(Resource.Id.containerAgenciasHorGuaList);

            textViewApertura = this.FindViewById<TextView>(Resource.Id.textViewApertura);
            textViewCancelar = this.FindViewById<TextView>(Resource.Id.textViewCancelar);
            textViewCierre = this.FindViewById<TextView>(Resource.Id.textViewCierre);
            textViewEspacio = this.FindViewById<TextView>(Resource.Id.textViewEspacio);
            textViewEditar = this.FindViewById<TextView>(Resource.Id.textViewEditar);
            textViewAgregar = this.FindViewById<TextView>(Resource.Id.textViewAgregar);
            textViewEditarOk = this.FindViewById<TextView>(Resource.Id.textViewEditarOk);
            textViewEditarCancelar = this.FindViewById<TextView>(Resource.Id.textViewEditarCancelar);
            textViewSinHorarios = this.FindViewById<TextView>(Resource.Id.textViewSinHorarios);
            textTimeApertura = this.FindViewById<TextView>(Resource.Id.textTimeApertura); //*
            textTimeCierre = this.FindViewById<TextView>(Resource.Id.textTimeCierre); //*

            linearBtnGuardar = this.FindViewById<LinearLayout>(Resource.Id.linearBtnGuardar);
            linearBtnRegresar = this.FindViewById<LinearLayout>(Resource.Id.linearBtnRegresar);
            linearBtnSiguiente = this.FindViewById<LinearLayout>(Resource.Id.linearBtnSiguiente);
            linearHorarios = this.FindViewById<LinearLayout>(Resource.Id.linearHorarios);
            linearApertura = this.FindViewById<LinearLayout>(Resource.Id.linearApertura);
            linearHorariosEditar = this.FindViewById<LinearLayout>(Resource.Id.linearHorariosEditar);

            var sparseArray = FindViewById<ListView>(Resource.Id.listViewAgenciasHorSubList).CheckedItemPositions;

            textViewSinHorarios.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            buttonRegresar.Text = "Regresar";

            list = this.FindViewById<ListView>(Resource.Id.listViewAgenciasHorList);
            guardarList = this.FindViewById<ListView>(Resource.Id.listViewAgenciasHorGuaList);

            if (!GlobalVariables.agenciasHorDiasListAdapter.Any())
                GlobalVariables.agenciasHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });

            if (!GlobalVariables.agenciasHorMenuListAdapter.Any())
                textViewSinHorarios.Visibility = ViewStates.Visible;

            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.agenciasHorMenuListAdapter);

            guardarList.ItemClick += OnListItemGuardarClick;

            string colorActivo = Resource.Color.white.ToString();
            string colorInactivo = Resource.Color.white_inactive.ToString();

            selectedColor = Color.ParseColor("#ffffff"); //The color u want    
            deselectedColor = Color.ParseColor("#b7ddf5");
            timeZone = TimeZoneInfo.Local;

            showViewHorario();
            inicializarSpinner();

            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            buttonSiguiente.Click += showTab3;
            buttonRegresar.Click += showViewBack;

            // Timepicker Dialog
            textTimeApertura.Click += activePickerApertura;
            textTimeCierre.Click += activePickerCierre;
            UpdateDisplay(activePicker);

            btnInicio.Click += delegate
            {
                var intent = new Intent(this, typeof(MenuHomeActivity));
                StartActivity(intent);
            };


            textViewAgregar.Click += delegate
            {
                posicionActual = GlobalVariables.agenciasHorMenuListAdapter.Count();
                showViewAgregar();

                editar = false;
                GlobalVariables.agenciasHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
                GlobalVariables.agenciasHorDiasListAdapter.Clear();
                GlobalVariables.agenciasHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
                list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.agenciasHorDiasListAdapter);
                list.ItemClick += OnListItemClick;
                buttonGuardar.Visibility = ViewStates.Gone; //
                inicializarSpinner(); //
            };

            textViewEditar.Click += delegate
            {
                if (!GlobalVariables.agenciasHorMenuListAdapter.Any())
                    Toast.MakeText(ApplicationContext, "No existen elementos para editar", ToastLength.Long).Show();
                else
                {
                    linearHorariosEditar.Visibility = ViewStates.Visible;
                    linearHorarios.Visibility = ViewStates.Gone;
                    GlobalVariables.agenciasEditarListAdapter.Clear();

                    foreach (MenusTableItem item in GlobalVariables.agenciasHorMenuListAdapter)
                    {
                        TextView tempText = new TextView(this);
                        TextView tempSubText = new TextView(this);
                        tempText.Text = item.Heading;
                        tempSubText.Text = item.SubHeading;
                        GlobalVariables.agenciasEditarListAdapter.Add(new MenusEditItem() { Text = tempText, SubHeading = tempSubText });
                    }
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
                    eliminar = false;  //Línea adicional
                }
                buttonGuardar.Visibility = ViewStates.Visible;
            };

            textViewEditarOk.Click += delegate
            {
                if (eliminar) //
                {
                    GlobalVariables.agenciasEditarListAdapter.ElementAt(posicionActual).IsDelete = false; //
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true); //
                } //
                else //
                { //
                    fillMenuListAdapter();
                    linearHorariosEditar.Visibility = ViewStates.Gone;
                    linearHorarios.Visibility = ViewStates.Visible;
                    GlobalVariables.agenciasHorMenuListAdapter.ForEach(delegate (MenusTableItem item)
                    {
                        item.ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco;
                       item.ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco;
                    });

                    guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.agenciasHorMenuListAdapter);
                    if (!GlobalVariables.agenciasHorMenuListAdapter.Any())
                    { //
                        textViewSinHorarios.Visibility = ViewStates.Visible; //
                        buttonSiguiente.Visibility = ViewStates.Gone; //
                    } //
                    else buttonSiguiente.Visibility = ViewStates.Visible; //

                    GlobalVariables.agenciasHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                    reiniciaDiasSeleccionados();
                } //
                editar = false; // 
                eliminar = false; //
            };

            textViewEditarCancelar.Click += delegate
            {
                if (eliminar) //
                { //
                    GlobalVariables.agenciasEditarListAdapter.ElementAt(posicionActual).IsDelete = false;
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
                } //
                else //
                { //
                    linearHorariosEditar.Visibility = ViewStates.Gone;
                    linearHorarios.Visibility = ViewStates.Visible;
                    guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.agenciasHorMenuListAdapter);
                    GlobalVariables.agenciasHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                    reiniciaDiasSeleccionados();
                } //
                editar = false; // 
                eliminar = false; //
            };

            buttonGuardar.Click += delegate
            {
                string diasAgencias = GlobalVariables.agenciasHorDiasListAdapter.ElementAt(0).SubHeading;

                if (diasAgencias == null || diasAgencias.Trim() == "")
                    Toast.MakeText(ApplicationContext, "Seleccione los días de apertura", ToastLength.Long).Show();
                else
                {
                    GuardarCancelarButtons();
                   
                    string timeApertura = string.Format("{0}:{1}", hourApertura, minuteApertura.ToString().PadLeft(2, '0'));
                    string timeCierre = string.Format("{0}:{1}", hourCierre, minuteCierre.ToString().PadLeft(2, '0'));
                    if (editar)
                    {
                        linearHorarios.Visibility = ViewStates.Gone;
                        linearHorariosEditar.Visibility = ViewStates.Visible;
                        GlobalVariables.agenciasEditarListAdapter.ElementAt(posicionActual).Text.Text = timeApertura + " a " + timeCierre;
                        GlobalVariables.agenciasEditarListAdapter.ElementAt(posicionActual).SubHeading.Text = diasAgencias;
                        guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
                        editar = false;
                    }
                    else
                    {
                        GlobalVariables.agenciasHorMenuListAdapter.Add(new MenusTableItem() { Heading = timeApertura + " a " + timeCierre, SubHeading = diasAgencias, ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco });
                        posicionActual = GlobalVariables.agenciasHorMenuListAdapter.Count() - 1;
                        guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.agenciasHorMenuListAdapter);
                    }

                    textViewSinHorarios.Visibility = ViewStates.Gone;
                    buttonSiguiente.Visibility = ViewStates.Visible;

                    GlobalVariables.agenciasHorHorarios[posicionActual, 0] = hourApertura;
                    GlobalVariables.agenciasHorHorarios[posicionActual, 1] = minuteApertura;
                    GlobalVariables.agenciasHorHorarios[posicionActual, 2] = hourCierre;
                    GlobalVariables.agenciasHorHorarios[posicionActual, 3] = minuteCierre;
                    GlobalVariables.agenciasHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
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
                    editar = false; //
                }
                GlobalVariables.agenciasHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
            };

            tab1Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);
                var intent = new Intent(this, typeof(AgenciasActivity));
                StartActivity(intent);

            };

            tab3Button.Click += showTab3;
        }

        // Timepicker Dialog
        private void activePickerApertura(object sender, EventArgs e)
        {
            activePicker = 1;
            ShowDialog(APERTURA_TIME_DIALOG_ID);
        }

        // Timepicker Dialog
        private void activePickerCierre(object sender, EventArgs e)
        {
            activePicker = 2;
            ShowDialog(CIERRE_TIME_DIALOG_ID);
        }

        // Timepicker Dialog
        private void UpdateDisplay(int picker)
        {
            string time;
            switch (picker)
            {
                case 0:
                    time = string.Format("{0}:{1}", hourApertura.ToString().PadLeft(2, '0'), minuteApertura.ToString().PadLeft(2, '0'));
                    textTimeApertura.Text = time;
                    time = string.Format("{0}:{1}", hourCierre.ToString().PadLeft(2, '0'), minuteCierre.ToString().PadLeft(2, '0'));
                    textTimeCierre.Text = time;
                    break;
                case 1:
                    time = string.Format("{0}:{1}", hourApertura.ToString().PadLeft(2, '0'), minuteApertura.ToString().PadLeft(2, '0'));
                    textTimeApertura.Text = time;
                    break;
                case 2:
                    time = string.Format("{0}:{1}", hourCierre.ToString().PadLeft(2, '0'), minuteCierre.ToString().PadLeft(2, '0'));
                    textTimeCierre.Text = time;
                    break;
            }
            activePicker = 0;
            showBtnGuardar();
        }

        // Timepicker Dialog
        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            if (activePicker == 1)
            {
                hourApertura = e.HourOfDay;
                minuteApertura = e.Minute;
            }
            else if (activePicker == 2)
            {
                hourCierre = e.HourOfDay;
                minuteCierre = e.Minute;
            }
            UpdateDisplay(activePicker);
        }

        // Timepicker Dialog
        protected override Dialog OnCreateDialog(int id)
        {
            if (id == APERTURA_TIME_DIALOG_ID)
            {
                return new TimePickerDialog(this, TimePickerCallback, hourApertura, minuteApertura, true);
            }
            else if (id == CIERRE_TIME_DIALOG_ID)
            {
                return new TimePickerDialog(this, TimePickerCallback, hourCierre, minuteCierre, true);
            }

            return null;
        }

        // Agregado
        private void showBtnGuardar()
        {
            string dias = GlobalVariables.agenciasHorDiasListAdapter.ElementAt(0).SubHeading; //
            if (!string.IsNullOrWhiteSpace(dias) && hourCierre > hourApertura)
            {
                buttonGuardar.Visibility = ViewStates.Visible;
            }//
            else buttonGuardar.Visibility = ViewStates.Gone; //
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var position = e.Position;

            linearApertura.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            buttonSiguiente.Visibility = ViewStates.Gone;
            containerAgenciasHorList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            textViewCierre.Visibility = ViewStates.Gone;
            textViewEspacio.Visibility = ViewStates.Gone;
            containerAgenciasHorList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            linearBtnSiguiente.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            containerAgenciasHorGuaList.Visibility = ViewStates.Gone;
            // TimePicker Dialog
            textTimeApertura.Visibility = ViewStates.Gone;
            textTimeCierre.Visibility = ViewStates.Gone;

            linearBtnRegresar.Visibility = ViewStates.Visible;
            buttonRegresar.Visibility = ViewStates.Visible;
            containerAgenciasHorSubList.Visibility = ViewStates.Visible;

            //Activar los elementos seleccionados
            InicializarDiasSeleccionados();
            changeTextBtnRegresar(); //
        }

        private void InicializarDiasSeleccionados()
        {
            var subListAdapter = new ArrayAdapter<String>(this, GlobalVariables.SimpleListItemChecked, OpcionesMenus.Dias);
            var subList = this.FindViewById<ListView>(Resource.Id.listViewAgenciasHorSubList);

            subList.Adapter = subListAdapter;
            subList.ChoiceMode = ChoiceMode.Multiple;
            subList.ItemClick += OnSubListItemClick;

            //Activar los elementos seleccionados
            int initialSelection = 0;
            foreach (bool item in GlobalVariables.agenciasHorDiasSemana)
            {
                subList.SetItemChecked(initialSelection, item);
                initialSelection++;
            }
        }

        private void inicializarSpinner()
        {
            // TimePicker Dialog
            hourApertura = 0;
            minuteApertura = 0;
            hourCierre = 0;
            minuteCierre = 0;
            activePicker = 0;
            UpdateDisplay(activePicker);
        }

        public void changeTextBtnRegresar()
        {
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewAgenciasHorSubList).CheckedItemPositions;
            string text = "Regresar";
            if (sparseArray != null)
                for (int x = 0; x < sparseArray.Size(); x++)
                {
                    if (sparseArray.ValueAt(x))
                        text = "Guardar";
                }
            buttonRegresar.Text = text;
        }

        public void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            changeTextBtnRegresar();
        }

        public void OnListItemGuardarClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        private void GuardarCancelarButtons()
        {
            linearApertura.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            containerAgenciasHorGuaList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            textViewCierre.Visibility = ViewStates.Gone;
            textViewEspacio.Visibility = ViewStates.Gone;
            containerAgenciasHorList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;

            linearHorariosEditar.Visibility = ViewStates.Gone;
            linearBtnRegresar.Visibility = ViewStates.Gone;
            buttonRegresar.Visibility = ViewStates.Gone;
            containerAgenciasHorSubList.Visibility = ViewStates.Gone;
            containerAgenciasHorList.Visibility = ViewStates.Gone;
            // TimePicker Dialog
            textTimeApertura.Visibility = ViewStates.Gone;
            textTimeCierre.Visibility = ViewStates.Gone;

            linearHorarios.Visibility = ViewStates.Visible;
            //buttonSiguiente.Visibility = ViewStates.Visible;
            linearBtnSiguiente.Visibility = ViewStates.Visible;

        }

        private void deselectAll()
        {
            tab1Button.SetColorFilter(deselectedColor);
            tab2Button.SetColorFilter(deselectedColor);
            tab3Button.SetColorFilter(deselectedColor);
           
        }

        private void showTab3(object sender, EventArgs e)
        {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(AgenciasTextosActivity));
           StartActivity(intent);
        }

        private void showViewBack(object sender, EventArgs e)
        {
            showViewAgregar();
            if (!GlobalVariables.agenciasHorMenuListAdapter.Any()) //
            {
                textViewCancelar.Visibility = ViewStates.Gone; //
            }
        }
        private void showViewAgregar()
        {
            linearApertura.Visibility = ViewStates.Visible;
            linearBtnGuardar.Visibility = ViewStates.Visible;
            buttonGuardar.Visibility = ViewStates.Visible;
            containerAgenciasHorList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Visible;
            textViewCancelar.Visibility = ViewStates.Visible;
            textViewCierre.Visibility = ViewStates.Visible;
            textViewEspacio.Visibility = ViewStates.Visible;
            containerAgenciasHorList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Visible;
            // TimePicker Dialog
            textTimeApertura.Visibility = ViewStates.Visible;
            textTimeCierre.Visibility = ViewStates.Visible;

            linearBtnRegresar.Visibility = ViewStates.Gone;
            buttonRegresar.Visibility = ViewStates.Gone;
            containerAgenciasHorSubList.Visibility = ViewStates.Gone;
            containerAgenciasHorGuaList.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            buttonSiguiente.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            linearBtnSiguiente.Visibility = ViewStates.Gone;

            AgregarSubHeadingDias();
        }

        private void showViewHorario()
        {
            if (!GlobalVariables.agenciasHorMenuListAdapter.Any())
            {
                showViewAgregar();
                textViewCancelar.Visibility = ViewStates.Gone;
                linearHorariosEditar.Visibility = ViewStates.Gone;
                reiniciaDiasSeleccionados();
            }
            else
            {
                GuardarCancelarButtons();
            }
        }

        private void AgregarSubHeadingDias()
        {
            diasLaborales = " ";
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewAgenciasHorSubList).CheckedItemPositions;
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
                            diasLaborales += " " + OpcionesMenus.Dias[x];
                            seleccionados++;
                        }
                        GlobalVariables.agenciasHorDiasSemana[x] = sparseArray.ValueAt(x);
                    };
                }

                for (int x = 0; x < sparseArray.Size(); x++)
                {
                    if (sparseArray.ValueAt(x))
                        GlobalVariables.agenciasHorDias[posicionActual, x] = 1;
                    else
                        GlobalVariables.agenciasHorDias[posicionActual, x] = 0;

                    GlobalVariables.agenciasHorDiasSemana[x] = sparseArray.ValueAt(x);
                };
            }
            MenusTableItem item = new MenusTableItem();
            item.SubHeading = diasLaborales;
            GlobalVariables.agenciasHorDiasListAdapter.ElementAt(0).SubHeading = item.SubHeading;
            list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.agenciasHorDiasListAdapter);
            list.ItemClick += OnListItemClick;
            showBtnGuardar();
        }

        private void reiniciaDiasSeleccionados()
        {
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewAgenciasHorSubList).CheckedItemPositions;
            if (sparseArray != null)
                sparseArray.Clear();
        }

        protected override void OnResume()
        {
            base.OnResume();
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);
            GlobalVariables.agenciasHorMenuListAdapter.ForEach(delegate (MenusTableItem item)
            {
                item.ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco;
                item.ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco;
            });
            //GuardarCancelarButtons();
            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.agenciasHorMenuListAdapter);
            showViewHorario();
        }

        private void fillMenuListAdapter()
        {
            GlobalVariables.agenciasHorMenuListAdapter.Clear();
            foreach (MenusEditItem item in GlobalVariables.agenciasEditarListAdapter)
            {
                GlobalVariables.agenciasHorMenuListAdapter.Add(new MenusTableItem() { Heading = item.Text.Text, SubHeading = item.SubHeading.Text, ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco });
            }
            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.agenciasHorMenuListAdapter);
        }

        internal void btnRemoveHorarioClick(int position)
        {
            //
            posicionActual = position;
            GlobalVariables.agenciasEditarListAdapter.ElementAt(posicionActual).IsDelete = true;
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
            eliminar = true;
            //
        }

        internal void btnConfirmRemoveHorarioClick(int position)
        {            
            posicionActual = position;
            GlobalVariables.agenciasEditarListAdapter.RemoveAt(posicionActual);
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
            int[,] tempDiasArray = new int[GlobalVariables.agenciasHorDias.GetLength(0), 7];
            int[,] tempHorarioArray = new int[GlobalVariables.agenciasHorHorarios.GetLength(0), GlobalVariables.agenciasHorHorarios.GetLength(1)];
            for (int item = 0; item < GlobalVariables.agenciasHorDias.GetLength(0); item++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.agenciasHorDias.GetLength(1); subIndice++)
                {
                    if (item == posicionActual) { }
                    else if (item < posicionActual)
                    {
                        tempDiasArray[item, subIndice] = GlobalVariables.agenciasHorDias[item, subIndice];
                    }
                    else tempDiasArray[item - 1, subIndice] = GlobalVariables.agenciasHorDias[item, subIndice];
                }

                for (int subIndice = 0; subIndice < GlobalVariables.agenciasHorHorarios.GetLength(1); subIndice++)
                {
                    if (item == posicionActual) { }
                    else if (item < posicionActual)
                    {
                        tempHorarioArray[item, subIndice] = GlobalVariables.agenciasHorHorarios[item, subIndice];
                    }
                    else
                    {
                        tempHorarioArray[item - 1, subIndice] = GlobalVariables.agenciasHorHorarios[item, subIndice];
                    }
                }
            }
            GlobalVariables.agenciasHorDias = tempDiasArray;
            GlobalVariables.agenciasHorHorarios = tempHorarioArray;
            linearHorarios.Visibility = ViewStates.Gone;
            linearHorariosEditar.Visibility = ViewStates.Visible;

            // Lineas adicionales
            fillMenuListAdapter();
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
            eliminar = true;
            //Agregado
            InicializarDiasSeleccionados();

            showViewHorario(); //
            inicializarSpinner(); //
            //
        }

        internal void btnEditHorarioClick(int position)
        {
            posicionActual = position;
            if (eliminar) //
            {
                GlobalVariables.agenciasEditarListAdapter.ElementAt(posicionActual).IsDelete = false;
                guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.agenciasEditarListAdapter, (int)GlobalVariables.Modulos.Agencia, true);
            }
            else //
            {
                guardarDiasHorarios();

                GlobalVariables.agenciasHorDiasListAdapter.Clear();
                GlobalVariables.agenciasHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });

                for (int cont = 0; cont < GlobalVariables.agenciasHorDiasSemana.Length; cont++)
                {
                    if (GlobalVariables.agenciasHorDias[posicionActual, cont] == 0)
                        GlobalVariables.agenciasHorDiasSemana[cont] = false;
                    else
                        GlobalVariables.agenciasHorDiasSemana[cont] = true;
                }
                InicializarDiasSeleccionados();
                // TimePicker Dialog
                hourApertura = (GlobalVariables.agenciasHorHorarios[posicionActual, 0]);
                minuteApertura = (GlobalVariables.agenciasHorHorarios[posicionActual, 1]);
                hourCierre = (GlobalVariables.agenciasHorHorarios[posicionActual, 2]);
                minuteCierre = (GlobalVariables.agenciasHorHorarios[posicionActual, 3]);
                activePicker = 0;
                UpdateDisplay(activePicker);

                showViewAgregar();
                AgregarSubHeadingDias();
                linearHorariosEditar.Visibility = ViewStates.Gone;
                editar = true;
            }
            eliminar = false; // Agregar línea
        }

        private void guardarDiasHorarios()
        {
            for (int indice = 0; indice < GlobalVariables.agenciasHorDias.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.agenciasHorDias.GetLength(1); subIndice++)
                {
                    recuperarDiasSemana[indice, subIndice] = GlobalVariables.agenciasHorDias[indice, subIndice];
                }
            }

            for (int indice = 0; indice < GlobalVariables.agenciasHorHorarios.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.agenciasHorHorarios.GetLength(1); subIndice++)
                {
                    recuperarHorarios[indice, subIndice] = GlobalVariables.agenciasHorHorarios[indice, subIndice];
                }
            }
        }

        private void restaurarDiasHorarios()
        {
            for (int indice = 0; indice < GlobalVariables.agenciasHorDias.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.agenciasHorDias.GetLength(1); subIndice++)
                {
                    GlobalVariables.agenciasHorDias[indice, subIndice] = recuperarDiasSemana[indice, subIndice];
                }
            }

            for (int indice = 0; indice < GlobalVariables.agenciasHorHorarios.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.agenciasHorHorarios.GetLength(1); subIndice++)
                {
                    GlobalVariables.agenciasHorHorarios[indice, subIndice] = recuperarHorarios[indice, subIndice];
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
        }
    }
}