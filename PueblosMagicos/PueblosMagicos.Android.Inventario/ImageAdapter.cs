using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Android.Content;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Graphics;
using Android.App;
using System;

namespace PueblosMagicos.Android.Inventario
{
   public class ImageAdapter : PagerAdapter
   {
       private File _dir, _file;
      private Context context;
      int mCount;
      private File[] images;
      public int TipoAdapter { get; set; }
      public ImageAdapter(Context context)
      {
         this.context = context;
      }

      public override int Count
      {
         get
         {
            try
            {
               int oldCound = mCount;
               string fotoActual = "";

               if (TipoAdapter == (int)GlobalVariables.Modulos.Senalamiento)
               {
                   _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosSignal");
                   fotoActual = GlobalVariables.senalFotoActual;
               }
               else if (TipoAdapter == (int)GlobalVariables.Modulos.Mercado)
               {
                   _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosMarket");
                   fotoActual = GlobalVariables.mercadoFotoActual;
               }
               else if (TipoAdapter == (int)GlobalVariables.Modulos.Oficina){
                   _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosOffice");
                   fotoActual = GlobalVariables.oficinaFotoActual;
               }
               else if (TipoAdapter == (int)GlobalVariables.Modulos.Estacionamiento){
                   _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosParking");
                   fotoActual = GlobalVariables.estacionamientoFotoActual;
               }
               else if (TipoAdapter == (int)GlobalVariables.Modulos.Fachada)
               {
                   _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosFacade");
                   fotoActual = GlobalVariables.fachadaFotoActual;
               }
                
                _file = new File(_dir, fotoActual);
               
                images = _dir.ListFiles();  //Lista de imagenes
                images[0] = _file;  // Una imágen

                if (!string.IsNullOrWhiteSpace(fotoActual))
                    mCount = 1; // _dir.List().Length;
                else
                    mCount = 0;

               if (oldCound != mCount)
                  this.NotifyDataSetChanged();
               return mCount;
            }
            catch
            {
               mCount = 0;
               return 0;
            }
         }
      }

      public void SetCount(int countParameter)
      {
         if (countParameter > 0 && countParameter <= Count)
         {
            NotifyDataSetChanged();
         }
      }

      public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
      {
         return view == ((ImageView)objectValue);
      }

      public override Java.Lang.Object InstantiateItem(View container, int position)
      {
         ImageView imageView = new ImageView(context);
         try
         {
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            var metrics = Application.Context.Resources.DisplayMetrics;
            int width = metrics.WidthPixels;
            int height = metrics.HeightPixels;

            if (Count > 0)
            {
               imageView.RecycleBitmap();
               using (Bitmap bitmap = images[position].Path.LoadAndResizeBitmap(width, height))
               {
                  imageView.SetImageBitmap(bitmap);
               }
            }
            else
            {
               imageView.SetImageResource(Resource.Drawable.fondo);

            }
            ((ViewPager)container).AddView(imageView, 0);
         }
         catch (Exception ex)
         {
            string errorMessage = ex.Message;
         }
         return imageView;
      }

      public override void DestroyItem(View container, int position, Java.Lang.Object objectValue)
      {
         ((ViewPager)container).RemoveView((ImageView)objectValue);
      }
   }
}
