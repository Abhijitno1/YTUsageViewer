using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YTUsageViewer.Models
{
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Column("Phone_Mobile")]
        public string PhoneMobile { get; set; }
        [Column("Phone_Work")]
        public string PhoneWork { get; set; }
        [Column("Phone_Home")]
        public string PhoneHome { get; set; }
        public string PreferredPhone { get; set; }
        public string Email { get; set; }
    }
}