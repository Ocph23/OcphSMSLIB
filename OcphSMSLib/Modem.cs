using OcphSMSLib.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OcphSMSLib
{

    public delegate void ExecMessage(int index);
    public delegate void CheckMessageIsSend();
    public delegate void DelegateSendMessage(SendBox sms);
    public delegate void OnIncomming(Inbox inbox, EventArgs args);
    public delegate void OnSending(SendBox sendbox, EventArgs args);
    public delegate void OnDeleteOutbox(Outbox outbox, EventArgs args);
     
    public class Modem
    {
        private SerialPort serial;
        private string portName="";
        private int boundRate=115200;
        private int dataBits=8;
        private int stopBits=1;
        private int readTimeOut=300;
        private int writeTimeOut=300;
        private String indata;
        private Outbox SMSData;
        public event OnIncomming OnReciveSMS;
        public event OnSending OnSendingSMS;
        public event OnDeleteOutbox OnDeletingSMS;
        
        public Modem(string portName)
        {
            this.PortName = portName;
            serial = new SerialPort();
        }
        public Modem()
        {
            serial = new SerialPort();
        }
        public SerialPort Port
        {
            get { return serial; }
            set { serial = value; }
        }
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        public int BoundRate
        {
            get { return boundRate; }
            set { boundRate = value; }
        }
        public int DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }
        public int StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }
        public int ReadTimeOut
        {
            get { return readTimeOut; }
            set { readTimeOut = value; }
        }
        public int WriteTimeOut
        {
            get { return writeTimeOut; }
            set { writeTimeOut = value; }
        }


        public string Signature { get; set; }


        public bool Connect()
        {

            if (OnSendingSMS != null)
            { 
                
            }

            bool Connected = false;
            try
            {
                serial.PortName = PortName;
                serial.BaudRate = this.BoundRate;
                serial.DataBits = this.DataBits;
                serial.StopBits = System.IO.Ports.StopBits.One;
                serial.ReadTimeout = this.ReadTimeOut;
                serial.WriteTimeout = this.WriteTimeOut;
                serial.DataReceived += new SerialDataReceivedEventHandler(serial_DataReceived);
                serial.PinChanged += Serial_PinChanged;
                serial.ErrorReceived += Serial_ErrorReceived;
                serial.Open();
                serial.DtrEnable = true;
                serial.RtsEnable = true;
                Connected = true;
                this.SetModeSMS(SMSMOde.Text);
                Task.Factory.StartNew(() => this.ReadMessageFromSimAll());

            }
            catch (Exception Ex)
            {
                throw new SystemException(Ex.Message);
                
            }
            return Connected;
        }

        private void Serial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void Serial_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        private void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            if (sp.ReadBufferSize > 0)
            {
                indata = sp.ReadExisting();
                if (indata.Trim().Contains("AT+CMGS"))
                {
                    var sms = this.SMSData.Clone();
                    string data = indata.Clone().ToString();
                    Task.Factory.StartNew(() => this.AddNewMessage(data, sms));
                }else
                    if (indata.Trim().Contains("+CMS"))
                    {
                        var sms = this.SMSData.Clone();
                        string data = indata.Clone().ToString();
                        Task.Factory.StartNew(() => this.AddNewMessage(data, sms));
                    }
                    else
                    {
                        if (this.SMSData != null)
                        {
                            var sms = this.SMSData.Clone();
                            string data = indata.Clone().ToString();
                            Task.Factory.StartNew(() => this.AddNewMessage(data, sms));
                        }
                        else
                        {
                            string data = indata.Clone().ToString();
                            Task.Factory.StartNew(() => this.AddNewMessage(data, null));
                        }
                    
                        
                      
                        
                    }
                
                indata = string.Empty;
            }

        }

        public void SetModeSMS(SMSMOde mode)
        {
            if (mode == SMSMOde.Text)
            {
                this.ExecuteCommand("AT+CMGF=1\r");
                this.ExecuteCommand("AT+CSCA=\"" + "081100000" + "\"\r\n");
                System.Threading.Thread.Sleep(3500);
            }
        }

        private void SendMessageToSIM(Outbox message)
        {
           
            try
            {
                serial.BaseStream.Flush();
                string cb = char.ConvertFromUtf32(26);
               // this.ExecuteCommand("AT+CMGF=1\r");
               // this.ExecuteCommand("AT+CSCA=\""+"081100000"+"\"\r\n");//Ufone              Service Center   
                this.SMSData = message; 
                this.ExecuteCommand("AT+CMGS=\"" + message.DestenationNumber+ "\"\r\n");// 
                this.ExecuteCommand((message.MessageText+"\r\n#"+this.Signature+"#") + cb);//message text message sending
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        
        }



        public void TestExecute()
        {

            try
            {
                serial.BaseStream.Flush();
                string cb = char.ConvertFromUtf32(26);
                // this.ExecuteCommand("AT+CMGF=1\r");
                // this.ExecuteCommand("AT+CSCA=\""+"081100000"+"\"\r\n");//Ufone              Service Center   
                this.ExecuteCommand("AT+CMSS=112" + "\"\r\n");// 
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }

        }




        public void ReadMessageFromSimAll()
        {
            try
            {
                serial.BaseStream.Flush();
                string cb = char.ConvertFromUtf32(26);
               //Ufone              Service Center                    
                this.ExecuteCommand("AT+CMGL=\"" + "ALL" + "\"\r\n");
                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                throw new SystemException( ex.Message);
            }
        
        }

        internal void ReadMessageFromSim(int index)
        {
            try
            {
                serial.BaseStream.Flush();
                System.Threading.Thread.Sleep(3500);
                string cb = char.ConvertFromUtf32(26);

            //    this.ExecuteCommand("AT+CMGF=1\r");
                this.ExecuteCommand("AT+CMGR="+index.ToString() + "\r");
                System.Threading.Thread.Sleep(3500);
            }
            catch (Exception ex)
            {

                throw new SystemException(ex.Message);
            }

        }

        internal void ExecuteCommand(string command)
        {
            try
            {
                if (this.serial.IsOpen)
                {
                    // to send bulk sms
                    serial.WriteLine(command);
                }
            }
            catch (Exception ex)
            {
                serial.Close();
                throw ex;
            }
        }



        public  void Close()
        {
            serial.Close();
        }

        internal void DeleteMessageInSIM(int index)
        {
            try
            {
                serial.BaseStream.Flush();
                System.Threading.Thread.Sleep(3500);
                string cb = char.ConvertFromUtf32(26);

                this.ExecuteCommand("AT+CMGD="+index+ "\r\n"); 
                System.Threading.Thread.Sleep(3500);
            }
            catch (Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }


        public void SendMessage(SMSMessage message)
        {
            var item = new Outbox
            {
                DestenationNumber = message.DestinationNumber,
                MessageText = message.MessageText,
                SendingDateTime = DateTime.Now
            };
            this.SendMessageToSIM(item);

        }


        public void AddNewMessage(string messages, object objsms)
        {


            if (messages.Contains("CREG: 1"))
            {
            }

            if (messages.Contains("+CMGS"))
            {
                var obj =(Outbox)objsms;
                if (messages.Contains("ERROR"))
                {
                    SendBox send = OcphSMSCommonLib.ConverOutboxToSendbox(obj);
                    send.Status = SendingStatus.SendingError;
                    if (OnSendingSMS != null)
                    {
                        OnSendingSMS(send, null);
                    }
                }
                else if(messages.Contains("OK"))
                {
                    SendBox send = OcphSMSCommonLib.ConverOutboxToSendbox(obj);
                    send.Status = SendingStatus.SendingOK;
                    if (OnSendingSMS != null)
                    {
                        OnSendingSMS(send, null);
                    }
                }

            }


            if (messages.Contains("AT+CMGS"))
            {
                var obj =(Outbox)objsms;
                if (obj != null)
                {
                    if (OnDeletingSMS != null)
                    {
                        OnDeletingSMS(obj, null);
                    }


                }
                    
            }

            if (messages.Contains("+CMTI"))
            {
                char[] delimiters = new char[] { '\r', '\n' };
                var res = messages.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                var indexs = res[0].ToString().Split(',');
                var messageIndex = Convert.ToInt32(indexs[1]);
                ReadMessageFromSim(messageIndex);
                DeleteMessageInSIM(messageIndex);

            }

            if (messages.Contains("+CMGL:"))
            {
                var i = messages.IndexOf("+CMGL:");
                var rm = messages.Substring(i).Replace('\n', ' ');

                string[] stringSeparators = new string[] { "+CMGL:", "\n" };

                var realmessage = rm.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                List<Inbox> listmessage = new List<Inbox>();
                foreach (var item in realmessage)
                {
                    Inbox sms = new Inbox();
                    char[] delimiters = new char[] { '\r', '\n', '\"', ',' };
                    var arrs = item.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (arrs.Length >= 5)
                    {
                        sms.InboxID = Int32.Parse(arrs[0]);
                        sms.SenderNumber = arrs[2].ToString().Trim();
                        sms.MessageText = arrs[5];
                        var st = arrs[1].Split(' ');
                        sms.ReadStatus = (ReadType)Enum.Parse(typeof(ReadType), st[1]);
                        var date = Convert.ToDateTime(arrs[3]);
                        var time = TimeSpan.Parse(arrs[4].Split('+')[0]);
                        sms.ReciveDateTime = OcphSMSCommonLib.ConverDateFromSIM(arrs[3] + "," + arrs[4]);
                        listmessage.Add(sms);
                    }
                }
                Task.Factory.StartNew(() => InsertListToInBox(listmessage));
            }



            if (messages.Contains("+CMGR:"))
            {
                char[] delimiters = new char[] { '\r', '\n', '"' };
                var i = messages.IndexOf("+CMGR:");
                var realmessage = messages.Substring(i);
                var res = realmessage.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                Inbox sms = new Inbox();
                var stat = res[1].Split(' ');
                sms.ReadStatus = (ReadType)Enum.Parse(typeof(ReadType), stat[1].ToString());
                sms.SenderNumber = res[3];
                sms.MessageText = res[6];
                sms.ReciveDateTime = OcphSMSCommonLib.ConverDateFromSIM(res[5]);
                if (OnReciveSMS != null)
                {
                    OnReciveSMS(sms, null);
                }


            }
               
        }
        private void InsertListToInBox(List<Inbox> listmessage)
        {

            foreach (var item in listmessage.OrderBy(O => O.ReciveDateTime))
            {
                DeleteMessageInSIM(item.InboxID);
                if (OnReciveSMS != null)
                {
                    OnReciveSMS(item, null);
                }

                
            }
            
        }




    }
}
