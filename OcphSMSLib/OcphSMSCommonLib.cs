using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcphSMSLib.Models;

namespace OcphSMSLib
{
    public class OcphSMSCommonLib
    {
        public static DateTime ConverDateFromSIM(string datetime)
        {
            DateTime result;
            var dt = datetime.Split(',');
            var s = dt[0].ToString().Split('/');
            string newDate = s[0] + s[1] + s[2];
            DateTime date = DateTime.ParseExact(newDate, "yyMMdd", null);

            var times = dt[1].Split('+');
            TimeSpan ts = TimeSpan.Parse(times[0]);
            TimeSpan ts1 = TimeSpan.Parse(string.Format("00:00:{0}", times[1].ToString()));
            TimeSpan tsResult = ts.Subtract(ts1);
            result = date.Subtract(tsResult);
            return result;

        }


        public static SendBox ConverOutboxToSendbox(Outbox obj)
        {
            SendBox send = new SendBox();
            send.DestenationNumber = obj.DestenationNumber;
            send.MessageText = obj.MessageText;
            send.SendBoxID = 0;
            send.SendingDateTime = obj.SendingDateTime;
            send.UpdatedInDb = obj.UpdatedInDb;
            return send;
        }

    }
}
