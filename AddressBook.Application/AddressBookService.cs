using AddressBook.Contracts;
using AddressBook.Contracts.Dto;
using AddressBook.Entities;
using AddressBook.Entities.Enums;
using AddressBook.EntityFramework;
using AddressBookProject.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AddressBook.Application
{
    public class AddressBookService : IAddressBookService
    {
        private readonly AppDbContext _context;
        public AddressBookService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<GetAll>> GetAllAddressBook(string searchString, string sortColumn, string currentFilter, int? pageNumber, int? pageSize)
        {
            var allList = from pd in _context.PersonalDetails
                          join c in _context.Company on pd.Id equals c.PersonalDetailId
                          join e in _context.Emails on pd.Id equals e.PersonalDetailId
                          join pa in _context.PermanentAddresses on pd.Id equals pa.PersonalDetailId
                          join ta in _context.TemporaryAddresses on pd.Id equals ta.PersonalDetailId
                          join ph in _context.Phone on pd.Id equals ph.PersonalDetailId
                          select new GetAll()
                          {
                              PersonalDetailId = pd.Id,
                              FirstName = pd.FirstName,
                              MiddleName = pd.MiddleName,
                              LastName = pd.LastName,
                              ProfilePicture = pd.ProfilePicture,
                              Relationship = pd.Relationship,
                              Email = e.Email,
                              PermanentPlaceName = pa.PlaceName,
                              PermanentDistrict = pa.District,
                              PermanentProvince = pa.Province,
                              TempPlaceName = ta.PlaceName,
                              TempDistrict = ta.District,
                              TempProvince = ta.Province,
                              PhoneNumber = ph.PhoneNumber
                          };
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                allList = allList.Where(c => c.FirstName!.Contains(searchString) || c.Email!.Contains(searchString) || c.PhoneNumber.ToString()!.Contains(searchString));
            }
            switch (sortColumn)
            {
                case "FirstName":
                    allList = allList.OrderByDescending(c => c.FirstName);
                    break;
                default:
                    allList = allList.OrderBy(c => c.FirstName);
                    break;

            }

            return (await PaginatedList<GetAll>.CreateAsync(allList.AsNoTracking(), pageNumber ?? 1, pageSize ?? 5));
        }
        public async Task<List<GetAll>> GetAllAddressBooks()
        {
            var allList = from pd in _context.PersonalDetails
                          join c in _context.Company on pd.Id equals c.PersonalDetailId
                          join e in _context.Emails on pd.Id equals e.PersonalDetailId
                          join pa in _context.PermanentAddresses on pd.Id equals pa.PersonalDetailId
                          join ta in _context.TemporaryAddresses on pd.Id equals ta.PersonalDetailId
                          join ph in _context.Phone on pd.Id equals ph.PersonalDetailId
                          select new GetAll()
                          {
                              PersonalDetailId = pd.Id,
                              FirstName = pd.FirstName,
                              MiddleName = pd.MiddleName,
                              LastName = pd.LastName,
                              ProfilePicture = pd.ProfilePicture,
                              Relationship = pd.Relationship,
                              Email = e.Email,
                              PermanentPlaceName = pa.PlaceName,
                              PermanentDistrict = pa.District,
                              PermanentProvince = pa.Province,
                              TempPlaceName = ta.PlaceName,
                              TempDistrict = ta.District,
                              TempProvince = ta.Province,
                              PhoneNumber = ph.PhoneNumber
                          };

            return (await allList.ToListAsync());
        }

        public class PaginatedList<T> : List<T>
        {
            public int PageIndex { get; private set; }
            public int TotalPages { get; private set; }

            public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
            {
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                this.AddRange(items);
            }

            public bool HasPreviousPage => PageIndex > 1;

            public bool HasNextPage => PageIndex < TotalPages;

            public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
            {
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
        }
        public async Task<AddDto> AddAddressBook(AddDto addDto)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //Add Personal Detail
                    PersonalDetail personalDetail = new PersonalDetail()
                    {
                        FirstName = addDto.FirstName,
                        MiddleName = addDto.MiddleName,
                        LastName = addDto.LastName,
                        ProfilePicture = addDto.ProfilePicture,
                        Relationship = addDto.Relationship,
                    };
                    var pD = _context.Add(personalDetail).Entity;
                    _context.SaveChanges();

                    //Add Company
                    Company company = new Company()
                    {
                        CompanyName = addDto.CompanyName,
                        Department = addDto.Department,
                        PersonalDetailId = pD.Id
                    };
                    _context.Add(company);
                    await _context.SaveChangesAsync();


                    TemporaryAddress temporaryAddress = new TemporaryAddress()
                    {
                        PlaceName = addDto.TempPlaceName,
                        District = addDto.TempDistrict,
                        Province = addDto.TempProvince,
                        PersonalDetailId = pD.Id
                    };
                    _context.Add(temporaryAddress);
                    await _context.SaveChangesAsync();


                    //Add Permanent Address
                    PermanentAddress permanentAddress = new PermanentAddress()
                    {
                        PlaceName = addDto.PermanentPlaceName,
                        District = addDto.PermanentDistrict,
                        Province = addDto.PermanentProvince,
                        PersonalDetailId = pD.Id
                    };
                    _context.Add(permanentAddress);
                    await _context.SaveChangesAsync();

                    //Add phone
                    Phone phone = new Phone()
                    {
                        PhoneNumber = addDto.PhoneNumber,
                        NumberType = addDto.NumberType,
                        PersonalDetailId = pD.Id
                    };
                    _context.Add(phone);
                    await _context.SaveChangesAsync();

                    //Add email
                    Emails emails = new Emails()
                    {
                        Email = addDto.Email,
                        EmailType = addDto.EmailType,
                        PersonalDetailId = pD.Id
                    };
                    _context.Add(emails);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    trans.Rollback();

                }
            }
            return null;
        }


        public async Task<UpdateDto> UpdateAddressBook(UpdateDto updateDto)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //Update Personal Detail
                    PersonalDetail personalDetail = new PersonalDetail()
                    {
                        Id = updateDto.PersonalDetailId,
                        FirstName = updateDto.FirstName,
                        MiddleName = updateDto.MiddleName,
                        LastName = updateDto.LastName,
                        ProfilePicture = updateDto.ProfilePicture,
                        Relationship = updateDto.Relationship,
                    };
                    var pD = _context.Update(personalDetail).Entity;
                    _context.SaveChanges();

                    //Update Company
                    Company company = new Company()
                    {
                        Id = updateDto.CompanyId,
                        CompanyName = updateDto.CompanyName,
                        Department = updateDto.Department,
                        PersonalDetailId = pD.Id
                    };
                    _context.Update(company);
                    await _context.SaveChangesAsync();

                    //update temp address
                    TemporaryAddress temporaryAddress = new TemporaryAddress()
                    {
                        Id = updateDto.TemporaryId,
                        PlaceName = updateDto.TempPlaceName,
                        District = updateDto.TempDistrict,
                        Province = updateDto.TempProvince,
                        PersonalDetailId = pD.Id
                    };
                    _context.Update(temporaryAddress);
                    await _context.SaveChangesAsync();


                    //update Permanent Address
                    PermanentAddress permanentAddress = new PermanentAddress()
                    {
                        Id = updateDto.PermanentId,
                        PlaceName = updateDto.PermanentPlaceName,
                        District = updateDto.PermanentDistrict,
                        Province = updateDto.PermanentProvince,
                        PersonalDetailId = pD.Id
                    };
                    _context.Update(permanentAddress);
                    await _context.SaveChangesAsync();

                    //Update Phone
                    Phone phone = new Phone()
                    {
                        Id = updateDto.PhoneId,
                        PhoneNumber = updateDto.PhoneNumber,
                        NumberType = updateDto.NumberType,
                        PersonalDetailId = pD.Id
                    };
                    _context.Update(phone);
                    await _context.SaveChangesAsync();


                    //Update email
                    Emails emails = new Emails()
                    {
                        Id = updateDto.EmailId,
                        Email = updateDto.Email,
                        EmailType = updateDto.EmailType,
                        PersonalDetailId = pD.Id
                    };
                    _context.Update(emails);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
            return null;
        }

        public async Task<UpdateDto> DetailAddressBook(int id)
        {
            var abDetail = await (from pd in _context.PersonalDetails
                                  join c in _context.Company on pd.Id equals c.PersonalDetailId
                                  join e in _context.Emails on pd.Id equals e.PersonalDetailId
                                  join pa in _context.PermanentAddresses on pd.Id equals pa.PersonalDetailId
                                  join ta in _context.TemporaryAddresses on pd.Id equals ta.PersonalDetailId
                                  join ph in _context.Phone on pd.Id equals ph.PersonalDetailId
                                  where pd.Id == id
                                  select new UpdateDto
                                  {
                                      PersonalDetailId = pd.Id,
                                      FirstName = pd.FirstName,
                                      MiddleName = pd.MiddleName,
                                      LastName = pd.LastName,
                                      ProfilePicture = pd.ProfilePicture,
                                      Relationship = pd.Relationship,
                                      Email = e.Email,
                                      PermanentPlaceName = pa.PlaceName,
                                      PermanentDistrict = pa.District,
                                      PermanentProvince = pa.Province,
                                      TempPlaceName = ta.PlaceName,
                                      TempDistrict = ta.District,
                                      TempProvince = ta.Province,
                                      PhoneNumber = ph.PhoneNumber,
                                      CompanyName = c.CompanyName,
                                      Department = c.Department,
                                      NumberType = ph.NumberType,
                                      EmailType = e.EmailType,
                                      EmailId = e.Id,
                                      CompanyId = c.Id,
                                      PhoneId = ph.Id,
                                      PermanentId = pa.Id,
                                      TemporaryId = ta.Id
                                  }).FirstOrDefaultAsync();            
            abDetail.Relationships = PopulateRelation();
            abDetail.EmailTypes = PopulateEmail();
            abDetail.NumberTypes = PopulateNumber();
            
            abDetail.PermanentProvinces = PopulateProvince();
            
            abDetail.TempProvinces = PopulateProvince();
            return abDetail;

        }

        public async Task<UpdateDto> DeleteAddressBook(int id)
        {
            var getData = await _context.PersonalDetails.FindAsync(id);
            if (getData! == null)
            {
                _context.PersonalDetails.Remove(getData);
                await _context.SaveChangesAsync();
            }
            return new UpdateDto();

        }
        public IEnumerable<SelectListItem> PopulateRelation()
        {
            var relation = from Relationship r in Enum.GetValues(typeof(Relationship))
                           select new SelectListItem { Value = r.ToString(), Text = r.ToString() };

            return relation;
        }
        public IEnumerable<SelectListItem> PopulateEmail()
        {
            var emailType = from EmailType r in Enum.GetValues(typeof(EmailType))
                            select new SelectListItem { Value = r.ToString(), Text = r.ToString() };

            return emailType;
        }
        public IEnumerable<SelectListItem> PopulateNumber()
        {
            var numberType = from NumberType r in Enum.GetValues(typeof(NumberType))
                             select new SelectListItem { Value = r.ToString(), Text = r.ToString() };

            return numberType;
        }
        public IEnumerable<SelectListItem> PopulateProvince()
        {
            var provinceType = from Province r in Enum.GetValues(typeof(Province))
                               select new SelectListItem { Value = r.ToString(), Text = r.ToString() };

            return provinceType;
        }

        public async Task<AddDto> GetAllEnum()
        {
            AddDto addDto = new AddDto();
            addDto.Relationships = PopulateRelation();
            addDto.EmailTypes = PopulateEmail();
            addDto.NumberTypes = PopulateNumber();
            addDto.PermanentProvinces = PopulateProvince();
            addDto.TempProvinces = PopulateProvince();

            return addDto;
        }


    }

}