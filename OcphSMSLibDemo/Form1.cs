using OcphSMSLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OcphSMSLib.Models;

namespace OcphSMSLibDemo
{
    public partial class Form1 : Form
    {
        private Modem modem;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyConfiguration config = new MyConfiguration();
            try
            {
                this.modem = new Modem(config.DevicePort);

                this.modem.OnReciveSMS += Modem_OnReciveSMS;
                this.modem.OnSendingSMS += Modem_OnSendingSMS;


                modem.Connect();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
           
        }

        private void Modem_OnSendingSMS(OcphSMSLib.Models.SendBox sendbox, EventArgs args)
        {
            //
        }

        private void Modem_OnReciveSMS(OcphSMSLib.Models.Inbox inbox, EventArgs args)
        {
            Task.Factory.StartNew(() => this.HandleMessage(inbox));
        }

        private void HandleMessage(Inbox inbox)
        {
            var data = inbox.MessageText.Split(' ');
            switch (data[0])
            {
                case "INFO":

                    var text = "INFO = Daftar Perintah"+Environment.NewLine+
                        "PINDAH SHIFT2= M       xqinta Pindah Jam";
                    var message = new OcphSMSLib.Models.SMSMessage { DateTime = DateTime.Now, DestinationNumber = inbox.SenderNumber, MessageText = text };
                    this.modem.SendMessage(message);
                    break;

                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var message = new OcphSMSLib.Models.SMSMessage { DateTime = DateTime.Now, DestinationNumber = textBox1.Text, MessageText = textBox2.Text };
            this.modem.SendMessage(message);
        }
    }
}
