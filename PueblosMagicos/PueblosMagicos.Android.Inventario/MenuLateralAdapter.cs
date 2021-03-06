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
using Android.Graphics;

namespace PueblosMagicos.Android.Inventario
{
   class MenuLateralAdapter : BaseAdapter<MenusTableItem>
   {
      List<MenusTableItem> items;
      Activity context;

      public MenuLateralAdapter(Activity context, List<MenusTableItem> items)
         : base()
      {
         this.context = context;
         this.items = items;
      }
      public override long GetItemId(int position)
      {
         return position;
      }
      public override MenusTableItem this[int position]
      {
         get { return items[position]; }
      }
      public override int Count
      {
         get { return items.Count; }
      }
      public override View GetView(int position, View convertView, ViewGroup parent)
      {
         var item = items[position];

         View view = convertView;
         if (view == null) // no view to re-use, create new
            if (item.isHeader)
            {
               view = context.LayoutInflater.Inflate(Resource.Layout.MenuLateralHeader, null);
               var txtUsuario = view.FindViewById<TextView>(Resource.Id.txtNameHeaderMenuLateral);
               txtUsuario.Text = "Bienvenido " + GlobalVariables.LoggedSession.fullname;
            }
            else
            {
               view = context.LayoutInflater.Inflate(Resource.Layout.MenuLateral, null);
               var texto = view.FindViewById<TextView>(Resource.Id.TextViewMenuLateral);
               texto.Text = item.Heading;
               var imagen = view.FindViewById<ImageView>(Resource.Id.ImageMenuLateral);
               imagen.SetImageResource(item.ImageResourceId);
            };
         return view;
      }
   }
}