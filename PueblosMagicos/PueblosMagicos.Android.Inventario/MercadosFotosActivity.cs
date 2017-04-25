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
using PueblosMagicos;
using Android.Provider;
using Android.Graphics;
using Android.Content.PM;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Java.IO;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "MercadosFotosActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MercadosFotosActivity : Activity
    {
        public const string SAMPLE_CATEGORY = "mono.apidemo.sample";
        private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
        private ImageButton MercadosFotosSigBtn;
        private Color selectedColor, deselectedColor;
        public PageIndicator mIndicator;
        private File _dir;
        private File _file;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.MercadosFotos);

            //MenuLateral
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

            mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            mLeftDrawer.ItemClick += OnMenuLateralItemClick;

            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_mercados_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_mercados_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_mercados_icon);
            tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_mercados_icon);
            MercadosFotosSigBtn = FindViewById<ImageButton>(Resource.Id.MercadosFotosSigBtn);

            selectedColor = Color.ParseColor("#303030"); //The color u want    
            deselectedColor = Color.ParseColor("#ffffff");

            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            FindViewById<Button>(Resource.Id.marketopenCameraButton).Click += OnOpenCamera;
            var viewPager = FindViewById<ViewPager>(Resource.Id.marketpager);
            ImageAdapter adapter = new ImageAdapter(this);
            adapter.TipoAdapter = 1;
            viewPager.Adapter = adapter;

            mIndicator = FindViewById<CirclePageIndicator>(Resource.Id.MercadosIndicator);
            mIndicator.SetViewPager(viewPager);

            if (IsThereAnAppToTakePictures())
            {
               CreateDirectoryForPictures();
            }

            MercadosFotosSigBtn.Click += showTab3;

            tab1Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);

               StartActivity(typeof(MercadosActivity));
            };
         

            tab3Button.Click += delegate
            {
                deselectAll();
                tab3Button.SetColorFilter(selectedColor);

                StartActivity(typeof(MercadosHorariosActivity));

            };

            tab4Button.Click += delegate
            {
                deselectAll();
                tab4Button.SetColorFilter(selectedColor);

                var intent = new Intent(this, typeof(MercadosTextosActivity));
                StartActivity(intent);
            };
        }

        private void CreateDirectoryForPictures()
        {
           _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosMarket");
           if (!_dir.Exists())
           {
              _dir.Mkdirs();
           }
        }

        private bool IsThereAnAppToTakePictures()
        {
           Intent intent = new Intent(MediaStore.ActionImageCapture);
           IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
           return availableActivities != null && availableActivities.Count > 0;
        }

        private void deselectAll()
        {
            tab1Button.SetColorFilter(deselectedColor);
            tab2Button.SetColorFilter(deselectedColor);
            tab3Button.SetColorFilter(deselectedColor);
            tab4Button.SetColorFilter(deselectedColor);
        }

       private void showTab3(object sender, EventArgs e)
        {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            StartActivity(typeof(MercadosHorariosActivity));
        }
        private void showTab1()
        {
            //deselectAll();
            //tab1Button.SetColorFilter(selectedColor);
            //Fragment frag = GeoFragment.NewInstance();
            //showFragment(frag);
        }

        public override void OnBackPressed()
        {
            Finish();
        }
        protected override void OnResume()
        {
            base.OnResume();
            deselectAll();
            tab2Button.SetColorFilter(selectedColor);
        }
        void OnOpenCamera(object sender, EventArgs e)
        {
           try
           {
              Intent intent = new Intent(MediaStore.ActionImageCapture);

              _file = new File(_dir, String.Format("market_{0}.jpg", Guid.NewGuid()));

              intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));

              StartActivityForResult(intent, 0);
           }
           catch (Exception ex)
           {
              string errorMessage = ex.Message;
           }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
           try
           {
              base.OnActivityResult(requestCode, resultCode, data);
           }
           catch (Exception ex)
           {
              string errorMessage = ex.Message;
           }

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