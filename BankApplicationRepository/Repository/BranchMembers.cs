using BankApplicationModels;
using BankApplicationRepository.IRepository;

namespace BankApplicationRepository.Repository
{
    public class BranchMembers : IBranchMembers
    {
        private readonly BankDBContext _context;

        public BranchMembers(BankDBContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<Bank>> GetAllBranchMembers(string branchId)
        //{
        //  // var data = (from managers in _context.Managers join staffs in _context.Staffs on managers.BranchId equals staffs.BranchId select new { managers.Name}.ToList();
        //    //  return await _context.Banks.Where(b => b.IsActive.Equals(true)).ToListAsync();
        //}
    }
}
