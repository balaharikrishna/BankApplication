using BankApplication.Repository.IRepository;
using BankApplication.Services.IServices;

namespace BankApplication.Services.Services
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
            IEnumerable<string>? members = await _branchMembersRepository.GetAllBranchMembers(branchId);
            if (members.Any())
            {
                return members;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
