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
using PueblosMagicos.Android.Inventario.DataTables;

namespace PueblosMagicos.Android.Inventario.Services
{
   public class DataService
   {
      const string DATABASENAME = "InvPueblosMagicos.sqlite";
      readonly string DataBaseFullPath = Path.Combine(
            Application.Context.FilesDir.AbsolutePath, DATABASENAME);
      public void InitializeDB()
      {
         bool alreadyExistsDB = File.Exists(DataBaseFullPath);

         using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
         {
            conn.CreateTable<Senalamientos>();
            conn.CreateTable<Mercados>();
            conn.CreateTable<Oficinas>();
            conn.CreateTable<Cajeros>();
            conn.CreateTable<UserSession>();


         }
      }

      public bool EsSesionExistente()
      {
         bool valorRetorno = false;
         using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
         {
            GlobalVariables.LoggedSession = conn.Table<UserSession>().FirstOrDefault();
            if (GlobalVariables.LoggedSession != null)
               valorRetorno = true;
            conn.Close();
         }
         return valorRetorno;
      }

      public void SaveData(object data)
      {
         using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
         {
            conn.Insert(data);
            conn.Close();
         }
      }
      public void DeleteAllData<T>()
      {
         using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
         {
            conn.DeleteAll<T>();
            conn.Close();
         }
      }

      internal IEnumerable<Senalamientos> AllSignals()
      {
         IEnumerable<Senalamientos> returnedData = null;
         using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
         {
            returnedData = conn.Table<Senalamientos>().ToList();
            conn.Close();
         }
         return returnedData;
      }

      internal IEnumerable<Mercados> AllMarkets()
      {
         IEnumerable<Mercados> returnedData = null;
         using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
         {
            returnedData = conn.Table<Mercados>().ToList();
            conn.Close();
         }
         return returnedData;
      }

      internal IEnumerable<Oficinas> AllOffices()
      {
          IEnumerable<Oficinas> returnedData = null;
          using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
          {
              returnedData = conn.Table<Oficinas>().ToList();
              conn.Close();
          }
          return returnedData;
      }
        internal IEnumerable<Cajeros> AllCajeros()
        {
            IEnumerable<Cajeros> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                returnedData = conn.Table<Cajeros>().ToList();
                conn.Close();
            }
            return returnedData;
        }
    }
}