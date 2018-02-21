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
   public class Estacionamientos
   {
      [PrimaryKey]
      public string Id { get; set; }
      public double Latitud { get; set; }
      public double Longitud { get; set; }
      public string Name { get; set; }
      public string CarCapacity { get; set; }
      public string Fee { get; set; }
      public string Contact { get; set; }
      public string Amenities { get; set; }
      public string ServiceDays { get; set; }
      public string ServiceHours { get; set; }
      public bool IsFormal { get; set; }
      public bool FreeTime { get; set; }
      public bool Is24h { get; set; }
      public bool IsSelfServices { get; set; }
      public string FotoName { get; set; }
      public string Enviado { get; set; }
   }
}