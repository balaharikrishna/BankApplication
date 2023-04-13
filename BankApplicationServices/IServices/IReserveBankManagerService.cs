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
        Task<Message> AuthenticateManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerPassword);
        Task<Message> IsReserveBankManagersExistAsync();
        Task<Message> OpenReserveBankManagerAccountAsync(string ReserveBankManagerName, string ReserveBankManagerPassword);
        Task<Message> UpdateReserveBankManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerName, string ReserveBankManagerPassword);
        Task<Message> DeleteReserveBankManagerAccountAsync(string ReserveBankManagerAccountId);
        Task<Message> IsAccountExistAsync(string ReserveBankManagerAccountId);
        Task<ReserveBankManager> GetReserveBankManagerDetailsAsync(string ReserveBankManagerAccountId);
    }
}