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
using System.Threading.Tasks;

namespace PueblosMagicos.Android.Inventario
{
    [Activity(Label = "RecuperarNipActivity", Theme = "@android:style/Theme.NoTitleBar")]
    public class RecuperarNipActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.RecuperarNip);

            FindViewById<Button>(Resource.Id.recuperarNipButton).Click += OnRecuperarNipClick;
        }

        async void OnRecuperarNipClick(object sender, EventArgs e)
        {
            // TODO

            string message = string.Empty;

            message = "NIP enviado al correo registrado";

            Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
            await Task.Delay(2000);
            Finish();
        }
    }
}