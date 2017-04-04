using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcphSMSLib
{
    public class EnumCollections
    {
        public static List<OcphSMSLib.Religion> GetReligions()
        {
            List<OcphSMSLib.Religion> list = new List<OcphSMSLib.Religion>();
            var item = typeof(OcphSMSLib.Religion).GetEnumValues();
            foreach (var a in item)
            {
                list.Add((OcphSMSLib.Religion)a);
            }

            return list;
        }
    }


    public enum SMSMOde
    {
        Text,
        Decoder
    }
    public enum ReadType
    {
        READ,
        UNREAD
    }

    public enum SendingStatus
    {
        SendingOK,
        SendingOKNoReport,
        SendingError,
        DeliveryOK,
        DeliveryFailed,
        DeliveryPending,
        DeliveryUnknown,
        Error
    }


    public enum Religion
    {
        Islam=1,
        Kristen,
        Katolik,
        Hindu,
        Budha,
        KongHuchu
    }


    public enum PhoneType
    {


    }



    public enum RowStatus
    { 
        New,
        Old,
        Change
    }
}
