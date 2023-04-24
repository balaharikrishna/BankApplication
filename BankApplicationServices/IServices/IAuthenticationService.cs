using BankApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationServices.IServices
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// authenticates the given userName and password.
        /// </summary>
        /// <param name="userName">user Name of User.</param>
        /// <param name="password">password of User.</param>
        /// <returns>A Message object containing information about the success or failure of the operation.</returns>
        Task<Message> AuthenticateUser(string userName, string password);
    }
}
