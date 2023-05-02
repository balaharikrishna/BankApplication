using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly BankDBContext _context;
        public BranchRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Branch>> GetAllBranches(string bankId)
        {
            return await _context.Branches.Where(b =>b.BankId.Equals(bankId) && b.IsActive.Equals(true)).ToListAsync();
        }

        public async Task<Branch?> GetBranchById(string branchId)
        {
            return await _context.Branches.FirstOrDefaultAsync(b => b.BranchId.Equals(branchId) && b.IsActive.Equals(true));
        }

        public async Task<bool> IsBranchExist(string branchId)
        {
            return await _context.Branches.AnyAsync(b => b.BranchId.Equals(branchId) && b.IsActive.Equals(true));
        }

        public async Task<bool> AddBranch(Branch branch,string bankId)
        {
            branch.BankId = bankId;
            await _context.Branches.AddAsync(branch);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            Branch branchObj = await GetBranchById(branch.BranchId);
            if (branch.BranchName is not null)
            {
                branchObj.BranchName = branch.BranchName;
            }

            if (branch.BranchAddress is not null)
            {
                branchObj.BranchAddress = branch.BranchAddress;
            }

            if (branch.BranchPhoneNumber is not null)
            {
                branchObj.BranchPhoneNumber = branch.BranchPhoneNumber;
            }
            _context.Branches.Update(branchObj);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBranch(string branchId)
        {
            Branch branch = await GetBranchById(branchId);
            branch.IsActive = false;
            _context.Branches.Update(branch);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<Branch?> GetBranchByName(string branchName)
        {
            return await _context.Branches.FirstOrDefaultAsync(b => b.BranchName.Equals(branchName) && b.IsActive.Equals(true));
        }

        public async Task<IEnumerable<TransactionCharge>> GetAllTransactionCharges(string branchId)
        {
            return await _context.TransactionCharges.Where(c => c.BranchId.Equals(branchId) && c.IsActive.Equals(true)).ToListAsync();
        }
    }
}
