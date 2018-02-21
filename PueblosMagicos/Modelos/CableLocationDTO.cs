using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class CableLocationDTO
   {
      public string type { get; set; }
      public List<CoordinatesDTO> coordinates { get; set; }
   }
}
