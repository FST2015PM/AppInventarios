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
using SQLite;

namespace PueblosMagicos.Android.Inventario.DataTables
{
   class Oficinas
   {
      [PrimaryKey]
      public string Id { get; set; }
      public double Latitud { get; set; }
      public double Longitud { get; set; }
      public string Name { get; set; }
      public string Contact { get; set; }
      public string Aforo { get; set; }
      public string FotoName { get; set; }
      public string Enviado { get; set; }
   }
}