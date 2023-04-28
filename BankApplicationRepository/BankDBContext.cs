using BankApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository
{
    public class BankDBContext : DbContext
    {
        public BankDBContext(DbContextOptions<BankDBContext> options) : base(options)
        {

        }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<TransactionCharges> TransactionCharges { get; set; }
        public DbSet<ReserveBankManager> ReserveBankManagers { get; set; }
        public DbSet<HeadManager> HeadManagers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

       
    }
}
