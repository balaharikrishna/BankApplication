using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly BankDBContext _context;
        public ManagerRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Manager?>> GetAllManagers(string branchId)
        {
            return await _context.Managers.Where(c => c.BranchId.Equals(branchId) && c.IsActive.Equals(true)).ToListAsync();
        }
        public async Task<bool> AddManagerAccount(Manager manager, string branchId)
        {
            manager.BranchId = branchId;
            await _context.Managers.AddAsync(manager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateManagerAccount(Manager manager, string branchId)
        {
            Manager managerObj = await GetManagerById(manager.AccountId, branchId);

            if (manager.Name is not null)
            {
                managerObj.Name = manager.Name;
            }

            if (manager.Salt is not null)
            {
                managerObj.Salt = manager.Salt;

                if (manager.HashedPassword is not null)
                {
                    managerObj.HashedPassword = manager.HashedPassword;
                }
            }

            _context.Managers.Update(managerObj);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteManagerAccount(string managerAccountId, string branchId)
        {
            Manager manager = await GetManagerById(managerAccountId, branchId);
            manager.IsActive = false;
            _context.Managers.Update(manager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsManagerExist(string managerAccountId, string branchId)
        {
            return await _context.Managers.AnyAsync(c => c.AccountId.Equals(managerAccountId) && c.BranchId.Equals(branchId) && c.IsActive.Equals(true));
        }
        public async Task<Manager?> GetManagerById(string managerAccountId, string branchId)
        {
            return await _context.Managers.FirstOrDefaultAsync(c => c.AccountId.Equals(managerAccountId)
            && c.BranchId.Equals(branchId) && c.IsActive.Equals(true));
        }
        public async Task<Manager?> GetManagerByName(string managerName, string branchId)
        {
            return await _context.Managers.FirstOrDefaultAsync(c => c.Name.Equals(managerName)
            && c.BranchId.Equals(branchId) && c.IsActive.Equals(true));
        }
    }
}
