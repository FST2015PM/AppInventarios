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
    public class MenusTableItem
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string SubSubHeading { get; set; }
        public int ImageResourceId { get; set; }
        public int ImageResourceMenuId { get; set; }
        public bool Activado { get; set; }
        public bool isHeader { get; set; }
    }

    public class MenusEditItem : Java.Lang.Object
    {
        public TextView Text { get; set; }
        public TextView SubHeading { get; set; }
        public TextView SubSubHeading { get; set; }
        public ImageView Image { get; set; }
        public ImageView ImageMenu { get; set; }
        public TextView Eliminar { get; set; }
        public bool IsDelete { get; set; }
    }
}

