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
    public class TemporaryAddress
    {
        [Key]
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public string District { get; set; }
        public Province Province { get; set; }
        public int PersonalDetailId { get; set; }
        public PersonalDetail PersonalDetail { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> Provinces { get; set; } = new List<SelectListItem>();
    }
}
