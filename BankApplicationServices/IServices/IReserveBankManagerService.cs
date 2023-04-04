using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IReserveBankManagerService
    {
        /// <summary>
        /// Authenticates the Reserve Bank Manager with the given username and password.
        /// </summary>
        /// <param name="userName">The username of the Reserve Bank Manager.</param>
        /// <param name="userPassword">The password of the Reserve Bank Manager.</param>
        /// <returns>A message indicating status of Authentication.</returns>
        Message AuthenticateReserveBankManager(string userName, string userPassword);
    }
}