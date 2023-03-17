using AddressBook.Contracts;
using AddressBook.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using PagedList;

namespace AddressBook.UI.Controllers
{
    public class AddressBookController : Controller
    {
        private readonly IAddressBookService _aBService;
        private readonly IWebHostEnvironment _env;

        public AddressBookController(IAddressBookService aBService, IWebHostEnvironment env)
        {
            _aBService = aBService;
            _env = env;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, string? currentFilter, int? pageNumber,int? pagesize=5)
        {
            var result = await _aBService.GetAllAddressBook(searchString, sortOrder, currentFilter, pageNumber,pagesize);
            return View(result);

        }

        public async Task<IActionResult> Create()
        {

            return View(await _aBService.GetAllEnum());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddDto addDto,IFormFile ProfilePicture)
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
                    await _aBService.AddAddressBook(addDto);
                return RedirectToAction("Index");
            }
            var data=await _aBService.GetAllEnum();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _aBService.DetailAddressBook(id);
            return View(data);
        }

        public async Task<IActionResult> Update(int id)
        {
            var data = await _aBService.DetailAddressBook(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateDto updateDto,IFormFile? ProfilePicture)
        {
            var data = await _aBService.DetailAddressBook(updateDto.PersonalDetailId);
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
                    updateDto.ProfilePicture = "/uploads/" + fileName;
                }

                if (ProfilePicture == null)
                {
                    updateDto.ProfilePicture = data.ProfilePicture;
                }
                var upd = await _aBService.UpdateAddressBook(updateDto);
                return RedirectToAction("Index");
            }          
            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = await _aBService.DetailAddressBook(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
    }
}
