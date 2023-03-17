using AddressBook.Entities.Enums;
using AddressBookProject.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Entities
{
    public class Phone
    {
        [Key]
        public int Id { get; set; }
        public long PhoneNumber { get; set; }
        public NumberType NumberType { get; set; }
        public int PersonalDetailId { get; set; }
        public PersonalDetail PersonalDetail { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> NumberTypes { get; set; } = new List<SelectListItem>();

    }
}
