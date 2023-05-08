using BankApplication.Models;
using BankApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repository.Repository
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
            IEnumerable<ReserveBankManager> reserveBankManagers =  await _context.ReserveBankManagers.Where(c => c.IsActive).ToListAsync();
            if (reserveBankManagers.Any())
            {
                return reserveBankManagers;
            }
            else
            {
                return Enumerable.Empty<ReserveBankManager>();
            }
        }
        public async Task<bool> AddReserveBankManager(ReserveBankManager reserveBankManager)
        {
            await _context.ReserveBankManagers.AddAsync(reserveBankManager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateReserveBankManager(ReserveBankManager reserveBankManager)
        {
            ReserveBankManager? reserveBankManagerObj = await GetReserveBankManagerById(reserveBankManager.AccountId);
            if (reserveBankManager.Name is not null)
            {
                reserveBankManagerObj!.Name = reserveBankManager.Name;
            }

            if (reserveBankManager.Salt is not null)
            {
                reserveBankManagerObj!.Salt = reserveBankManager.Salt;

                if (reserveBankManager.HashedPassword is not null)
                {
                    reserveBankManagerObj.HashedPassword = reserveBankManager.HashedPassword;
                }
            }
            _context.ReserveBankManagers.Update(reserveBankManagerObj!);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReserveBankManager(string reserveBankManagerAccountId)
        {
            ReserveBankManager? reserveBankManager = await GetReserveBankManagerById(reserveBankManagerAccountId);
            reserveBankManager!.IsActive = false;
            _context.ReserveBankManagers.Update(reserveBankManager);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsReserveBankManagerExist(string reserveBankManagerAccountId)
        {
            return await _context.ReserveBankManagers.AnyAsync(c => c.AccountId.Equals(reserveBankManagerAccountId) && c.IsActive);
        }
        public async Task<ReserveBankManager?> GetReserveBankManagerById(string reserveBankManagerAccountId)
        {
            ReserveBankManager? reserveBankManager =  await _context.ReserveBankManagers.FirstOrDefaultAsync(c => c.AccountId.Equals(reserveBankManagerAccountId)
            && c.IsActive);
            if (reserveBankManager is not null)
            {
                return reserveBankManager;
            }
            else
            {
                return null;
            }
        }
        public async Task<ReserveBankManager?> GetReserveBankManagerByName(string reserveBankManagerName)
        {
            ReserveBankManager? reserveBankManager = await _context.ReserveBankManagers.FirstOrDefaultAsync(c => c.Name.Equals(reserveBankManagerName)
            && c.IsActive);
            if (reserveBankManager is not null)
            {
                return reserveBankManager;
            }
            else
            {
                return null;
            }
        }
    }
}
