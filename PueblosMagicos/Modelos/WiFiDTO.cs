using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class WiFiDTO
   {
      public LocationDTO loc { get; set; }
      public string provider { get; set; }
      public int upSpeed { get; set; }
      public int downSpeed { get; set; }
      public string accessType { get; set; }
      public bool inService { get; set; }
   }
}
