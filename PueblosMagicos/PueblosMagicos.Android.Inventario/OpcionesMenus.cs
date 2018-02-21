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

namespace PueblosMagicos.Android.Inventario
{
   internal class OpcionesMenus
   {
       public static string[] Dias = { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
       public static string[] CondicionesServicio = { "Formal", "Informal" };


       #region Señalamientos
           public static string[] TipoSenalamiento = { "Turístico", "Tránsito" };
           public static string[] PosicionSenalamiento = { "A nivel del suelo", "Elevado" };
           public static string[] VisibilidadSenalamiento = { "Totalmente visible", "Parcialmente visible", "No visible" };
       #endregion

       #region Mercados
            public static string[] TipoMercado = { "Artesanal", "Gastronómico", "Tradicional" };
        #endregion

        #region Agencias
            public static string[] TipoAgencia = { "Agencia", "Touroperador" };
        #endregion

        #region Fachadas
            public static string[] TipoFachada = { "Casa", "Comercio" };
            public static string[] TipoFachadaComercio = { "Anuncio homologado" };
        #endregion
    }
}