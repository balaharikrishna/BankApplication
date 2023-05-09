namespace BankApplication.Services.IServices
{
    public interface IBranchMembersService
    {
        /// <summary>
        /// Gets all Names of Branch Memebers by branchId.
        /// </summary>
        /// <param name="branchId">Branch Id</param>
        /// <returns>Returns all Names of Branch Memebers</returns>
        Task<IEnumerable<string>> GetAllBranchesAsync(string branchId);
    }
}