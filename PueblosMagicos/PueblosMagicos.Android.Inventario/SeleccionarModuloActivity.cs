using Android.App;
using Android.OS;
using Android.Views;
using System;
using System.Linq;
using System.Text;

using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android.Content;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using System.Collections.Generic;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "Menu", ParentActivity = typeof(MainActivity))]
    public class SeleccionarModuloActivity : Activity
    {
        private ImageView senalButton, mercadosButton, cajerosButton, oficinasButton;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         RequestWindowFeature(WindowFeatures.NoTitle);
         SetContentView(Resource.Layout.SeleccionarModulo);

         senalButton = this.FindViewById<ImageView>(Resource.Id.btnSenalamiento);
         mercadosButton = this.FindViewById<ImageView>(Resource.Id.btnMercado);
         cajerosButton = this.FindViewById<ImageView>(Resource.Id.btnCajero);
         oficinasButton = this.FindViewById<ImageView>(Resource.Id.btnOficina);
          //MenuLateral
         mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
         mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
         if (!GlobalVariables.menuLateralListAdapter.Any())
         {
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
         }
         
         mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
         mLeftDrawer.ItemClick += OnMenuLateralItemClick;

         senalButton.Click += delegate
         {
             StartActivity(typeof(SenalamientosMapActivity));
         };

         mercadosButton.Click += delegate
         {
             var intent = new Intent(this, typeof(MercadosActivity));
             StartActivity(intent);
         };

         cajerosButton.Click += delegate
         {
             StartActivity(typeof(CajerosTextosActivity));
         };

         oficinasButton.Click += delegate
         {
             StartActivity(typeof(OficinasTextosActivity));
         };
      }

      private void OnMenuLateralItemClick(object sender, AdapterView.ItemClickEventArgs e)
      {
          var listView = sender as ListView;
          var posicion = e.Position;
          switch (posicion)
          {
              case 0:
                  StartActivity(typeof(SenalamientosMapActivity));
                  break;
              case 3:
                  StartActivity(typeof(MercadosActivity));
                  break;
              case 4:
                  StartActivity(typeof(CajerosActivity));
                  break;
              case 6:
                  StartActivity(typeof(OficinasActivity));
                  break;
          }
          mDrawerLayout.CloseDrawer(mLeftDrawer);
      }

    }
    
}