﻿using System;
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
   public class Senalamientos
   {
      public string Id { get; set; }
      public double Latitud { get; set; }
      public double Longitud { get; set; }
      public string Type { get; set; }
      public string Position { get; set; }
      public string Visible { get; set; }

   }
}