using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankApplicationServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IEncryptionService encryptionService, IUserRepository userRepository)
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
        }
        public async Task<Message> AuthenticateUser(string userName, string password)
        {
            Message message = new();
            IEnumerable<AuthenticateUser?> users = await _userRepository.GetAllUsersAuthenticationDetails();
            if (users.Any())
            {
                byte[] salt = new byte[32];
                AuthenticateUser? user = users.FirstOrDefault(u => u?.Name == userName);
                if (user is null)
                {
                    message.Result = false;
                    message.ResultMessage = "User Authentication Failed.";
                }
                else
                {
                    salt = user.Salt;
                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(password, salt);
                    bool isValidPassword = Convert.ToBase64String(user!.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                    if (isValidPassword)
                    {
                        string token = await GenerateTokenAsync(user.Name, user.Role);
                        message.Result = true;
                        message.ResultMessage = "User Authentication Successful.";
                        message.Data = token;
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "User Authentication Failed.";
                    }
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No Users Available";
            }
            return message;
        }

        public async Task<string> GenerateTokenAsync(string name, Roles role)
        {
            string mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
            SymmetricSecurityKey mySecurityKey = new(Encoding.UTF8.GetBytes(mySecret));

            JwtSecurityTokenHandler tokenHandler = new ();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                  new Claim("Name", name),
                  new Claim("Role", role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = await Task.Run(() => tokenHandler.CreateToken(tokenDescriptor));
            return tokenHandler.WriteToken(token);
        }
    }
}
