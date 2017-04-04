using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphSMSLib.Models
{
    public class Outbox :ICloneable
    {
        public int OuboxID { get; set; }

        public DateTime UpdatedInDb { get; set; }


        public DateTime SendingDateTime { get; set; }

        public string MessageText { get; set; }


        public string DestenationNumber { get; set; }



        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
