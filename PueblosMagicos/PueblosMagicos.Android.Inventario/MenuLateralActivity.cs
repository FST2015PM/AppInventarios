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
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "MenuLateralActivity", Theme = "@style/MyTheme.MenuLateral")]
    public class MenuLateralActivity : Activity
    {
        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ArrayAdapter mLeftAdapter;
        ListView mLeftDrawer;
        ActionBarDrawerToggle mDrawerToggle;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MenuLateral);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.iconBar,Resource.String.open_drawer,Resource.String.close_drawer);

            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Señalamientos turísticos", ImageResourceId = Resource.Drawable.icSenalamientos, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cableado subterráneo", ImageResourceId = Resource.Drawable.icCableado, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Estacionamientos", ImageResourceId = Resource.Drawable.icEstacionamientos, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Mercados", ImageResourceId = Resource.Drawable.icMercados, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cajeros automáticos", ImageResourceId = Resource.Drawable.icCajeros, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "WiFi", ImageResourceId = Resource.Drawable.icWiFi, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Oficinas de congresos", ImageResourceId = Resource.Drawable.icCongresos, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Agencias y tour operadores", ImageResourceId = Resource.Drawable.icAgencias, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Fachadas restauradas", ImageResourceId = Resource.Drawable.icFachadas, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });
            //GlobalVariables.menuLateralListAdapter.Add(new MenusTableItem() { Heading = "Cuestionario", ImageResourceId = Resource.Drawable.icCuestionario, ImageResourceMenuId = Resource.Drawable.MercadosEditarEliminarVacioIco });

            mLeftDrawer.Adapter = new MenuListAdapter(this, GlobalVariables.menuLateralListAdapter);
            mLeftDrawer.ItemClick += OnListItemClick;
            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            //ActionBar.SetHomeButtonEnabled(true);

        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var posicion = e.Position;
            switch (posicion)
            {
                case 0:
                    StartActivity(typeof(SenalamientosMapActivity));
                    break;
                case 3:
                     var intent = new Intent(this, typeof(MercadosActivity));
                     StartActivity(intent);
                    break;
                case 4:
                    StartActivity(typeof(CajerosActivity));
                   
                    break;
            }

        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (mDrawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}