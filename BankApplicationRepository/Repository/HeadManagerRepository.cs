using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class HeadManagerRepository : IHeadManagerRepository
    {
        private readonly SqlConnection _connection;
        public HeadManagerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<HeadManager>> GetAllHeadManagers(string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM HeadManagers WHERE IsActive = 1 and BankId=@bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            var headManagers = new List<HeadManager>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var headManager = new HeadManager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(6)
                };
                headManagers.Add(headManager);
            }
            await reader.CloseAsync();
            return headManagers;
        }
        public async Task<bool> AddHeadManagerAccount(HeadManager headManager, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO HeadManagers (AccountId,Name,Salt,HashedPassword,IsActive,BankId)" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@bankId)";
            command.Parameters.AddWithValue("@accountId", headManager.AccountId);
            command.Parameters.AddWithValue("@name", headManager.Name);
            command.Parameters.AddWithValue("@salt", headManager.Salt);
            command.Parameters.AddWithValue("@hasedPassword", headManager.HashedPassword);
            command.Parameters.AddWithValue("@isActive", headManager.IsActive);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateHeadManagerAccount(HeadManager headManager, string bankId)
        {
            var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder("UPDATE HeadManagers SET ");

            if (headManager.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", headManager.Name);
            }

            if (headManager.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", headManager.Salt);

                if (headManager.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", headManager.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE AccountId = @accountId AND BankId = @bankId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", headManager.AccountId);
            command.Parameters.AddWithValue("@bankId", bankId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }


        public async Task<bool> DeleteHeadManagerAccount(string headManagerAccountId, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE HeadManagers SET IsActive = 0 WHERE AccountId=@headManagerAccountId and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@headManagerAccountId", headManagerAccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsHeadManagerExist(string headManagerAccountId, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM HeadManagers WHERE AccountId=@headManagerAccountId and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@headManagerAccountId", headManagerAccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<HeadManager> GetHeadManagerById(string headManagerAccountId, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM HeadManagers WHERE AccountId=@headManagerAccountId and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@headManagerAccountId", headManagerAccountId);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var headManager = new HeadManager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(6)
                };
                await reader.CloseAsync();
                return headManager;
            }
            else
            {
                return null;
            }
        }
        public async Task<HeadManager> GetHeadManagerByName(string headManagerName, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM HeadManagers WHERE Name=@headManagerName and BankId=@bankId AND IsActive = 1";
            command.Parameters.AddWithValue("@headManagerName", headManagerName);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var headManager = new HeadManager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(6)
                };
                await reader.CloseAsync();
                return headManager;
            }
            else
            {
                return null;
            }
        }
    }
}
