using BankApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationRepository.IRepository
{
    public interface IBranchRepository
    {
        Task<bool> AddBranch(Branch branch, string bankId);
        Task<bool> DeleteBranch(string branchId);
        Task<IEnumerable<Branch>> GetAllBranches();
        Task<Branch> GetBranchById(string branchId);
        Task<bool> UpdateBranch(Branch branch);
        Task<bool> IsBranchExist(string branchId);
        Task<Branch?> GetBranchByName(string branchName);
        Task<IEnumerable<TransactionCharges>> GetAllTransactionCharges(string bankId);
    }
}
