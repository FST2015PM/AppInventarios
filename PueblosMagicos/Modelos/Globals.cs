using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public  class Globals
   {
      public const string APIKEY = "Tn6nDO2XK5EuBgDEuJec2/m9ouYZUQYBeN+Znr12zI8=";
      public const string APISECRET = "qrrP9hxQhcmF9sgQojlByrXdNFpfr0tsLhqErqPdG5M=";
      public static readonly string AuthorizationString = "Basic " + APIKEY;
      public const string BASESERVICEURL = "https://miit.mx/api/v1/services/";
      public const string LOGINSERVICE = "login";
      public static LoginResult CurrentUserSession { get; set; }
   }
}
