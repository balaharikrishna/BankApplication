using BankApplication.Models;
using BankApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repository.Repository
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
            IEnumerable<Branch> branches =  await _context.Branches.Where(b => b.BankId.Equals(bankId) && b.IsActive).ToListAsync();
            if (branches.Any())
            {
                return branches;
            }
            else
            {
              return  Enumerable.Empty<Branch>();
            }
        }

        public async Task<Branch?> GetBranchById(string branchId)
        {
            Branch? branch =  await _context.Branches.FirstOrDefaultAsync(b => b.BranchId.Equals(branchId) && b.IsActive);
            if (branch is not null)
            {
                return branch;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsBranchExist(string branchId)
        {
            return await _context.Branches.AnyAsync(b => b.BranchId.Equals(branchId) && b.IsActive);
        }

        public async Task<bool> AddBranch(Branch branch, string bankId)
        {
            branch.BankId = bankId;
            await _context.Branches.AddAsync(branch);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            Branch? branchObj = await GetBranchById(branch.BranchId);
            if (branch.BranchName is not null)
            {
                branchObj!.BranchName = branch.BranchName;
            }

            if (branch.BranchAddress is not null)
            {
                branchObj!.BranchAddress = branch.BranchAddress;
            }

            if (branch.BranchPhoneNumber is not null)
            {
                branchObj!.BranchPhoneNumber = branch.BranchPhoneNumber;
            }
            _context.Branches.Update(branchObj!);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBranch(string branchId)
        {
            Branch? branch = await GetBranchById(branchId);
            branch!.IsActive = false;
            _context.Branches.Update(branch);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<Branch?> GetBranchByName(string branchName)
        {
            Branch? branch =  await _context.Branches.FirstOrDefaultAsync(b => b.BranchName.Equals(branchName) && b.IsActive);
            if (branch is not null)
            {
                return branch;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<TransactionCharge>> GetAllTransactionCharges(string branchId)
        {
            IEnumerable<TransactionCharge> charges =  await _context.TransactionCharges.Where(c => c.BranchId.Equals(branchId) && c.IsActive).ToListAsync();
            if (charges.Any())
            {
                return charges;
            }
            else
            {
                return Enumerable.Empty<TransactionCharge>();
            }
        }
    }
}
