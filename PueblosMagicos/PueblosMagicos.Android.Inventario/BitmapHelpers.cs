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

using System.IO;

using Android.Graphics;
using Android.Graphics.Drawables;

namespace PueblosMagicos.Android.Inventario
{
   public static class BitmapHelpers
   {
      /// <summary>
      /// This method will recyle the memory help by a bitmap in an ImageView
      /// </summary>
      /// <param name="imageView">Image view.</param>
      public static void RecycleBitmap(this ImageView imageView)
      {
         if (imageView == null)
         {
            return;
         }

         Drawable toRecycle = imageView.Drawable;
         if (toRecycle != null)
         {
            ((BitmapDrawable)toRecycle).Bitmap.Recycle();
         }
      }


      /// <summary>
      /// Load the image from the device, and resize it to the specified dimensions.
      /// </summary>
      /// <returns>The and resize bitmap.</returns>
      /// <param name="fileName">File name.</param>
      /// <param name="width">Width.</param>
      /// <param name="height">Height.</param>
      public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
      {
         // First we get the the dimensions of the file on disk
         BitmapFactory.Options options = new BitmapFactory.Options
         {
            InPurgeable = true,
            InJustDecodeBounds = false,
            InSampleSize = 2,
            InPreferredConfig = Bitmap.Config.Rgb565
         };
         return BitmapFactory.DecodeFile(fileName, options);
      }
   }
}