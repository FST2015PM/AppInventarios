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
using PueblosMagicos.Android.Inventario.DataTables;

namespace PueblosMagicos.Android.Inventario
{
    public static class GlobalVariables
    {
        public static UserSession LoggedSession { get; set; }
        public static Senalamientos SenalamientoRecord { get; set; }
        public static Mercados MercadoRecord { get; set; }
        public static double Latitud { get; set; }
        public static double Longitud { get; set; }
        public static double LatitudSenalamiento { get; set; }
        public static double LongitudSenalamiento { get; set; }
        public static double LatitudMercado { get; set; }
        public static double LongitudMercado { get; set; }
        public static double LatitudCajero { get; set; }
        public static double LongitudCajero { get; set; }
        public static double LatitudOficina { get; set; }
        public static double LongitudOficina { get; set; }
        public static double LatitudEstacionamiento { get; set; }
        public static double LongitudEstacionamiento { get; set; }
        public static double LatitudFachada { get; set; }
        public static double LongitudFachada { get; set; }
        public static double LatitudWifi { get; set; }
        public static double LongitudWifi { get; set; }

        public static double LatitudAgencias { get; set; }
        public static double LongitudAgencias
        {
            get;
            set;
        }

        public static double LatitudCableado { get; set; }
        public static double LongitudCableado
        {
           get;
           set;
        }

        public static string LatitudCoordinates { get; set; }

        public static string LongitudCoordinates { get; set; }

        public const int SimpleListItemChecked = 17367045;
        public const int SimpleExpandableListItem1 = 17367046;

        public enum Modulos { Senalamiento, Mercado, Cajero, Oficina, Agencia, Estacionamiento, Fachada, Wifi, Cableado };

        #region SenalamientosActivity
        public static bool senalNuevaCaptura = true;
        //Fotos
        public static string senalFotoActual = "";
        public static List<MenusTableItem> senalTxtMenuListAdapter = new List<MenusTableItem>();
        public static string senalTxtComentario = "";
        public static bool[] senalTxtTipo = { false, false };
        public static bool[] senalTxtPosicion = { false, false };
        public static bool[] senalTxtVisibilidad = { false, false, false };
        #endregion



        #region AgenciasHorariosActivity
        public static bool agenciaNuevaCaptura = true;

        public static List<MenusTableItem> agenciasHorMenuListAdapter = new List<MenusTableItem>();
        public static List<MenusTableItem> agenciasHorDiasListAdapter = new List<MenusTableItem>();
        public static List<MenusEditItem> agenciasEditarListAdapter = new List<MenusEditItem>();

        public static List<MenusTableItem> agenciasTxtMenuListAdapter = new List<MenusTableItem>();
        public static string AgenciaType = "", AgenciaName = "", AgenciaAddress = "", AgenciaContact = "", AgenciaProducts = "", AgenciaNombre = "";
        public static bool[] AgenciasTxtTipo = { false, false };
        public static bool[] AgenciasTxtPosicion = { false, false };
        public static bool[] AgenciasTxtVisibilidad = { false, false, false };

        public static bool[] agenciasHorDiasSemana = { false, false, false, false, false, false, false };
        public static int[,] agenciasHorDias = new int[20, 7];
        public static int[,] agenciasHorHorarios = new int[20, 4];
        public static bool[] agenciasTipoDeServicio = { false, false };
        #endregion

        #region Mercados
        public static bool mercadoNuevaCaptura = true;
        //Fotos
        public static string mercadoFotoActual = "";

        //Horarios
        public static List<MenusTableItem> mercadoHorMenuListAdapter = new List<MenusTableItem>();
        public static List<MenusTableItem> mercadoHorDiasListAdapter = new List<MenusTableItem>();
        public static List<MenusEditItem> mercadoEditarListAdapter = new List<MenusEditItem>();
        public static bool[] mercadoHorDiasSemana = { false, false, false, false, false, false, false };
        public static int[,] mercadoHorDias = new int[20, 7];
        public static int[,] mercadosHorHorarios = new int[20, 4];

        //Textos
        public static List<MenusTableItem> mercadoTxtMenuListAdapter = new List<MenusTableItem>();
        public static bool[] mercadoTxtTipo = { false, false, false };
        public static bool[] mercadoTxtCondicion = { false, false };
        public static string mercadoTxtNombre = "", mercadoTxtDesc = "", mercadoTxtNoCom = "";
        #endregion

        #region CajerosTextosActivity
        public static bool cajeroNuevaCaptura = true;
        public static string cajerosTxtBanco = "", cajerosTxtNoCajeros = "";
        public static bool cajerosEnServicio;
        #endregion

        #region OficinasTextos
        public static bool oficinaNuevaCaptura = true;
        //Fotos
        public static string oficinaFotoActual = "";
        public static string oficinaTxtNombre, oficinaTxtNoSalas, oficinaTxtAforo, oficinaTxtContacto;
        #endregion

        #region Estacionamientos
        public static bool estacionamientoNuevaCaptura = true;
        public static string estacionamientoFotoActual = "";

        public static List<MenusTableItem> estacionamientoHorMenuListAdapter = new List<MenusTableItem>();
        public static List<MenusTableItem> estacionamientoHorDiasListAdapter = new List<MenusTableItem>();
        public static List<MenusEditItem> estacionamientoEditarListAdapter = new List<MenusEditItem>();
        public static bool[] estacionamientoHorDiasSemana = { false, false, false, false, false, false, false };
        public static int[,] estacionamientoHorDias = new int[20, 7];
        public static int[,] estacionamientosHorHorarios = new int[20, 4];
        public static bool[] estacionamientoCondicion = { false, false };
        public static List<MenusTableItem> estacionamientoTxtMenuListAdapter = new List<MenusTableItem>();
        public static bool EstacionamientoIsFormal = false, EstacionamientoIsFreeTime = false, EstacionamientoIs24h = false, EstacionamientoIsSelfService = false;
        public static string EstacionamientoName = "", EstacionamientoCarCapacity = "", EstacionamientoFee = "", EstacionamientoContact = "", EstacionamientoAmenities = "";
        #endregion

        #region Fachadas
        public static bool fachadaNuevaCaptura = true;
        public static string fachadaFotoActual = "";

        public static List<MenusTableItem> fachadaHorMenuListAdapter = new List<MenusTableItem>();
        public static List<MenusEditItem> fachadaEditarListAdapter = new List<MenusEditItem>();
        public static string[,] fachadasHorHorarios = new string[20, 3];
        public static List<MenusTableItem> fachadaTxtMenuListAdapter = new List<MenusTableItem>();
        public static bool FachadaIsCommerce = false, FachadaIsHomologated = false;
        public static string FachadaNumber = "";
        #endregion

        #region Wifi
        public static bool wifiNuevaCaptura = true;

        public static List<MenusTableItem> WifiTxtMenuListAdapter = new List<MenusTableItem>();
        public static bool WifiFunciona = false, WifiIsFormal = false;
        public static bool[] WifiCondicion = { false, false };
        public static string WifiProveedor = "";

        //WifiTextos
        public static string WifiSubida, WifiBajada;
        #endregion
        #region Cableado
        public static bool cableadoNuevaCaptura = true;
        #endregion
        #region MenuLateralActivity
        public static List<MenusTableItem> menuLateralListAdapter = new List<MenusTableItem>();
        #endregion

        public static string SignalPhotoName { get; set; }

        public static string OfficesPhotoName { get; set; }

        public static string MarketPhotoName { get; set; }

        public static string ParkingPhotoName { get; set; }

        public static string FacadePhotoName { get; set; }
    }
}