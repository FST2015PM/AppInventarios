using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class SignalDTO
   {
      public LocationDTO loc { get; set; }
      public string type { get; set; }
      public string position { get; set; }
      public string visible { get; set; }
      public ImageDTO image { get; set; }
   }
}
