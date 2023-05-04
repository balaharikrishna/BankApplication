using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface IBranchMembersRepository
    {
        Task<IEnumerable<string>> GetAllBranchMembers(string branchId);
    }
}