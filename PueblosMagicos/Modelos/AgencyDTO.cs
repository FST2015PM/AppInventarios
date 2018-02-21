using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class AgencyDTO
   {
      public LocationDTO loc { get; set; }
      public string type { get; set; }
      public string name { get; set; }
      public string address { get; set; }
      public string contact { get; set; }
      public string products { get; set; }
      public string[] serviceDays { get; set; }
      public string[] serviceHours { get; set; }
   }
}
