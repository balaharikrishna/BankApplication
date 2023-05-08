using BankApplication.Models;

namespace BankApplication.Repository.IRepository
{
    public interface IBranchMembersRepository
    {
        Task<IEnumerable<string>> GetAllBranchMembers(string branchId);
    }
}