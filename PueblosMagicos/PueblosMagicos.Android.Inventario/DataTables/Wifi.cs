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
   public class Wifi
   {
      [PrimaryKey]
      public string Id { get; set; }
      public double Latitud { get; set; }
      public double Longitud { get; set; }
      public string Provider { get; set; }
      public string UpSpeed { get; set; }
      public string DownSpeed { get; set; }
      public string AccessType { get; set; }
      public bool InService { get; set; }
      public string Enviado { get; set; }
   }
}