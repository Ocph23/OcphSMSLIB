using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcphSMSLib.Models
{
    public class SMSMessage:ICloneable
    {
        public int Index { get; set; }
        public ReadType StatusRead{get;set;}
        public SendingStatus SendingStatus { get; set; }
        public string DestinationNumber{get;set;}
        public string SenderNumber {get;set;}
        public string MessageText { get; set; }
        public DateTime DateTime { get; set; }
         object ICloneable.Clone()
         {
             return this.MemberwiseClone();
         }

        private SMSMessage sms;
         internal SMSMessage GetClone()
         {
            
             if (this.MemberwiseClone() != null)
             {
                 sms = (SMSMessage)this.MemberwiseClone();
             }
             return sms;
         }
    }
}
