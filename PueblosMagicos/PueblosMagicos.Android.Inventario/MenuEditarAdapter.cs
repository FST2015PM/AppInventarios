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
        int posicionActual;
        private int TipoAdapter { get; set; }
        private bool Editar;
        public MenuEditarAdapter(Activity context, List<MenusEditItem> items, int tipoadapter, bool editar)
            : base()
        {
            this.context = context;
            this.items = items;
            this.TipoAdapter = tipoadapter;
            this.Editar = editar;
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
                if (TipoAdapter == (int)GlobalVariables.Modulos.Fachada)
                {
                    if (item.IsDelete)
                    {
                        view = context.LayoutInflater.Inflate(Resource.Layout.MenuEliminarHorario, null);
                        holder.Eliminar = view.FindViewById<TextView>(Resource.Id.TextViewEliminar);
                        holder.Eliminar.Tag = position;
                        holder.Eliminar.Click += btnConfirmRemoveClick;
                    }
                    else
                    {
                        view = context.LayoutInflater.Inflate(Resource.Layout.FachadaEditarHorario, null);
                        if (string.IsNullOrWhiteSpace(item.SubSubHeading.Text))
                            view.FindViewById<TextView>(Resource.Id.TextViewSubSubGuardarHorario).Visibility = ViewStates.Gone;
                        else
                        {
                        holder.SubSubHeading = view.FindViewById<TextView>(Resource.Id.TextViewSubSubGuardarHorario);
                        holder.SubSubHeading.Text = item.SubSubHeading.Text;
                        }
                    }
                }
                else
                    if (item.IsDelete)
                    {
                        view = context.LayoutInflater.Inflate(Resource.Layout.MenuEliminarHorario, null);
                        holder.Eliminar = view.FindViewById<TextView>(Resource.Id.TextViewEliminar);
                        holder.Eliminar.Tag = position;
                        holder.Eliminar.Click += btnConfirmRemoveClick;
                    }
                    else
                        view = context.LayoutInflater.Inflate(Resource.Layout.MercadosEditarHorario, null);
                

                holder.Text = view.FindViewById<TextView>(Resource.Id.TextViewGuardarHorario);
                holder.SubHeading = view.FindViewById<TextView>(Resource.Id.TextViewSubGuardarHorario);
                holder.Image = view.FindViewById<ImageView>(Resource.Id.ImageGuardarHorario);
                holder.Image.Tag = position;
                holder.ImageMenu = view.FindViewById<ImageView>(Resource.Id.ImageMenuHorario);
                holder.ImageMenu.Tag = position;

                view.Tag = holder;
            }

            holder.Text.Text = item.Text.Text;
            holder.SubHeading.Text = item.SubHeading.Text;
            if (Editar)
            {
                holder.Image.SetImageResource(Resource.Drawable.ListEditarEliminarIco);
                holder.ImageMenu.SetImageResource(Resource.Drawable.OpcionesMenuIco);
            }
            else
            {
                holder.Image.SetImageResource(Resource.Drawable.ListEditarEliminarVacioIco);
                holder.ImageMenu.SetImageResource(Resource.Drawable.ListEditarEliminarVacioIco);
            }
            holder.Image.Click += btnRemoveClick;
            holder.ImageMenu.Click += btnEditClick;

            return view;
        }

        private void btnConfirmRemoveClick(object sender, EventArgs e)
        {
            var position = (int)((TextView)sender).Tag;
            if (TipoAdapter == (int)GlobalVariables.Modulos.Mercado)
                ((MercadosHorariosActivity)context).btnConfirmRemoveHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Agencia)
                ((AgenciasHorariosActivity)context).btnConfirmRemoveHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Estacionamiento)
                ((EstacionamientosHorariosActivity)context).btnConfirmRemoveHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Fachada)
                ((FachadasTextosActivity)context).btnConfirmRemoveHorarioClick(position);
        }

        private void btnRemoveClick(object sender, EventArgs e)
        {
            var position = (int)((ImageView)sender).Tag;
            if (TipoAdapter == (int)GlobalVariables.Modulos.Mercado)
                ((MercadosHorariosActivity)context).btnRemoveHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Agencia)
                ((AgenciasHorariosActivity)context).btnRemoveHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Estacionamiento)
                ((EstacionamientosHorariosActivity)context).btnRemoveHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Fachada)
                ((FachadasTextosActivity)context).btnRemoveHorarioClick(position);
        }

        private void btnEditClick(object sender, EventArgs e)
        {
            var position = (int)((ImageView)sender).Tag;
            if (TipoAdapter == 1)
                ((MercadosHorariosActivity)context).btnEditHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Agencia)
                ((AgenciasHorariosActivity)context).btnEditHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Estacionamiento)
                ((EstacionamientosHorariosActivity)context).btnEditHorarioClick(position);
            else if (TipoAdapter == (int)GlobalVariables.Modulos.Fachada)
                ((FachadasTextosActivity)context).btnEditHorarioClick(position);
        }
    }
}