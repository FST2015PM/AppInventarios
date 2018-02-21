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
using Modelos;

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
                conn.CreateTable<Agencias>();
                conn.CreateTable<Estacionamientos>();
                conn.CreateTable<UserSession>();
                conn.CreateTable<Wifi>();
                conn.CreateTable<Fachadas>();
                conn.CreateTable<Bitacora>();
                conn.CreateTable<Cableados>();
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

        public void UpdateRecord(object record)
        {
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                conn.Update(record);
                conn.Close();
            }
        }

        internal IEnumerable<Senalamientos> AllSignals(bool soloPendientes = false)
        {
            IEnumerable<Senalamientos> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Senalamientos>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Senalamientos>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }

        internal IEnumerable<Mercados> AllMarkets(bool soloPendientes = false)
        {
            IEnumerable<Mercados> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Mercados>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Mercados>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }

        internal IEnumerable<Oficinas> AllOffices(bool soloPendientes = false)
        {
            IEnumerable<Oficinas> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Oficinas>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Oficinas>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }
        internal IEnumerable<Cajeros> AllCajeros(bool soloPendientes = false)
        {
            IEnumerable<Cajeros> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Cajeros>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Cajeros>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }
        internal IEnumerable<Agencias> AllAgencias(bool soloPendientes = false)
        {
            IEnumerable<Agencias> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Agencias>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Agencias>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }

        internal IEnumerable<Estacionamientos> AllEstacionamientos(bool soloPendientes = false)
        {
            IEnumerable<Estacionamientos> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Estacionamientos>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Estacionamientos>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }
        internal IEnumerable<Wifi> AllWifi(bool soloPendientes = false)
        {
            IEnumerable<Wifi> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                if (soloPendientes)
                {
                    returnedData = conn.Table<Wifi>().Where(w => w.Enviado == "").ToList();
                }
                else
                {
                    returnedData = conn.Table<Wifi>().ToList();
                }
                conn.Close();
            }
            return returnedData;
        }
        internal IEnumerable<Cableados> AllCableado(bool soloPendientes = false)
        {
           IEnumerable<Cableados> returnedData = null;
           using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
           {
              if (soloPendientes)
              {
                 returnedData = conn.Table<Cableados>().Where(w => w.Enviado == "" || w.Enviado == "0").ToList();
              }
              else
              {
                 returnedData = conn.Table<Cableados>().ToList();
              }
              conn.Close();
           }
           return returnedData;
        }

        internal IEnumerable<Fachadas> AllFachadas()
        {
            IEnumerable<Fachadas> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                returnedData = conn.Table<Fachadas>().ToList();
                conn.Close();
            }
            return returnedData;
        }

        internal void LogOut()
        {
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                conn.DeleteAll<UserSession>();
                GlobalVariables.LoggedSession = null;
                Globals.CurrentUserSession = null;
                conn.Close();
            }
        }

        internal void RegistrarBitacora(string modulo, string textoARegistrar)
        {
            Bitacora log = new Bitacora();
            log.FechaHoraOp = DateTime.Now;
            log.Resultado = textoARegistrar;
            log.TipoOp = modulo;
            log.Id = Guid.NewGuid().ToString();
            SaveData(log);
        }
        public IList<Bitacora> AllBitacoras()
        {
            List<Bitacora> returnedData = null;
            using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
            {
                returnedData = conn.Table<Bitacora>().ToList();
                conn.Close();
            }
            return returnedData;
        }

        public IList<Cableados> AllCableados()
        {
           List<Cableados> returnedData = null;
           using (var conn = new SQLite.SQLiteConnection(DataBaseFullPath))
           {
              returnedData = conn.Table<Cableados>().ToList();
              conn.Close();
           }
           return returnedData;
        }
    }
}