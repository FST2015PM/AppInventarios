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
   public class UserSession
   {
      public string fullname { get; set; }
      public string email { get; set; }
      public string sessionId { get; set; }
      public string value { get; set; }
      public string CVE_MUN { get; set; }
      public string CVE_ENT { get; set; }
      public string CVE_LOC { get; set; }
      public string _id { get; set; }
   }
}