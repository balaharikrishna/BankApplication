using BankApplication.Models;
using BankApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repository.Repository
{
    public class HeadManagerRepository : IHeadManagerRepository
    {
        private readonly BankDBContext _context;
        public HeadManagerRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<HeadManager>> GetAllHeadManagers(string bankId)
        {
            IEnumerable<HeadManager> headManagers = await _context.HeadManagers.Where(c => c.BankId.Equals(bankId) && c.IsActive).ToListAsync();
            if (headManagers.Any())
            {
                return headManagers;
            }
            else
            {
                return Enumerable.Empty<HeadManager>();
            }
        }

        public async Task<bool> AddHeadManagerAccount(HeadManager headManager, string bankId)
        {
            headManager.BankId = bankId;
            await _context.HeadManagers.AddAsync(headManager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateHeadManagerAccount(HeadManager headManager, string bankId)
        {
            HeadManager? headManagerObj = await GetHeadManagerById(headManager.AccountId, bankId);
            if (headManager.Name is not null)
            {
                headManagerObj!.Name = headManager.Name;
            }

            if (headManager.Salt is not null)
            {
                headManagerObj!.Salt = headManager.Salt;

                if (headManager.HashedPassword is not null)
                {
                    headManagerObj.HashedPassword = headManager.HashedPassword;
                }
            }

            _context.HeadManagers.Update(headManagerObj!);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHeadManagerAccount(string headManagerAccountId, string bankId)
        {
            HeadManager? headManager = await GetHeadManagerById(headManagerAccountId, bankId);
            headManager!.IsActive = false;
            _context.HeadManagers.Update(headManager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsHeadManagerExist(string headManagerAccountId, string bankId)
        {
            return await _context.HeadManagers.AnyAsync(c => c.AccountId.Equals(headManagerAccountId) && c.BankId.Equals(bankId) && c.IsActive.Equals(true));
        }
        public async Task<HeadManager?> GetHeadManagerById(string headManagerAccountId, string bankId)
        {
            HeadManager? headManager =  await _context.HeadManagers.FirstOrDefaultAsync(c => c.AccountId.Equals(headManagerAccountId)
            && c.BankId.Equals(bankId) && c.IsActive);
            if (headManager is not null)
            {
                return headManager;
            }
            else
            {
                return null;
            }
        }
        public async Task<HeadManager?> GetHeadManagerByName(string headManagerName, string bankId)
        {
            HeadManager? headManager = await _context.HeadManagers.FirstOrDefaultAsync(c => c.Name.Equals(headManagerName)
            && c.BankId.Equals(bankId) && c.IsActive);
            if (headManager is not null)
            {
                return headManager;
            }
            else
            {
                return null;
            }
        }
    }
}
