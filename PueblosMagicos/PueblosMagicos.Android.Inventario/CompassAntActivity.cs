
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace PueblosMagicos.Android.Inventario
{
    public class CompassAntActivity : Activity
    {
        //public const string SAMPLE_CATEGORY = "mono.apidemo.sample";
        private ImageButton tab1Button, tab2Button, tab3Button, tab4Button;
        private Color selectedColor, deselectedColor;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Senalamientos);

            tab1Button = this.FindViewById<ImageButton>(Resource.Id.tab1_icon);
            tab2Button = this.FindViewById<ImageButton>(Resource.Id.tab2_icon);
            tab3Button = this.FindViewById<ImageButton>(Resource.Id.tab3_icon);
            tab4Button = this.FindViewById<ImageButton>(Resource.Id.tab4_icon);

            selectedColor = Color.ParseColor("#303030"); //The color u want    
            deselectedColor = Color.ParseColor("#ffffff");

            deselectAll();
            tab3Button.SetColorFilter(selectedColor);

            var fragment = new CompassFragment(Resource.Layout.orientacion_fragment);
            FragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.containerTab, fragment)
                .Commit();
            showTab1();


            tab1Button.Click += delegate
            {
                deselectAll();
                tab1Button.SetColorFilter(selectedColor);
                //var fragment = new GeolocalizacionFragment();
                //var ft = FragmentManager.BeginTransaction();
                //ft.Add(Resource.Id.containerTab, fragment);
                //ft.Commit();
            };

            tab2Button.Click += delegate
            {
                deselectAll();
                tab2Button.SetColorFilter(selectedColor);

                var intent = new Intent(this, typeof(FotosActivity));
                StartActivity(intent);
            };

            tab4Button.Click += delegate
            {
                deselectAll();
                tab4Button.SetColorFilter(selectedColor);

                var intent = new Intent(this, typeof(NotasActivity));
                StartActivity(intent);
            };
        }

        private void deselectAll()
        {
            tab1Button.SetColorFilter(deselectedColor);
            tab2Button.SetColorFilter(deselectedColor);
            tab3Button.SetColorFilter(deselectedColor);
            tab4Button.SetColorFilter(deselectedColor);
        }

        private void showFragment(Fragment fragment)
        {
            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.containerTab, fragment);
            ft.Commit();
        }

        private void showTab1()
        {
            //deselectAll();
            //tab1Button.SetColorFilter(selectedColor);
            //Fragment frag = GeoFragment.NewInstance();
            //showFragment(frag);
        }
    }
}