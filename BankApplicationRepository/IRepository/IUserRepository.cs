using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<AuthenticateUser>> GetAllUsersAuthenticationDetails();
    }
}
