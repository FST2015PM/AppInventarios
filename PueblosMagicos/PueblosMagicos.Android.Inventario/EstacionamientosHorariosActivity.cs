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
    [Activity(Label = "EstacionamientosHorariosActivity")]
    public class EstacionamientosHorariosActivity : Activity
    {
        private ImageButton tab1Button, tab2Button, tab3Button, tab4Button, btnInicio;
        private Button buttonGuardar, buttonRegresar, buttonSiguiente;

        private FrameLayout containerEstacionamientoList, containerEstacionamientoSubList, containerEstacionamientoGuardarList;
        private TextView textViewApertura, textViewCancelar, textViewCierre, textViewEspacio, textViewEditar, textViewAgregar, 
            textViewEditarCancelar, textViewEditarOk, textViewSinHorarios, textTimeApertura,
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
            SetContentView(Resource.Layout.EstacionamientosHorarios);

            // Create your application here
            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_estacionamientos_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_estacionamientos_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_estacionamientos_icon);
            tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_estacionamientos_icon);
            btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //

            buttonGuardar = FindViewById<Button>(Resource.Id.EstacionamientosGuardarBtn);
            buttonRegresar = FindViewById<Button>(Resource.Id.EstacionamientosRegresarBtn);
            buttonSiguiente = FindViewById<Button>(Resource.Id.EstacionamientosSiguienteBtn);

            containerEstacionamientoList = this.FindViewById<FrameLayout>(Resource.Id.containerEstacionamientoHorList);
            containerEstacionamientoSubList = this.FindViewById<FrameLayout>(Resource.Id.containerEstacionamientoHorSubList);
            containerEstacionamientoGuardarList = this.FindViewById<FrameLayout>(Resource.Id.containerEstacionamientoHorGuaList);

            textViewApertura = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosApertura);
            textViewCancelar = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosCancelar);
            textViewCierre = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosCierre);
            textViewEspacio = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosEspacio);
            textViewEditar = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosEditar);
            textViewAgregar = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosAgregar);
            textViewEditarOk = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosEditarOk);
            textViewEditarCancelar = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosEditarCancelar);
            textViewSinHorarios = this.FindViewById<TextView>(Resource.Id.textViewEstacionamientosSinHorarios);
            textTimeApertura = this.FindViewById<TextView>(Resource.Id.textTimeApertura); //*
            textTimeCierre = this.FindViewById<TextView>(Resource.Id.textTimeCierre); //*

            linearBtnGuardar = this.FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosBtnGuardar);
            linearBtnRegresar = this.FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosBtnRegresar);
            linearBtnSiguiente = this.FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosBtnSiguiente);
            linearHorarios = this.FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosHorarios);
            linearApertura = this.FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosApertura);
            linearHorariosEditar = this.FindViewById<LinearLayout>(Resource.Id.linearEstacionamientosHorariosEditar);

            var sparseArray = FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorSubList).CheckedItemPositions;

            textViewSinHorarios.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone; //
            buttonRegresar.Text = "Regresar"; //

            list = this.FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorList);
            guardarList = this.FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorGuaList);

            if (!GlobalVariables.estacionamientoHorDiasListAdapter.Any())
                GlobalVariables.estacionamientoHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });

            if (!GlobalVariables.estacionamientoHorMenuListAdapter.Any())
                textViewSinHorarios.Visibility = ViewStates.Visible;

            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.estacionamientoHorMenuListAdapter);

            guardarList.ItemClick += OnListItemGuardarClick;

            selectedColor = Color.ParseColor("#ffffff"); //The color u want    
            deselectedColor = Color.ParseColor("#e9b7a0");
            timeZone = TimeZoneInfo.Local;

            showViewHorario(); //
            inicializarSpinner(); //

            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            buttonSiguiente.Click += showTab4;
            buttonRegresar.Click += showViewBack;

            // Timepicker Dialog
            textTimeApertura.Click += activePickerApertura;
            textTimeCierre.Click += activePickerCierre;
            UpdateDisplay(activePicker);

            btnInicio.Click += delegate //
            {
                var intent = new Intent(this, typeof(MenuHomeActivity));
                StartActivity(intent);
            };

            textViewAgregar.Click += delegate
            {
                posicionActual = GlobalVariables.estacionamientoHorMenuListAdapter.Count();
                showViewAgregar();

                editar = false;
                GlobalVariables.estacionamientoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
                GlobalVariables.estacionamientoHorDiasListAdapter.Clear();
                GlobalVariables.estacionamientoHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });
                list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.estacionamientoHorDiasListAdapter);
                list.ItemClick += OnListItemClick;
                buttonGuardar.Visibility = ViewStates.Gone; //
                inicializarSpinner(); //
            };

            textViewEditar.Click += delegate
            {
                if (!GlobalVariables.estacionamientoHorMenuListAdapter.Any())
                    Toast.MakeText(ApplicationContext, "No existen elementos para editar", ToastLength.Long).Show();
                else
                {
                    linearHorariosEditar.Visibility = ViewStates.Visible;
                    linearHorarios.Visibility = ViewStates.Gone;
                    GlobalVariables.estacionamientoEditarListAdapter.Clear();

                    foreach (MenusTableItem item in GlobalVariables.estacionamientoHorMenuListAdapter)
                    {
                        TextView tempText = new TextView(this);
                        TextView tempSubText = new TextView(this);
                        tempText.Text = item.Heading;
                        tempSubText.Text = item.SubHeading;
                        GlobalVariables.estacionamientoEditarListAdapter.Add(new MenusEditItem() { Text = tempText, SubHeading = tempSubText });
                    }
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true);
                    eliminar = false;  //Línea adicional
                }
                buttonGuardar.Visibility = ViewStates.Visible; //
            };

            textViewEditarOk.Click += delegate
            {
                 if (eliminar) //
                {
                    GlobalVariables.estacionamientoEditarListAdapter.ElementAt(posicionActual).IsDelete = false; //
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true); //
                } //
                else //
                { //
                fillMenuListAdapter();
                linearHorariosEditar.Visibility = ViewStates.Gone;
                linearHorarios.Visibility = ViewStates.Visible;
                GlobalVariables.estacionamientoHorMenuListAdapter.ForEach(delegate(MenusTableItem item)
                {
                    item.ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco;
                    item.ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco;
                });

                guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.estacionamientoHorMenuListAdapter);
                if (!GlobalVariables.estacionamientoHorMenuListAdapter.Any())
                { //
                    textViewSinHorarios.Visibility = ViewStates.Visible; //
                    buttonSiguiente.Visibility = ViewStates.Gone; //
                } //
                else buttonSiguiente.Visibility = ViewStates.Visible; //

                GlobalVariables.estacionamientoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
                } //
                editar = false; // 
                eliminar = false; //
            };

            textViewEditarCancelar.Click += delegate
            {
                if (eliminar) //
                { //
                    GlobalVariables.estacionamientoEditarListAdapter.ElementAt(posicionActual).IsDelete = false; //
                    guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true); //
                } //
                else //
                { //
                    linearHorariosEditar.Visibility = ViewStates.Gone;
                    linearHorarios.Visibility = ViewStates.Visible;
                    guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.estacionamientoHorMenuListAdapter);
                    GlobalVariables.estacionamientoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                    reiniciaDiasSeleccionados();
                } //
                editar = false; // 
                eliminar = false; //
            };

            buttonGuardar.Click += delegate
            {
                string diasEstacionamiento = GlobalVariables.estacionamientoHorDiasListAdapter.ElementAt(0).SubHeading;

                if (diasEstacionamiento == null || diasEstacionamiento.Trim() == "")
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
                        GlobalVariables.estacionamientoEditarListAdapter.ElementAt(posicionActual).Text.Text = timeApertura + " a " + timeCierre;
                        GlobalVariables.estacionamientoEditarListAdapter.ElementAt(posicionActual).SubHeading.Text = diasEstacionamiento;
                        guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true);
                        editar = false;
                    }
                    else
                    {
                        GlobalVariables.estacionamientoHorMenuListAdapter.Add(new MenusTableItem() { Heading = timeApertura + " a " + timeCierre, SubHeading = diasEstacionamiento, ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco });
                        posicionActual = GlobalVariables.estacionamientoHorMenuListAdapter.Count() - 1;
                        guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.estacionamientoHorMenuListAdapter);
                    }

                    textViewSinHorarios.Visibility = ViewStates.Gone;
                    buttonSiguiente.Visibility = ViewStates.Visible; //
                    GlobalVariables.estacionamientosHorHorarios[posicionActual, 0] = hourApertura;
                    GlobalVariables.estacionamientosHorHorarios[posicionActual, 1] = minuteApertura;
                    GlobalVariables.estacionamientosHorHorarios[posicionActual, 2] = hourCierre;
                    GlobalVariables.estacionamientosHorHorarios[posicionActual, 3] = minuteCierre;
                    GlobalVariables.estacionamientoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
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
                GlobalVariables.estacionamientoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                reiniciaDiasSeleccionados();
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

            tab4Button.Click += delegate
            {
                deselectAll();
                tab4Button.SetColorFilter(selectedColor);

                var intent = new Intent(this, typeof(EstacionamientosTextosActivity));
                StartActivity(intent);
            };
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
            string dias = GlobalVariables.estacionamientoHorDiasListAdapter.ElementAt(0).SubHeading; //
            if (!string.IsNullOrWhiteSpace(dias) && hourCierre > hourApertura)
            {
                buttonGuardar.Visibility = ViewStates.Visible;
            }//
            else buttonGuardar.Visibility = ViewStates.Gone; //
        }

        private void OnListItemGuardarClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var position = e.Position;

            linearApertura.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            buttonSiguiente.Visibility = ViewStates.Gone;
            containerEstacionamientoList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            textViewCierre.Visibility = ViewStates.Gone;
            textViewEspacio.Visibility = ViewStates.Gone;
            containerEstacionamientoList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            linearBtnSiguiente.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            containerEstacionamientoGuardarList.Visibility = ViewStates.Gone;
            // TimePicker Dialog
            textTimeApertura.Visibility = ViewStates.Gone;
            textTimeCierre.Visibility = ViewStates.Gone;

            linearBtnRegresar.Visibility = ViewStates.Visible;
            buttonRegresar.Visibility = ViewStates.Visible;
            containerEstacionamientoSubList.Visibility = ViewStates.Visible;

            //Activar los elementos seleccionados
            InicializarDiasSeleccionados();
            changeTextBtnRegresar(); //
        }

        private void InicializarDiasSeleccionados()
        {
            var subListAdapter = new ArrayAdapter<String>(this, GlobalVariables.SimpleListItemChecked, OpcionesMenus.Dias);
            var subList = this.FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorSubList);

            subList.Adapter = subListAdapter;
            subList.ChoiceMode = ChoiceMode.Multiple;
            subList.ItemClick += OnSubListItemClick;

            //Activar los elementos seleccionados
            int initialSelection = 0;
            foreach (bool item in GlobalVariables.estacionamientoHorDiasSemana)
            {
                subList.SetItemChecked(initialSelection, item);
                initialSelection++;
            }
        }

        private void inicializarSpinner() //
        {
            // TimePicker Dialog
            hourApertura = 0;
            minuteApertura = 0;
            hourCierre = 0;
            minuteCierre = 0;
            activePicker = 0;
            UpdateDisplay(activePicker);
        }

        public void OnSubListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            changeTextBtnRegresar(); //
        }

        public void changeTextBtnRegresar() //
        {
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorSubList).CheckedItemPositions;
            string text = "Regresar";
            if (sparseArray != null)
                for (int x = 0; x < sparseArray.Size(); x++)
                {
                    if (sparseArray.ValueAt(x))
                        text = "Guardar";
                }
            buttonRegresar.Text = text;
        }

        private void GuardarCancelarButtons()
        {
            linearApertura.Visibility = ViewStates.Gone;
            linearBtnGuardar.Visibility = ViewStates.Gone;
            buttonGuardar.Visibility = ViewStates.Gone;
            containerEstacionamientoGuardarList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Gone;
            textViewCancelar.Visibility = ViewStates.Gone;
            textViewCierre.Visibility = ViewStates.Gone;
            textViewEspacio.Visibility = ViewStates.Gone;
            containerEstacionamientoList.Visibility = ViewStates.Gone;
            textViewApertura.Visibility = ViewStates.Gone;
            linearHorariosEditar.Visibility = ViewStates.Gone;
            linearBtnRegresar.Visibility = ViewStates.Gone;
            buttonRegresar.Visibility = ViewStates.Gone;
            containerEstacionamientoSubList.Visibility = ViewStates.Gone;
            containerEstacionamientoList.Visibility = ViewStates.Gone;
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
            tab4Button.SetColorFilter(deselectedColor);
        }

        private void showTab4(object sender, EventArgs e)
        {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            var intent = new Intent(this, typeof(EstacionamientosTextosActivity));
            StartActivity(intent);
        }

        private void showViewHorario() //
        {
            if (!GlobalVariables.estacionamientoHorMenuListAdapter.Any())
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

        private void showViewBack(object sender, EventArgs e)
        {
            showViewAgregar();
            if (!GlobalVariables.estacionamientoHorMenuListAdapter.Any()) //
            {
                textViewCancelar.Visibility = ViewStates.Gone; //
            }
        }
        private void showViewAgregar()
        {
            linearApertura.Visibility = ViewStates.Visible;
            linearBtnGuardar.Visibility = ViewStates.Visible;
            buttonGuardar.Visibility = ViewStates.Visible;
            containerEstacionamientoList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Visible;
            textViewCancelar.Visibility = ViewStates.Visible;
            textViewCierre.Visibility = ViewStates.Visible;
            textViewEspacio.Visibility = ViewStates.Visible;
            containerEstacionamientoList.Visibility = ViewStates.Visible;
            textViewApertura.Visibility = ViewStates.Visible;
            // TimePicker Dialog
            textTimeApertura.Visibility = ViewStates.Visible;
            textTimeCierre.Visibility = ViewStates.Visible;

            linearBtnRegresar.Visibility = ViewStates.Gone;
            buttonRegresar.Visibility = ViewStates.Gone;
            containerEstacionamientoSubList.Visibility = ViewStates.Gone;
            containerEstacionamientoGuardarList.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            buttonSiguiente.Visibility = ViewStates.Gone;
            linearHorarios.Visibility = ViewStates.Gone;
            linearBtnSiguiente.Visibility = ViewStates.Gone;

            AgregarSubHeadingDias();

        }

        private void AgregarSubHeadingDias()
        {
            diasLaborales = " ";
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorSubList).CheckedItemPositions;
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
                        GlobalVariables.estacionamientoHorDiasSemana[x] = sparseArray.ValueAt(x);
                    };
                }

                for (int x = 0; x < sparseArray.Size(); x++)
                {
                    if (sparseArray.ValueAt(x))
                        GlobalVariables.estacionamientoHorDias[posicionActual, x] = 1;
                    else
                        GlobalVariables.estacionamientoHorDias[posicionActual, x] = 0;

                    GlobalVariables.estacionamientoHorDiasSemana[x] = sparseArray.ValueAt(x);
                };
            }
            MenusTableItem item = new MenusTableItem();
            item.SubHeading = diasLaborales;
            GlobalVariables.estacionamientoHorDiasListAdapter.ElementAt(0).SubHeading = item.SubHeading;
            list.Adapter = new MenuSimpleAdapter(this, GlobalVariables.estacionamientoHorDiasListAdapter);
            list.ItemClick += OnListItemClick;
            string diasEstacionamiento = GlobalVariables.estacionamientoHorDiasListAdapter.ElementAt(0).SubHeading; //
            if ((diasEstacionamiento == null || diasEstacionamiento.Trim() == "")) //
                buttonGuardar.Visibility = ViewStates.Gone; //
            else buttonGuardar.Visibility = ViewStates.Visible; //
        }

        private void reiniciaDiasSeleccionados()
        {
            var sparseArray = FindViewById<ListView>(Resource.Id.listViewEstacionamientoHorSubList).CheckedItemPositions;
            if (sparseArray != null)
                sparseArray.Clear();
        }

        protected override void OnResume()
        {
            base.OnResume();
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);
            GlobalVariables.estacionamientoHorMenuListAdapter.ForEach(delegate(MenusTableItem item)
            {
                item.ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco;
                item.ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco;
            });
            //GuardarCancelarButtons();
            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.estacionamientoHorMenuListAdapter);
            showViewHorario(); //
        }

        private void fillMenuListAdapter()
        {
            GlobalVariables.estacionamientoHorMenuListAdapter.Clear();
            foreach (MenusEditItem item in GlobalVariables.estacionamientoEditarListAdapter)
            {
                GlobalVariables.estacionamientoHorMenuListAdapter.Add(new MenusTableItem() { Heading = item.Text.Text, SubHeading = item.SubHeading.Text, ImageResourceId = Resource.Drawable.ListEditarEliminarVacioIco, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco });
            }
            guardarList.Adapter = new MenuListAdapter(this, GlobalVariables.estacionamientoHorMenuListAdapter);
        }

        internal void btnRemoveHorarioClick(int position)
        {
            //cambiar
            posicionActual = position;
            GlobalVariables.estacionamientoEditarListAdapter.ElementAt(posicionActual).IsDelete = true;
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true);
            eliminar = true;
            //
        }

        internal void btnConfirmRemoveHorarioClick(int position)
        {
            posicionActual = position;
            GlobalVariables.estacionamientoEditarListAdapter.RemoveAt(posicionActual);
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true);
            int[,] tempDiasArray = new int[GlobalVariables.estacionamientoHorDias.GetLength(0), 7];
            int[,] tempHorarioArray = new int[GlobalVariables.estacionamientosHorHorarios.GetLength(0), GlobalVariables.estacionamientosHorHorarios.GetLength(1)];
            for (int item = 0; item < GlobalVariables.estacionamientoHorDias.GetLength(0); item++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.estacionamientoHorDias.GetLength(1); subIndice++)
                {
                    if (item == posicionActual) { }
                    else if (item < posicionActual)
                    {
                        tempDiasArray[item, subIndice] = GlobalVariables.estacionamientoHorDias[item, subIndice];
                    }
                    else tempDiasArray[item - 1, subIndice] = GlobalVariables.estacionamientoHorDias[item, subIndice];
                }

                for (int subIndice = 0; subIndice < GlobalVariables.estacionamientosHorHorarios.GetLength(1); subIndice++)
                {
                    if (item == posicionActual) { }
                    else if (item < posicionActual)
                    {
                        tempHorarioArray[item, subIndice] = GlobalVariables.estacionamientosHorHorarios[item, subIndice];
                    }
                    else
                    {
                        tempHorarioArray[item - 1, subIndice] = GlobalVariables.estacionamientosHorHorarios[item, subIndice];
                    }
                }
            }
            GlobalVariables.estacionamientoHorDias = tempDiasArray;
            GlobalVariables.estacionamientosHorHorarios = tempHorarioArray;
            linearHorarios.Visibility = ViewStates.Gone;
            linearHorariosEditar.Visibility = ViewStates.Visible;

            // Lineas adicionales
            fillMenuListAdapter();
            guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true);
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
                GlobalVariables.estacionamientoEditarListAdapter.ElementAt(posicionActual).IsDelete = false;
                guardarList.Adapter = new MenuEditarAdapter(this, GlobalVariables.estacionamientoEditarListAdapter, (int)GlobalVariables.Modulos.Estacionamiento, true);
            }
            else //
            {
                guardarDiasHorarios();

                GlobalVariables.estacionamientoHorDiasListAdapter.Clear();
                GlobalVariables.estacionamientoHorDiasListAdapter.Add(new MenusTableItem() { Heading = "Días", ImageResourceMenuId = Resource.Drawable.OpcionesMenuIco });

                for (int cont = 0; cont < GlobalVariables.estacionamientoHorDiasSemana.Length; cont++)
                {
                    if (GlobalVariables.estacionamientoHorDias[posicionActual, cont] == 0)
                        GlobalVariables.estacionamientoHorDiasSemana[cont] = false;
                    else
                        GlobalVariables.estacionamientoHorDiasSemana[cont] = true;
                }
                InicializarDiasSeleccionados();
                // TimePicker Dialog
                hourApertura = (GlobalVariables.estacionamientosHorHorarios[posicionActual, 0]);
                minuteApertura = (GlobalVariables.estacionamientosHorHorarios[posicionActual, 1]);
                hourCierre = (GlobalVariables.estacionamientosHorHorarios[posicionActual, 2]);
                minuteCierre = (GlobalVariables.estacionamientosHorHorarios[posicionActual, 3]);
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
            for (int indice = 0; indice < GlobalVariables.estacionamientoHorDias.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.estacionamientoHorDias.GetLength(1); subIndice++)
                {
                    recuperarDiasSemana[indice, subIndice] = GlobalVariables.estacionamientoHorDias[indice, subIndice];
                }
            }

            for (int indice = 0; indice < GlobalVariables.estacionamientosHorHorarios.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.estacionamientosHorHorarios.GetLength(1); subIndice++)
                {
                    recuperarHorarios[indice, subIndice] = GlobalVariables.estacionamientosHorHorarios[indice, subIndice];
                }
            }
        }



        private void restaurarDiasHorarios()
        {
            for (int indice = 0; indice < GlobalVariables.estacionamientoHorDias.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.estacionamientoHorDias.GetLength(1); subIndice++)
                {
                    GlobalVariables.estacionamientoHorDias[indice, subIndice] = recuperarDiasSemana[indice, subIndice];
                }
            }

            for (int indice = 0; indice < GlobalVariables.estacionamientosHorHorarios.GetLength(0); indice++)
            {
                for (int subIndice = 0; subIndice < GlobalVariables.estacionamientosHorHorarios.GetLength(1); subIndice++)
                {
                    GlobalVariables.estacionamientosHorHorarios[indice, subIndice] = recuperarHorarios[indice, subIndice];
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