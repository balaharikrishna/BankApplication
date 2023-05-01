using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository.Repository
{
    public class ReserveBankManagerRepository : IReserveBankManagerRepository
    {
        private readonly BankDBContext _context;
        public ReserveBankManagerRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagers()
        {
            return await _context.ReserveBankManagers.Where(c => c.IsActive.Equals(true)).ToListAsync();
        }
        public async Task<bool> AddReserveBankManager(ReserveBankManager reserveBankManager)
        {
            await _context.ReserveBankManagers.AddAsync(reserveBankManager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateReserveBankManager(ReserveBankManager reserveBankManager)
        {
            ReserveBankManager reserveBankManagerObj = await GetReserveBankManagerById(reserveBankManager.AccountId);
            if (reserveBankManager.Name is not null)
            {
                reserveBankManagerObj.Name = reserveBankManager.Name;
            }

            if (reserveBankManager.Salt is not null)
            {
                reserveBankManagerObj.Salt = reserveBankManager.Salt;

                if (reserveBankManager.HashedPassword is not null)
                {
                    reserveBankManagerObj.HashedPassword = reserveBankManager.HashedPassword;
                }
            }
            _context.ReserveBankManagers.Update(reserveBankManagerObj);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReserveBankManager(string reserveBankManagerAccountId)
        {
            ReserveBankManager reserveBankManager = await GetReserveBankManagerById(reserveBankManagerAccountId);
            reserveBankManager.IsActive = false;
            _context.ReserveBankManagers.Update(reserveBankManager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsReserveBankManagerExist(string reserveBankManagerAccountId)
        {
            return await _context.ReserveBankManagers.AnyAsync(c => c.AccountId.Equals(reserveBankManagerAccountId) && c.IsActive.Equals(true));
        }
        public async Task<ReserveBankManager?> GetReserveBankManagerById(string reserveBankManagerAccountId)
        {
            return await _context.ReserveBankManagers.FirstOrDefaultAsync(c => c.AccountId.Equals(reserveBankManagerAccountId)
           && c.IsActive.Equals(true));
        }
        public async Task<ReserveBankManager?> GetReserveBankManagerByName(string reserveBankManagerName)
        {
            return await _context.ReserveBankManagers.FirstOrDefaultAsync(c => c.Name.Equals(reserveBankManagerName)
            && c.IsActive.Equals(true));
        }
    }
}
