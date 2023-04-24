using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;

namespace BankApplicationRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;
        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<AuthenticateUser>> GetAllUsersAuthenticationDetails()
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT Name,Role,Salt,HashedPassword, FROM ReserveBankManagers where IsActive = 1 UNION ALL " +
                                  "SELECT Name, Role, Salt, HashedPassword FROM HeadManagers where IsActive = 1 UNION ALL " +
                                  "SELECT Name, Role, Salt, HashedPassword FROM Managers where IsActive = 1 UNION ALL " +
                                  "SELECT Name, Role, Salt, HashedPassword FROM Staffs where IsActive = 1 UNION ALL " +
                                  "SELECT Name, Role, Salt, HashedPassword FROM Customers where IsActive = 1 " +
                                  "ORDER BY Name,Role,Salt,HashedPassword where ";

            List<AuthenticateUser> users = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                AuthenticateUser user = new()
                {
                    Name = reader[0].ToString(),
                    Role = (Roles)Convert.ToUInt16(reader[1]),
                    Salt = (byte[])reader[2],
                    HashedPassword = (byte[])reader[3]
                };
                users.Add(user);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            if (users.Count > 0)
            {
                return users;
            }
            else
            {
                return null;
            }
        }
    }
}
