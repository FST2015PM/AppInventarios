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
       #region Se�alamientos
           public static string[] TipoSenalamiento = { "Tur�stico", "Tr�nsito" };
           public static string[] PosicionSenalamiento = { "A nivel del suelo", "Elevado" };
           public static string[] VisibilidadSenalamiento = { "Totalmente visible", "Parcialmente visible", "No visible" };
       #endregion

       #region Mercados
            public static string[] DiasMercados = { "Domingo", "Lunes", "Martes", "Mi�rcoles", "Jueves", "Viernes", "S�bado" };

            public static string[] TipoMercado = { "Artesanal", "Gastron�mico", "Tradicional" };
            public static string[] CondicionesServicioMercado = { "Formal", "Informal" };
        #endregion
    }
}