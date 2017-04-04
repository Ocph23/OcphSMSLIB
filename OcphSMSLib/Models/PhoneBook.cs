using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OcphSMSLib.Models
{
    public class PhoneBook
    {

        public int PhoneBookID { get; set; }

        public int GroupID { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        public string City { get; set; }

        public string Description { get; set; }

    
    }
}
