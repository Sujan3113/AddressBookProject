using AddressBook.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBookProject.Entities
{
    public class PersonalDetail
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public Relationship Relationship { get; set; }
       
        [NotMapped]
        public IEnumerable<SelectListItem> Relationships { get; set; } = new List<SelectListItem>();
    }
}