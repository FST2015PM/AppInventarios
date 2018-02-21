using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class MarketDTO
   {
      public LocationDTO loc { get; set; }
      public string name { get; set; }
      public string type { get; set; }
      public string descrption { get; set; }
      public string created { get; set; }
      public int shopNumber { get; set; }
      public string[] serviceDays { get; set; }
      public string[] serviceHours { get; set; }
      public ImageDTO image { get; set; }
   }
}
