using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class AtmDTO
   {
      public LocationDTO loc { get; set; }
      public string bank { get; set; }
      public Int32 atmUnits { get; set; }
      public bool inService { get; set; }
   }
}
