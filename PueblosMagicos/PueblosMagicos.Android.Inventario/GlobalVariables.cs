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

      public static string LatitudCoordinates { get; set; }

      public static string LongitudCoordinates { get; set; }

      public const int SimpleListItemChecked = 17367045;
      public const int SimpleExpandableListItem1 = 17367046;

       #region SenalamientosActivity
          public static List<MenusTableItem> senalTxtMenuListAdapter = new List<MenusTableItem>();
          public static string senalTxtComentario="";
          public static bool[] senalTxtTipo = { false, false };
          public static bool[] senalTxtPosicion = { false, false };
          public static bool[] senalTxtVisibilidad = { false, false, false };
       #endregion

          #region MercadosHorariosActivity
              public static List<MenusTableItem> mercadoHorMenuListAdapter = new List<MenusTableItem>();
              public static List<MenusTableItem> mercadoHorDiasListAdapter = new List<MenusTableItem>();
              public static List<MenusEditItem> editarListAdapter = new List<MenusEditItem>();
              public static bool[] mercadoHorDiasSemana = { false, false, false, false, false, false, false };
              public static int[,] mercadoHorDias = new int [20, 7];
              public static int[,] mercadosHorHorarios= new int [20, 4];
              
          #endregion

       #region MercadosTextosActivity
          public static List<MenusTableItem> mercadoTxtMenuListAdapter = new List<MenusTableItem>();
          public static bool[] mercadoTxtTipo = { false, false, false };
          public static bool[] mercadoTxtCondicion = { false, false };
          public static string mercadoTxtNombre = "", mercadoTxtDesc = "", mercadoTxtNoCom = "";
       #endregion

        #region CajerosTextosActivity
          public static string cajerosTxtBanco = "", cajerosTxtNoCajeros = "";
        public static bool cajerosEnServicio;
        #endregion

          #region OficinasTextos

          public static string oficinaTxtNombre, oficinaTxtNoSalas, oficinaTxtAforo, oficinaTxtContacto;
          #endregion

          #region MenuLateralActivity
          public static List<MenusTableItem> menuLateralListAdapter = new List<MenusTableItem>();
       #endregion
   }
}