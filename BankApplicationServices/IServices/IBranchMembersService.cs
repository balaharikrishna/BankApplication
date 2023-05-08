namespace BankApplication.Services.IServices
{
    public interface IBranchMembersService
    {
        Task<IEnumerable<string>> GetAllBranchesAsync(string branchId);
    }
}