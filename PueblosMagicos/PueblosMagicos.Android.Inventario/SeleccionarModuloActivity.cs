using Android.App;
using Android.OS;
using Android.Views;
using System;
using System.Linq;
using System.Text;

using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android.Content;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.IO;
using Environment = Android.OS.Environment;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "Menu", ParentActivity = typeof(MainActivity))]
    public class SeleccionarModuloActivity : Activity
    {
        private ImageView senalButton, mercadosButton, cajerosButton, oficinasButton, estacionamientosButton, agenciasButton,
            fachadasButton, wifisButton, cableadoButton;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;

        private File _dir;
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.SeleccionarModulo);

         senalButton = this.FindViewById<ImageView>(Resource.Id.btnSenalamiento);
         mercadosButton = this.FindViewById<ImageView>(Resource.Id.btnMercado);
         cajerosButton = this.FindViewById<ImageView>(Resource.Id.btnCajero);
         oficinasButton = this.FindViewById<ImageView>(Resource.Id.btnOficina);
         agenciasButton = this.FindViewById<ImageView>(Resource.Id.btnAgencia);
         estacionamientosButton = this.FindViewById<ImageView>(Resource.Id.btnEstacionamientos);
         wifisButton = this.FindViewById<ImageView>(Resource.Id.btnWifi);
         cableadoButton = this.FindViewById<ImageView>(Resource.Id.btnCableado);

          //MenuLateral
         //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
         //if (!GlobalVariables.menuLateralListAdapter.Any())
         //{
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Señalamientos turísticos", ImageResourceId = Resource.Drawable.icSenalamientos, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Mercados", ImageResourceId = Resource.Drawable.icMercados, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cajeros automáticos", ImageResourceId = Resource.Drawable.icCajeros, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Oficinas de congresos", ImageResourceId = Resource.Drawable.icCongresos, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Agencias y tour operadores", ImageResourceId = Resource.Drawable.icAgencias, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Estacionamientos", ImageResourceId = Resource.Drawable.icEstacionamientos, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Fachadas restauradas", ImageResourceId = Resource.Drawable.icFachadas, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "WiFi", ImageResourceId = Resource.Drawable.icWiFi, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cableado subterráneo", ImageResourceId = Resource.Drawable.icCableado, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //    GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Sincronizar con la plataforma", ImageResourceId = Resource.Drawable.icCuestionario, ImageResourceMenuId = Resource.Drawable.ListEditarEliminarVacioIco, Activado = false });
         //}
         
         //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         senalButton.Click += delegate
         {
             StartActivity(typeof(SenalamientosMapActivity));

             if (GlobalVariables.senalNuevaCaptura)
             {

                 if (!string.IsNullOrWhiteSpace(GlobalVariables.senalFotoActual))
                 {
                     CreateDirectoryForPictures();
                 }

                 GlobalVariables.senalFotoActual = "";
                 GlobalVariables.senalTxtMenuListAdapter.Clear();
                 GlobalVariables.senalTxtComentario = "";
                 GlobalVariables.senalTxtTipo = Enumerable.Repeat(false, 2).ToArray();
                 GlobalVariables.senalTxtPosicion = Enumerable.Repeat(false, 2).ToArray();
                 GlobalVariables.senalTxtVisibilidad = Enumerable.Repeat(false, 3).ToArray();

                 GlobalVariables.senalNuevaCaptura = false;
             }
         };

         mercadosButton.Click += delegate
         {
             if (GlobalVariables.mercadoNuevaCaptura){

                 if (!string.IsNullOrWhiteSpace(GlobalVariables.mercadoFotoActual)) {
                     CreateDirectoryForPictures();
                  }

                  GlobalVariables.mercadoFotoActual = "";

                GlobalVariables.mercadoHorMenuListAdapter.Clear();
                GlobalVariables.mercadoHorDiasListAdapter.Clear();
                GlobalVariables.mercadoEditarListAdapter.Clear();
                GlobalVariables.mercadoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                GlobalVariables.mercadoHorDias = new int[20, 7];
                GlobalVariables.mercadosHorHorarios = new int[20, 4];
            
                //Textos
               GlobalVariables.mercadoTxtMenuListAdapter.Clear();
               GlobalVariables.mercadoTxtTipo = Enumerable.Repeat(false, 3).ToArray();
               GlobalVariables.mercadoTxtCondicion = Enumerable.Repeat(false, 2).ToArray();
               GlobalVariables.mercadoTxtNombre = ""; 
               GlobalVariables.mercadoTxtDesc = "";
               GlobalVariables.mercadoTxtNoCom = "";

               GlobalVariables.mercadoNuevaCaptura = false;
             }
             
             var intent = new Intent(this, typeof(MercadosActivity));
             StartActivity(intent);
         };

         cajerosButton.Click += delegate
         {
             if (GlobalVariables.cajeroNuevaCaptura)
             {
                 //Textos
                 GlobalVariables.cajerosTxtBanco = "";
                 GlobalVariables.cajerosTxtNoCajeros = "";
                 GlobalVariables.cajerosEnServicio = false;

                 GlobalVariables.cajeroNuevaCaptura = false;
             }
             StartActivity(typeof(CajerosActivity));
         };

         oficinasButton.Click += delegate
         {
             if (GlobalVariables.oficinaNuevaCaptura)
             {

                 if (!string.IsNullOrWhiteSpace(GlobalVariables.oficinaFotoActual))
                 {
                     CreateDirectoryForPictures();
                 }

                 GlobalVariables.oficinaFotoActual = "";

                 //Textos
                 GlobalVariables.oficinaTxtNombre = "";
                 GlobalVariables.oficinaTxtNoSalas = "";
                 GlobalVariables.oficinaTxtAforo = "";
                 GlobalVariables.oficinaTxtContacto = "";

                 GlobalVariables.oficinaNuevaCaptura = false;
             }
             StartActivity(typeof(OficinasActivity));
         };

         agenciasButton.Click += delegate
         {
             if (GlobalVariables.agenciaNuevaCaptura)
             {
                GlobalVariables.agenciasHorMenuListAdapter.Clear();
                GlobalVariables.agenciasHorDiasListAdapter.Clear();
                GlobalVariables.agenciasEditarListAdapter.Clear();
                GlobalVariables.agenciasTxtMenuListAdapter.Clear();
                GlobalVariables.AgenciaType = "";
                GlobalVariables.AgenciaName = "";
                GlobalVariables.AgenciaAddress="";
                GlobalVariables.AgenciaContact="";
                GlobalVariables.AgenciaProducts="";
                GlobalVariables.AgenciaNombre="";
                GlobalVariables.AgenciasTxtTipo = Enumerable.Repeat(false, 2).ToArray();
                GlobalVariables.AgenciasTxtPosicion  = Enumerable.Repeat(false, 2).ToArray();
                GlobalVariables.AgenciasTxtVisibilidad  = Enumerable.Repeat(false, 3).ToArray();
        
                GlobalVariables.agenciasHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                GlobalVariables.agenciasHorDias = new int[20, 7];
                GlobalVariables.agenciasHorHorarios = new int[20, 4];
                GlobalVariables.agenciasTipoDeServicio = Enumerable.Repeat(false, 2).ToArray();
                 
                 GlobalVariables.agenciaNuevaCaptura = false;
             }
             StartActivity(typeof(AgenciasActivity));
         };
      
         estacionamientosButton.Click += delegate
         {
             if (GlobalVariables.oficinaNuevaCaptura)
             {
                 if (!string.IsNullOrWhiteSpace(GlobalVariables.estacionamientoFotoActual))
                 {
                     CreateDirectoryForPictures();
                 }

                GlobalVariables.estacionamientoFotoActual = "";
                GlobalVariables.estacionamientoHorMenuListAdapter.Clear();
                GlobalVariables.estacionamientoHorDiasListAdapter.Clear();
                GlobalVariables.estacionamientoEditarListAdapter.Clear();
                GlobalVariables.estacionamientoHorDiasSemana = Enumerable.Repeat(false, 7).ToArray();
                GlobalVariables.estacionamientoHorDias = new int [20, 7];
                GlobalVariables.estacionamientosHorHorarios= new int [20, 4];
                GlobalVariables.estacionamientoCondicion = Enumerable.Repeat(false, 2).ToArray();
                GlobalVariables.estacionamientoTxtMenuListAdapter.Clear();
                GlobalVariables.EstacionamientoIsFormal = false;
                GlobalVariables.EstacionamientoIsFreeTime = false;
                GlobalVariables.EstacionamientoIs24h = false;
                GlobalVariables.EstacionamientoIsSelfService = false;
                GlobalVariables.EstacionamientoName = "";
                GlobalVariables.EstacionamientoCarCapacity = "";
                GlobalVariables.EstacionamientoFee = "";
                GlobalVariables.EstacionamientoContact = "";
                GlobalVariables.EstacionamientoAmenities = "";

                 GlobalVariables.estacionamientoNuevaCaptura = false;
             }
             StartActivity(typeof(EstacionamientosActivity));
         };

         wifisButton.Click += delegate
         {
             if (GlobalVariables.wifiNuevaCaptura)
             {
                 GlobalVariables.WifiTxtMenuListAdapter.Clear();
                 GlobalVariables.WifiFunciona = false;
                 GlobalVariables.WifiIsFormal = false;
                 GlobalVariables.WifiCondicion = Enumerable.Repeat(false, 2).ToArray();
                 GlobalVariables.WifiProveedor = "";
      
            //WifiTextos
            GlobalVariables.WifiSubida = "";
            GlobalVariables.WifiBajada = "";

                 GlobalVariables.wifiNuevaCaptura = false;
             }
             StartActivity(typeof(WifisActivity));
         };

         cableadoButton.Click += delegate
         {
             if (GlobalVariables.cableadoNuevaCaptura)
             {
                 GlobalVariables.cableadoNuevaCaptura = false;
             }
             StartActivity(typeof(CableadoActivity));
         };

      }

      private void CreateDirectoryForPictures()
      {
          _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosSignal");
          if (!_dir.Exists())
          {
              _dir.Mkdirs();
          }

          _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosMarket");
          if (!_dir.Exists())
          {
              _dir.Mkdirs();
          }

          _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosOffice");
          if (!_dir.Exists())
          {
              _dir.Mkdirs();
          }

          _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosParking");
          if (!_dir.Exists())
          {
              _dir.Mkdirs();
          }

          _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosFacade");
          if (!_dir.Exists())
          {
              _dir.Mkdirs();
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
                    //StartActivity(typeof(FachadasActivity));
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