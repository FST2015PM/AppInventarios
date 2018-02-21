using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelos
{
   public class ParkingDTO
   {
      public LocationDTO loc { get; set; }
      public string name { get; set; }
      public int carCapacity { get; set; }
      public decimal fee { get; set; }
      public bool freeTime { get; set; }
      public string[] serviceDays { get; set; }
      public string[] serviceHours { get; set; }
      public bool is24H { get; set; }
      public bool isSelfService { get; set; }
      public bool isFormal { get; set; }
      public string contact { get; set; }
      public string amenities { get; set; }
      public ImageDTO image { get; set; }
   }
}
