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
using Android.Media;
using Android.Graphics;
using PueblosMagicos.Android.Inventario.DataTables;

namespace PueblosMagicos.Android.Inventario.Adapters
{
    public class TaskListAdapter : BaseAdapter<Bitacora>
    {
        protected Activity context = null;
        protected IList<Bitacora> _bitacora = new List<Bitacora>();


        public TaskListAdapter(Activity context, IList<Bitacora> mascotas)
            : base()
        {
            this.context = context;
            this._bitacora = mascotas;
        }

        public override Bitacora this[int position]
        {
            get { return _bitacora[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _bitacora.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _bitacora[position];
            View view;

            if (convertView == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.LogItem, null);
            }
            else
            {
                view = convertView;
            }

            var tipoOp = view.FindViewById<TextView>(Resource.Id.txtTipoOp);
            tipoOp.Text = item.TipoOp;

            var fechaOp = view.FindViewById<TextView>(Resource.Id.txtFechaOp);
            fechaOp.Text = item.FechaHoraOp.ToString("dd/MM/yyyy");

            var resultado = view.FindViewById<TextView>(Resource.Id.txtResultado);
            resultado.Text =item.Resultado;

            return view;
        }
    }
}