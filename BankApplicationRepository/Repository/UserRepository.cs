using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BankDBContext _context;
        public UserRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<AuthenticateUser?> GetUserAuthenticationDetails(string accountId)
        {
            var result = await (from reserveBankManagers in _context.ReserveBankManagers
                                where reserveBankManagers.IsActive && reserveBankManagers.AccountId.Equals(accountId)
                                select new { reserveBankManagers.AccountId, reserveBankManagers.Name, reserveBankManagers.Role, reserveBankManagers.Salt, reserveBankManagers.HashedPassword })
              .Union(from headManagers in _context.HeadManagers
                     where headManagers.IsActive && headManagers.AccountId.Equals(accountId)
                     select new { headManagers.AccountId, headManagers.Name, headManagers.Role, headManagers.Salt, headManagers.HashedPassword })
              .Union(from managers in _context.Managers
                     where managers.IsActive && managers.AccountId.Equals(accountId)
                     select new { managers.AccountId, managers.Name, managers.Role, managers.Salt, managers.HashedPassword })
              .Union(from staffs in _context.Staffs
                     where staffs.IsActive && staffs.AccountId.Equals(accountId)
                     select new { staffs.AccountId, staffs.Name, staffs.Role, staffs.Salt, staffs.HashedPassword })
              .Union(from customers in _context.Customers
                     where customers.IsActive && customers.AccountId.Equals(accountId)
                     select new { customers.AccountId, customers.Name, customers.Role, customers.Salt, customers.HashedPassword })
              .FirstOrDefaultAsync();

            if (result is not null)
            {
                AuthenticateUser user = new()
                {
                    AccountId = result.AccountId,
                    Name = result.Name,
                    Role = (Roles)result.Role,
                    Salt = result.Salt,
                    HashedPassword = result.HashedPassword
                };
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
