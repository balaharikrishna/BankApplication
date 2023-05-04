using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class BranchMembersService : IBranchMembersService
    {
        private readonly IBranchMembersRepository _branchMembersRepository;
        public BranchMembersService(IBranchMembersRepository branchMembersRepository)
        {
            _branchMembersRepository = branchMembersRepository;
        }

        public async Task<IEnumerable<string>> GetAllBranchesAsync(string branchId)
        {
            return await _branchMembersRepository.GetAllBranchMembers(branchId);
        }
    }
}
