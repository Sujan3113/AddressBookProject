using AddressBook.Entities;
using AddressBookProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<PersonalDetail> PersonalDetails { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<Emails> Emails { get; set; }
        public DbSet<PermanentAddress> PermanentAddresses { get; set; }
        public DbSet<TemporaryAddress> TemporaryAddresses { get; set; }
    }
}