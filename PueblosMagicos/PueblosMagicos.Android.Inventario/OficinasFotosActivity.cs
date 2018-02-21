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
    [Activity(Label = "OficinasFotosActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class OficinasFotosActivity : Activity
    {
        public const string SAMPLE_CATEGORY = "mono.apidemo.sample";
        private ImageButton tab1Button, tab2Button, tab3Button, btnInicio;
        private Button OficinaFotosSigBtn, oficinaChangeCameraButton, oficinaOpenCameraButton; //;
        private TextView textViewSinFotos, textChangeCameraButton, textOpenCameraButton; //
        private Color selectedColor, deselectedColor;
        public PageIndicator mIndicator;
        private ViewPager viewPager; //
        private int NoFotos; //
        private File _dir;
        private File _file;

        DrawerLayout mDrawerLayout;
        List<String> mLeftItems = new List<string>();
        ListView mLeftDrawer;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.OficinasFotos);

            //MenuLateral
            //mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            
            //mLeftDrawer.Adapter = new MenuLateralAdapter(this, GlobalVariables.menuLateralListAdapter);
            //mLeftDrawer.ItemClick += OnMenuLateralItemClick;

            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_oficinas_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_oficinas_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_oficinas_icon);
            OficinaFotosSigBtn = FindViewById<Button>(Resource.Id.oficinaFotosSigBtn);
            btnInicio = this.FindViewById<ImageButton>(Resource.Id.btnInicio); //
            textViewSinFotos = this.FindViewById<TextView>(Resource.Id.textViewSinFotos); //
            textChangeCameraButton = this.FindViewById<TextView>(Resource.Id.textChangeCameraButton); //
            textOpenCameraButton = this.FindViewById<TextView>(Resource.Id.textOpenCameraButton); //
            oficinaChangeCameraButton = FindViewById<Button>(Resource.Id.oficinaChangeCameraButton); //
            oficinaOpenCameraButton = FindViewById<Button>(Resource.Id.oficinaOpenCameraButton); //

            selectedColor = Color.ParseColor("#ffffff"); //The color u want    
            deselectedColor = Color.ParseColor("#ca9def");

            deselectAll();
            tab2Button.SetColorFilter(selectedColor);

            oficinaOpenCameraButton.Click += OnOpenCamera; //
            oficinaChangeCameraButton.Click += OnOpenCamera; //

            viewPager = FindViewById<ViewPager>(Resource.Id.signalpager);

            actualizaFotos();
            
            OficinaFotosSigBtn.Click += showTab3;
            tab3Button.Click += showTab3;

            tab1Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);

                StartActivity(typeof(OficinasActivity));
            };

            btnInicio.Click += delegate //
            {
                var intent = new Intent(this, typeof(MenuHomeActivity));
                StartActivity(intent);
            };
        }

        private void showTab3(object sender, EventArgs e)
        {
            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            StartActivity(typeof(OficinasTextosActivity));
        }

        private void actualizaFotos() //
        {
            ImageAdapter adapter = new ImageAdapter(this);
            adapter.TipoAdapter = (int)GlobalVariables.Modulos.Oficina;
            viewPager.Adapter = adapter;

            NoFotos = adapter.Count;

            mIndicator = FindViewById<CirclePageIndicator>(Resource.Id.signalindicator);
            mIndicator.SetViewPager(viewPager);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
            }

            showMessages();
        }
        private void showMessages() //
        {
            var state1 = ViewStates.Visible;
            var state2 = ViewStates.Gone;

            if (NoFotos == 0)
            {
                oficinaOpenCameraButton.Visibility = state1;
                textViewSinFotos.Visibility = state1;
                textOpenCameraButton.Visibility = state1;

                oficinaChangeCameraButton.Visibility = state2;
                OficinaFotosSigBtn.Visibility = state2;
                textChangeCameraButton.Visibility = state2;

            }
            else
            {
                oficinaOpenCameraButton.Visibility = state2;
                textViewSinFotos.Visibility = state2;
                textOpenCameraButton.Visibility = state2;

                oficinaChangeCameraButton.Visibility = state1;
                OficinaFotosSigBtn.Visibility = state1;
                textChangeCameraButton.Visibility = state1;
            }
        }
        private void CreateDirectoryForPictures()
        {
            _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosOffice");
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
            actualizaFotos(); //
        }

        void OnOpenCamera(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                if (string.IsNullOrWhiteSpace(GlobalVariables.oficinaFotoActual))
                    GlobalVariables.oficinaFotoActual = String.Format("office_{0}.jpg", Guid.NewGuid());

                _file = new File(_dir, GlobalVariables.oficinaFotoActual);
                intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));
                GlobalVariables.OfficesPhotoName = Uri.FromFile(_file).Path;

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
                case 1:
                    StartActivity(typeof(MercadosActivity));
                    break;
                case 2:
                    StartActivity(typeof(CajerosActivity));
                    break;
                case 3:
                    StartActivity(typeof(OficinasActivity));
                    break;
                case 4:
                    StartActivity(typeof(AgenciasActivity));
                    break;
                case 5:
                    StartActivity(typeof(EstacionamientosActivity));
                    break;
                case 6:
                    StartActivity(typeof(FachadasActivity));
                    break;
                case 7:
                     StartActivity(typeof(WifisActivity));
                    break;
                case 8:
                    //StartActivity(typeof(CableadosActivity));
                    
                    break;
            }
            //mDrawerLayout.CloseDrawer(mLeftDrawer);
        }
    }
}