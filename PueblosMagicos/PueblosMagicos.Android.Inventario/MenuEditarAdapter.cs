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
    class MenuEditarAdapter : BaseAdapter<MenusEditItem>
    {
        List<MenusEditItem> items;
        Activity context;
        private MercadosHorariosActivity mercadosHorariosActivity;
        private List<MenusTableItem> list;
        int posicionActual;
        public MenuEditarAdapter(Activity context, List<MenusEditItem> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
             posicionActual = position;
            return position;
        }
        public override MenusEditItem this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            MenusEditItem holder = null;
            posicionActual = position;
            var item = items[position];
            View view = convertView;

            if (holder == null)
            {
                holder = new MenusEditItem();
                view = context.LayoutInflater.Inflate(Resource.Layout.MercadosEditarHorario, null);
                holder.Text = view.FindViewById<TextView>(Resource.Id.TextViewMercadosGuardarHorario);
                holder.SubHeading = view.FindViewById<TextView>(Resource.Id.TextViewSubMercadosGuardarHorario);
                holder.Image = view.FindViewById<ImageView>(Resource.Id.ImageMercadosGuardarHorario);
                holder.Image.Tag = position;
                holder.ImageMenu = view.FindViewById<ImageView>(Resource.Id.ImageMercadosMenuHorario);
                holder.ImageMenu.Tag = position;
                view.Tag = holder;
            }

            holder.Text.Text = item.Text.Text;
            holder.SubHeading.Text = item.SubHeading.Text;
            holder.Image.SetImageResource(Resource.Drawable.MercadosEditarEliminarIco);
            holder.ImageMenu.SetImageResource(Resource.Drawable.OpcionesMenuIco);
            holder.Image.Click += btnRemoveClick;
            holder.ImageMenu.Click += btnEditClick;

            return view;
        }

        private void btnRemoveClick(object sender, EventArgs e)
        {
            var position = (int)((ImageView)sender).Tag;
            ((MercadosHorariosActivity)context).btnRemoveHorarioClick(position);
        }

        private void btnEditClick(object sender, EventArgs e)
        {
            var position = (int)((ImageView)sender).Tag;
            ((MercadosHorariosActivity)context).btnEditHorarioClick(position);
        }

        private void btnRemoveClick()
        {
            
        }

    }
}