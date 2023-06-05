using AddressBook.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Contracts.Dto
{
    public class GetAll
    {
        //Person details
        public int PersonalDetailId { get; set; }
        [RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "First Name should be only in alphabet")]
        public string FirstName { get; set; }
        [RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "Middle Name should be only in alphabet")]
        public string MiddleName { get; set; }
        [RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "Last Name should be only in alphabet")]
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public Relationship Relationship { get; set; }

        //Emails except id,personaldetailid,Emailtype
        [RegularExpression(@"^[a-zA-Z0-9!#$%&.'*+/=?^_`{|}~-]*@[a-zA-Z]+\.com$", ErrorMessage = "eg:abc1@gmail.com")]
        //[RegularExpression(@"^[a-zA-Z]+[0-9]{0,3}@(gmail|yahoo|hotmail).com$", ErrorMessage = "eg:abc1@(gmail|yahoo|hotmail).com")]
        public string Email { get; set; }

        //Permanent Address except id and personaldetailId
        [RegularExpression("[a-zA-Z]+$", ErrorMessage = "Place Name should be only in alphabet")]
        public string PermanentPlaceName { get; set; }
        [RegularExpression("[a-zA-Z]+$", ErrorMessage = "District should be only in alphabet")]

        public string PermanentDistrict { get; set; }


        public Province PermanentProvince { get; set; }

        //Temporary Address except id and personaldetailId
        [RegularExpression("[a-zA-Z]+$", ErrorMessage = "Place Name should be only in alphabet")]
        public string TempPlaceName { get; set; }

        [RegularExpression("[a-zA-Z]+$", ErrorMessage = "District should be only in alphabet")]
        public string TempDistrict { get; set; }

        public Province TempProvince { get; set; }

        //Phone except id, personaldetailId,typeofNumber
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number should be only 10 digits")]
        public long PhoneNumber { get; set; }
        public IEnumerable<SelectListItem> Relationships { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TempProvinces { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PermanentProvinces { get; set; } = new List<SelectListItem>();
    }
    public class AddDto : GetAll
    {
        //Company
        public string CompanyName { get; set; }
        public string Department { get; set; }
        //personal detail id
        public int PersonalDetailId { get; set; }
        //Number type
        public NumberType NumberType { get; set; }
        //Email type
        public EmailType EmailType { get; set; }

        public IFormFile ProfilePicture { get;set; }
        public IEnumerable<SelectListItem> EmailTypes { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> NumberTypes { get; set; } = new List<SelectListItem>();

    }
    public class UpdateDto : AddDto
    {
        public int PersonalDetailId { get; set; }
        public int EmailId { get; set; }
        public int CompanyId { get; set; }
        public int PhoneId { get; set; }
        public int PermanentId { get; set; }
        public int TemporaryId { get; set; }
        public string SProfilePicture { get; set; }

    }
}
