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
   }
}