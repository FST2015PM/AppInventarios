using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class OfficeDTO
   {
      public LocationDTO loc { get; set; }
      public string name { get; set; }
      public string contact { get; set; }
      public string capacity { get; set; }
      public string manager { get; set; }
      public ImageDTO image { get; set; }
   }
}
