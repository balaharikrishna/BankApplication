using BankApplication.Models;

namespace BankApplication.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<AuthenticateUser?> GetUserAuthenticationDetails(string accountId);
    }
}
