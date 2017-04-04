using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcphSMSLib.Models
{
    public class PhoneBookDetails
    {
        public RowStatus RowStatus;
        public int PhoneBookDetailsID { get; set; }

        public PhoneType PhoneType { get; set; }


        public string Content { get; set; }
        public string Description { get; set; }
        public bool Actived { get; set; }


    }
}
