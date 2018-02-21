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
   public class Cableados
   {
      [PrimaryKey]
      public string Id { get; set; }
      public string pointsArray { get; set; }
      public string Enviado { get; set; }
   }
}