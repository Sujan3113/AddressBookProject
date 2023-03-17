using AddressBookProject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Department { get; set; }
        public int PersonalDetailId { get; set; }
        public PersonalDetail PersonalDetail { get; set; }
    }
}
