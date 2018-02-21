using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public  class Globals
   {
      public const string APIKEY = "e96WNBU93Ee15dhc3KU9pgFETfydGnOBetMUqF1sOtU=";
      public const string APISECRET = "PJ2+Vlt87QQfjZFBZa4BV+7ZsyIpNAhPmT8XuFpD3Ig=";
      public static readonly string AuthorizationString = "Basic " + APIKEY;
      public const string BASESERVICEURL = "https://miit.mx/api/v1/services/";
      public const string LOGINSERVICE = "login";
      public const string SIGNALSERVICE = "signal";
      public const string ATMSERVICE = "atm";
      public const string PARKINGSERVICE = "parking";
      public const string MARKETSERVICE = "market";
      public const string AGENCYSERVICE = "travel";
      public const string OFFICESERVICE = "conference";
      public const string WIFISERVICE = "wifi";
      public const string CABLEADOSERVICE = "wiring";
      public static LoginResult CurrentUserSession { get; set; }
   }
}
