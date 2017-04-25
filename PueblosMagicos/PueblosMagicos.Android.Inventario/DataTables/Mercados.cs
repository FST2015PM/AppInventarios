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

namespace PueblosMagicos.Android.Inventario.DataTables
{
   public class Mercados
   {
      public string Id { get; set; }
      public double Latitud { get; set; }
      public double Longitud { get; set; }
      public string Type { get; set; }
      public string Description { get; set; }
      public string Created { get; set; }
      public string ShopNum { get; set; }
      public string ServiceDays{get;set;}
      public string ServiceHours { get; set; }
   }
}