using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphSMSLib.Models
{
    public class Inbox
    {
        public int InboxID { get; set; }

        public DateTime UpdatedInDb { get; set; }

        public DateTime ReciveDateTime { get; set; }
        
        public string MessageText { get; set; }

        public string SenderNumber { get; set; }
    
        public ReadType ReadStatus { get; set; }
    }
}
