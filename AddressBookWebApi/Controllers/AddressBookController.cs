using AddressBook.Application;
using AddressBook.Contracts;
using AddressBook.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : Controller
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IWebHostEnvironment _env;

        public AddressBookController(IAddressBookService addressBookService, IWebHostEnvironment env)
        {
            _addressBookService = addressBookService;
            _env = env;
        }

        [HttpGet(nameof(GetAllAddressBook))]
        public async Task<IActionResult> GetAllAddressBook()
        {
            try
            {
                return Ok(await _addressBookService.GetAllAddressBooks());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(nameof(AddAddressBook))]
        public async Task<IActionResult> AddAddressBook(AddDto addDto,IFormFile ProfilePicture)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ProfilePicture != null && ProfilePicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                        var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ProfilePicture.CopyToAsync(stream);
                        }
                        addDto.ProfilePicture = "/uploads/" + fileName;
                    }
                    await _addressBookService.AddAddressBook(addDto);
                    return Ok("Address book Added Successfully");
                }
                var data = await _addressBookService.GetAllEnum();
                return BadRequest(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
