using AddressBook.Contracts.Dto;
using AddressBook.Entities;
using AddressBookProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace AddressBook.Contracts
{
    public interface IAddressBookService
    {
        Task<List<GetAll>> GetAllAddressBook(string searchString, string sortOrder, string currentFilter, int? pageNumber, int? pageSize);
        Task<List<GetAll>> GetAllAddressBooks();
        Task<AddDto> GetAllEnum();
        Task<AddDto> AddAddressBook(AddDto addDto);
        Task<UpdateDto> DetailAddressBook(int id);
        Task<UpdateDto> UpdateAddressBook(UpdateDto updateDto);
        Task<UpdateDto> DeleteAddressBook(int id);

    }
}
