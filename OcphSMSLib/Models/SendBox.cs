using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphSMSLib.Models
{
    public class SendBox
    {
        public int SendBoxID { get; set; }

        public DateTime UpdatedInDb { get; set; }

        public DateTime SendingDateTime { get; set; }

        public string MessageText { get; set; }

        public string DestenationNumber { get; set; }


        public string SMSCNumber { get; set; }

        public SendingStatus Status { get; set; }

    }
}
