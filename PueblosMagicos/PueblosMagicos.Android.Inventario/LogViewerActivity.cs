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
using PueblosMagicos.Android.Inventario.DataTables;
using PueblosMagicos.Android.Inventario.Services;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "LogViewerActivity")]
    public class LogViewerActivity : Activity
    {
        protected Adapters.TaskListAdapter taskList;
        protected ListView ListaBitacora = null;
        Button botonRegresar;
        DataService data;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LogViewer);
            data = new DataService();

            ListaBitacora = FindViewById<ListView>(Resource.Id.logsList);

            botonRegresar = FindViewById<Button>(Resource.Id.btnRegresarLogs);
            botonRegresar.Click += botonRegresar_Click;

            BindList();
        }

        private void BindList()
        {
            IList<Bitacora> logs = new List<Bitacora>();
            logs = data.AllBitacoras();
            taskList = new Adapters.TaskListAdapter(this, logs);

            RunOnUiThread(() =>
            {
                ListaBitacora.Adapter = taskList;
            });
        }

        void botonRegresar_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MenuHomeActivity));
            StartActivity(intent);
        }
        public override void OnBackPressed()
        {
            Finish();
        }

    }
}