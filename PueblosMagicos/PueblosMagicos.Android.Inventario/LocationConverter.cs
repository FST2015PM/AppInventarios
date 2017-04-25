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
using Android.Locations;

namespace PueblosMagicos.Android.Inventario
{
   public class LocationConverter
   {
      public static String getLatitudeAsDMS(Double latitud, int decimalPlace)
      {
         String strLatitude = Location.Convert(latitud, Format.Seconds);
         strLatitude = replaceDelimiters(strLatitude, decimalPlace);
         strLatitude = strLatitude + " N";
         return strLatitude;
      }

      public static String getLongitudeAsDMS(Double longitud, int decimalPlace)
      {
         String strLongitude = Location.Convert(longitud, Format.Seconds);
         strLongitude = replaceDelimiters(strLongitude, decimalPlace);
         strLongitude = strLongitude + " W";
         return strLongitude;
      }

      private static String replaceDelimiters(String str, int decimalPlace)
      {
         int counter = 0;
         char[] locationString = str.ToCharArray();
         for (int i = 0; i< locationString.Length; i++) {
            if (locationString[i] == ':' && counter == 0)
            {
               locationString[i] = '°';
               counter++;
               continue;
            }
            if (locationString[i] == ':' && counter == 1)
            {
               locationString[i] = '\'';
               counter++;
               continue;
            }
         }
         str = String.Join("",locationString);

         int pointIndex = str.IndexOf(".");
         int endIndex = pointIndex + 1 + decimalPlace;
         if (endIndex < str.Length)
         {
            str = str.Substring(0, endIndex);
         }
         str = str + "\"";
         return str;
      }
   }
}