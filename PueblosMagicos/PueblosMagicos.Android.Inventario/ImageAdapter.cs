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
      private File _dir;
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

               if (TipoAdapter == 0)
                  _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosSignal");
               else if (TipoAdapter == 1)
                  _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosMarket");
               else if (TipoAdapter == 2)
                  _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosATM");
               else if (TipoAdapter == 3)
                  _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "InvPueblosMagicosOffice");

               images = _dir.ListFiles();
               mCount = _dir.List().Length;
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
