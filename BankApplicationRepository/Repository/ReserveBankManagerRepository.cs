using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class ReserveBankManagerRepository : IReserveBankManagerRepository
    {
        private readonly SqlConnection _connection;
        public ReserveBankManagerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagers()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM ReserveBankManagers WHERE IsActive = 1";
            var reserveBankManagers = new List<ReserveBankManager>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var reserveBankManager = new ReserveBankManager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(6)
                };
                reserveBankManagers.Add(reserveBankManager);
            }
            await reader.CloseAsync();
            return reserveBankManagers;
        }
        public async Task<bool> AddReserveBankManager(ReserveBankManager reserveBankManager)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO ReserveBankManagers (AccountId,Name,Salt,HashedPassword,IsActive)" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive)";
            command.Parameters.AddWithValue("@accountId", reserveBankManager.AccountId);
            command.Parameters.AddWithValue("@name", reserveBankManager.Name);
            command.Parameters.AddWithValue("@salt", reserveBankManager.Salt);
            command.Parameters.AddWithValue("@hasedPassword", reserveBankManager.HashedPassword);
            command.Parameters.AddWithValue("@isActive", reserveBankManager.IsActive);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
       
        public async Task<bool> UpdateReserveBankManager(ReserveBankManager reserveBankManager)
        {
            var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder("UPDATE ReserveBankManagers SET ");

            if (reserveBankManager.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", reserveBankManager.Name);
            }

            if (reserveBankManager.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", reserveBankManager.Salt);

                if (reserveBankManager.HashedPassword != null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", reserveBankManager.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE AccountId = @accountId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", reserveBankManager.AccountId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReserveBankManager(string reserveBankManagerAccountId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE ReserveBankManagers SET IsActive = 0 WHERE AccountId=@reserveBankManagerAccountId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@reserveBankManagerAccountId", reserveBankManagerAccountId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsReserveBankManagerExist(string reserveBankManagerAccountId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM ReserveBankManagers WHERE AccountId=@reserveBankManagerAccountId  AND IsActive = 1 ";
            command.Parameters.AddWithValue("@reserveBankManagerAccountId", reserveBankManagerAccountId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<ReserveBankManager> GetReserveBankManagerById(string reserveBankManagerAccountId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM ReserveBankManagers WHERE AccountId=@reserveBankManagerAccountId AND IsActive = 1";
            command.Parameters.AddWithValue("@reserveBankManagerAccountId", reserveBankManagerAccountId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var reserveBankManager = new ReserveBankManager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(6)
                };
                await reader.CloseAsync();
                return reserveBankManager;
            }
            else
            {
                return null;
            }
        }
        public async Task<ReserveBankManager> GetReserveBankManagerByName(string reserveBankManagerName)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM ReserveBankManagers WHERE Name=@reserveBankManagerName AND IsActive = 1";
            command.Parameters.AddWithValue("@reserveBankManagerName", reserveBankManagerName);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var reserveBankManager = new ReserveBankManager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(6)
                };
                await reader.CloseAsync();
                return reserveBankManager;
            }
            else
            {
                return null;
            }
        }
    }
}
