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
                view = context.LayoutInflater.Inflate(Resource.Layout.MenuLateral, null);
            view.FindViewById<TextView>(Resource.Id.TextViewMenuLateral).Text = item.Heading;
            view.FindViewById<ImageView>(Resource.Id.ImageMenuLateral).SetImageResource(item.ImageResourceId);

            return view;
        }
    }
}