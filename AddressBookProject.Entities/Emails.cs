using AddressBook.Entities.Enums;
using AddressBookProject.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AddressBook.Entities
{
    public class Emails
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public EmailType EmailType { get; set; }
        public int PersonalDetailId { get; set; }
        public PersonalDetail PersonalDetail { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> EmailTypes { get; set; } = new List<SelectListItem>();
    }
}
