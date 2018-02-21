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
using System.Net.Http;
using Modelos;
using Newtonsoft.Json;
using System.IO;
namespace PueblosMagicos.Android.Inventario.Services
{
   internal class SecurityService
   {
      HttpClient client;

      public SecurityService()
      {
         client = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());
         client.BaseAddress = new System.Uri(Modelos.Globals.BASESERVICEURL);
         client.MaxResponseContentBufferSize = 256000;
      }
      public async Task<bool> ValidLogin(string user, string password)
      {
         try
         {
            var uri = new System.Uri(Modelos.Globals.BASESERVICEURL);
            client.DefaultRequestHeaders.Authorization = System.Net.Http.Headers.AuthenticationHeaderValue.Parse("Basic " + Modelos.Globals.APIKEY);
            LoginData dto = new LoginData();
            dto.email = user;
            dto.password = password;
            String postContent = JsonConvert.SerializeObject(dto);
            var content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Modelos.Globals.LOGINSERVICE, content);
            string responseValue = await response.Content.ReadAsStringAsync();

            if (responseValue.ToLower().Contains("unauthorized"))
               return false;

            Globals.CurrentUserSession = JsonConvert.DeserializeObject<LoginResult>(responseValue);
            GlobalVariables.LoggedSession = new DataTables.UserSession();

            if (Globals.CurrentUserSession != null && Globals.CurrentUserSession.user != null)
            {
               GlobalVariables.LoggedSession.email = Globals.CurrentUserSession.user.email;
               GlobalVariables.LoggedSession.fullname = Globals.CurrentUserSession.user.fullname;
               if (Globals.CurrentUserSession.user.magictown != null)
               {
                  GlobalVariables.LoggedSession._id = Globals.CurrentUserSession.user.magictown._id;
                  GlobalVariables.LoggedSession.CVE_ENT = Globals.CurrentUserSession.user.magictown.CVE_ENT;
                  GlobalVariables.LoggedSession.CVE_LOC = Globals.CurrentUserSession.user.magictown.CVE_LOC;
                  GlobalVariables.LoggedSession.CVE_MUN = Globals.CurrentUserSession.user.magictown.CVE_MUN;
               }
            }
            if (Globals.CurrentUserSession != null && Globals.CurrentUserSession.session != null)
            {
               GlobalVariables.LoggedSession.sessionId = Globals.CurrentUserSession.session.sessionId;
               GlobalVariables.LoggedSession.value = Globals.CurrentUserSession.session.value;
            }
            return true;
         }
         catch (Exception ex)
         {
            string exceptionmessage = ex.Message;
            return false;
         }
      }

      public async Task<bool> SendData(Activity target)
      {
         var uri = new System.Uri(Modelos.Globals.BASESERVICEURL);
         client.DefaultRequestHeaders.Authorization = System.Net.Http.Headers.AuthenticationHeaderValue.Parse("Basic " + Modelos.Globals.APIKEY + ":" + GlobalVariables.LoggedSession.value);
         DataService dataService = new DataService();
         String postContent;
         StringContent content;
         string responseValue;
         bool completoExito = true;

         try
         {
            var cajeros = dataService.AllCajeros(true);
            List<AtmDTO> cajeroData = new List<AtmDTO>();
            foreach (var cajero in cajeros)
            {
               try
               {

                  cajeroData.Add(new AtmDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = cajero.Latitud, Long = cajero.Longitud } },
                     atmUnits = System.Convert.ToInt32(cajero.atmUnits),
                     bank = cajero.Bank,
                     inService = cajero.inService
                  });

                  postContent = JsonConvert.SerializeObject(cajeroData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.ATMSERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("ATM", "Cajero " + cajero.Bank + " enviado con respuesta " + responseValue);
                  //Marcar registros como enviados.
                  cajero.Enviado = "1";
                  dataService.UpdateRecord(cajero);
                  cajeroData.Clear();
               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("ATM", "Error en Cajero " + cajero.Bank + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("ATM", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var signals = dataService.AllSignals(true);
            List<SignalDTO> signalData = new List<SignalDTO>();
            foreach (var signal in signals)
            {
               try
               {
                  signalData.Add(new SignalDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = signal.Latitud, Long = signal.Longitud } },
                     position = signal.Position,
                     type = signal.Type,
                     visible = signal.Visible,
                     image = new ImageDTO()
                     {
                        fileType = "jpg",
                        fileName = signal.FotoName,
                        content = GetBase64(signal.FotoName)
                     }
                  });
                  postContent = JsonConvert.SerializeObject(signalData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.SIGNALSERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("SIGNAL", "Señal pos:" + signal.Position + " typ:" + signal.Type + " vis:" + signal.Visible + " enviado con respuesta " + responseValue);

                  signal.Enviado = "1";
                  dataService.UpdateRecord(signal);
                  signalData.Clear();

               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("SIGNAL", "Error en Señal pos" + signal.Position + " typ:" + signal.Type + " vis:" + signal.Visible + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("SIGNAL", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var parkings = dataService.AllEstacionamientos(true);
            List<ParkingDTO> parkingData = new List<ParkingDTO>();
            foreach (var parking in parkings)
            {
               try
               {
                  parkingData.Add(new ParkingDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = parking.Latitud, Long = parking.Longitud } },
                     name = parking.Name,
                     carCapacity = System.Convert.ToInt32(parking.CarCapacity),
                     fee = System.Convert.ToInt32(parking.Fee),
                     freeTime = parking.FreeTime,
                     serviceDays = parking.ServiceDays.Split(','),
                     serviceHours = parking.ServiceHours.Split(','),
                     is24H = parking.Is24h,
                     isSelfService = parking.IsSelfServices,
                     isFormal = parking.IsFormal,
                     contact = parking.Contact,
                     amenities = parking.Amenities,
                     image = new ImageDTO()
                     {
                        fileType = "jpg",
                        fileName = parking.FotoName,
                        content = GetBase64(parking.FotoName)
                     }
                  });

                  postContent = JsonConvert.SerializeObject(parkingData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.PARKINGSERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("PARKING", "Parking " + parking.Name + " enviado con respuesta " + responseValue);

                  parking.Enviado = "1";
                  dataService.UpdateRecord(parking);
                  parkingData.Clear();
               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("PARKING", "Error en PARKING " + parking.Name + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }

            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("PARKING", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var markets = dataService.AllMarkets(true);
            List<MarketDTO> marketsData = new List<MarketDTO>();
            foreach (var market in markets)
            {
               try
               {
                  marketsData.Add(new MarketDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = market.Latitud, Long = market.Longitud } },
                     name = market.Description,
                     type = market.Type,
                     descrption = market.Description,
                     created = market.Created,
                     shopNumber = System.Convert.ToInt32(market.ShopNum),
                     serviceDays = market.ServiceDays.Split(','),
                     serviceHours = market.ServiceHours.Split(','),
                     image = new ImageDTO()
                     {
                        fileType = "jpg",
                        fileName = market.FotoName,
                        content = GetBase64(market.FotoName)
                     }
                  });

                  postContent = JsonConvert.SerializeObject(marketsData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.MARKETSERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("MARKET", "MARKET " + market.Description + " enviado con respuesta " + responseValue);
                  //Marcar registros como enviados.
                  market.Enviado = "1";
                  dataService.UpdateRecord(market);
                  marketsData.Clear();
               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("MARKET", "Error en Market " + market.Description + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("MARKET", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var agencies = dataService.AllAgencias(true);
            List<AgencyDTO> agenciesData = new List<AgencyDTO>();
            foreach (var agency in agencies)
            {
               try
               {

                  agenciesData.Add(new AgencyDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = agency.Latitud, Long = agency.Longitud } },
                     name = agency.Name,
                     type = agency.Type,
                     address = agency.Address,
                     contact = agency.Contact,
                     products = agency.Products,
                     serviceDays = agency.ServiceDays.Split(','),
                     serviceHours = agency.ServiceHours.Split(','),
                  });

                  postContent = JsonConvert.SerializeObject(agenciesData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.AGENCYSERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("AGENCY", "Agendy " + agency.Name + " enviado con respuesta " + responseValue);
                  agency.Enviado = "1";
                  dataService.UpdateRecord(agency);
                  agenciesData.Clear();
               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("AGENCY", "Error en Agency " + agency.Name + " " + exSnd.Message + " " + exSnd.StackTrace);

                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("MARKET", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var offices = dataService.AllOffices(true);
            List<OfficeDTO> officesData = new List<OfficeDTO>();
            foreach (var office in offices)
            {
               try
               {
                  officesData.Add(new OfficeDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = office.Latitud, Long = office.Longitud } },
                     name = office.Name,
                     capacity = office.Aforo,
                     contact = office.Contact,
                     manager = office.Contact,
                     image = new ImageDTO()
                     {
                        fileType = "jpg",
                        fileName = office.FotoName,
                        content = GetBase64(office.FotoName)
                     }
                  });

                  postContent = JsonConvert.SerializeObject(officesData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.OFFICESERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("OFFICE", "Office " + office.Name + " enviado con respuesta " + responseValue);
                  office.Enviado = "1";
                  dataService.UpdateRecord(office);
                  officesData.Clear();
               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("OFFICE", "Error en Office " + office.Name + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("OFFICE", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var wifis = dataService.AllWifi(true);
            List<WiFiDTO> wifisData = new List<WiFiDTO>();
            foreach (var wifi in wifis)
            {
               try
               {

                  wifisData.Add(new WiFiDTO()
                  {
                     loc = new LocationDTO() { type = "Point", coordinates = new CoordinatesDTO() { Lat = wifi.Latitud, Long = wifi.Longitud } },
                     provider = wifi.Provider,
                     upSpeed = System.Convert.ToInt32(wifi.UpSpeed),
                     downSpeed = System.Convert.ToInt32(wifi.DownSpeed),
                     accessType = wifi.AccessType,
                     inService = wifi.InService
                  });

                  postContent = JsonConvert.SerializeObject(wifisData);
                  content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                  var responseSignal = await client.PostAsync(Modelos.Globals.WIFISERVICE, content);
                  responseValue = await responseSignal.Content.ReadAsStringAsync();
                  dataService.RegistrarBitacora("WIFI", "WiFi " + wifi.Provider + " enviado con respuesta " + responseValue);
                  wifi.Enviado = "1";
                  dataService.UpdateRecord(wifi);

               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("WIFI", "WiFi " + wifi.Provider + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }

            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("WIFI", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         try
         {
            var cables = dataService.AllCableado(true);
            List<CableadoDTO> cablesData = new List<CableadoDTO>();
            foreach (var cable in cables)
            {
               try
               {
                  if (cable.pointsArray.Contains(';') && cable.pointsArray.Contains(','))
                  {

                     List<CoordinatesDTO> localCoordinates = cable.pointsArray.Split(';').ToList().Select(s => new CoordinatesDTO()
                     {
                        Lat = System.Convert.ToDouble(s.Split(',')[0].Replace(",", "").Replace(";", ""))
                        ,
                        Long = System.Convert.ToDouble(s.Split(',')[0].Replace(",", "").Replace(";", ""))
                     }).ToList();

                     cablesData.Add(new CableadoDTO()
                     {
                        loc = new CableLocationDTO() { type = "LineString", coordinates = localCoordinates },
                        type = "Subterráneo"
                     });

                     postContent = JsonConvert.SerializeObject(cablesData);
                     content = new StringContent(postContent, System.Text.Encoding.UTF8, "application/json");
                     var responseSignal = await client.PostAsync(Modelos.Globals.CABLEADOSERVICE, content);
                     responseValue = await responseSignal.Content.ReadAsStringAsync();
                     dataService.RegistrarBitacora("Cableado", "Cableado " + cable.pointsArray + " enviado con respuesta " + responseValue);
                     cable.Enviado = "1";
                     dataService.UpdateRecord(cable);

                  }

               }
               catch (Exception exSnd)
               {
                  dataService.RegistrarBitacora("Cableado", "Cableado " + cable.pointsArray + " " + exSnd.Message + " " + exSnd.StackTrace);
                  completoExito = false;
                  if (exSnd.Message.ToLower().Contains("unauthorized"))
                  {
                     dataService.LogOut();
                     QuitApplication(target);
                  }
               }

            }
         }
         catch (Exception ex)
         {
            dataService.RegistrarBitacora("Cableado", "No se enviaron los registros debido a " + " " + ex.Message + " " + ex.StackTrace);
            completoExito = false;
            if (ex.Message.ToLower().Contains("unauthorized"))
            {
               dataService.LogOut();
               QuitApplication(target);
            }
         }

         return completoExito;
      }

      private void QuitApplication(Activity target)
      {
         AlertDialog.Builder builder = new AlertDialog.Builder(target);
         AlertDialog alertDialog = builder.Create();

         alertDialog.SetTitle("Pueblos Magicos");
         alertDialog.SetMessage("Se requiere iniciar sesion nuevamente.");
         alertDialog.SetButton("Ok", (s, ev) =>
         {
            Intent salida = new Intent(Intent.ActionMain);
            target.FinishAffinity();
            target.Finish();
            Process.KillProcess(Process.MyPid());
            System.Environment.Exit(0);
            target.MoveTaskToBack(true);
         });

         alertDialog.Show();
      }

      private string GetBase64(string filename)
      {
         // provide read access to the file
         FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
         // Create a byte array of file stream length
         byte[] ImageData = new byte[fs.Length];
         //Read block of bytes from stream into the byte array
         fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
         //Close the File Stream
         fs.Close();
         string _base64String = Convert.ToBase64String(ImageData);
         return _base64String;
      }
   }
}