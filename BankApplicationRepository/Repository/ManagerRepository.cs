using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly SqlConnection _connection;
        public ManagerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Manager>> GetAllManagers(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Managers WHERE IsActive = 1 AND BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            var managers = new List<Manager>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var manager = new Manager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                managers.Add(manager);
            }
            await reader.CloseAsync();
            return managers;
        }
        public async Task<bool> AddManagerAccount(Manager manager, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Managers (AccountId,Name,Salt,HashedPassword,IsActive,BranchId)" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@branchId)";
            command.Parameters.AddWithValue("@accountId", manager.AccountId);
            command.Parameters.AddWithValue("@name", manager.Name);
            command.Parameters.AddWithValue("@salt", manager.Salt);
            command.Parameters.AddWithValue("@hasedPassword", manager.HashedPassword);
            command.Parameters.AddWithValue("@isActive", manager.IsActive);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateManagerAccount(Manager manager, string branchId)
        {
            var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder("UPDATE Managers SET ");

            if (manager.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", manager.Name);
            }

            if (manager.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", manager.Salt);

                if (manager.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", manager.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE AccountId = @accountId AND BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", manager.AccountId);
            command.Parameters.AddWithValue("@branchId", branchId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteManagerAccount(string managerAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Managers SET IsActive = 0 WHERE AccountId=@managerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerAccountId", managerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsManagerExist(string managerAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM Managers WHERE AccountId=@managerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerAccountId", managerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<Manager> GetManagerById(string managerAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Managers WHERE AccountId=@managerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerAccountId", managerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var manager = new Manager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                await reader.CloseAsync();
                return manager;
            }
            else
            {
                return null;
            }
        }
        public async Task<Manager> GetManagerByName(string managerName, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Managers WHERE Name=@managerName and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@managerName", managerName);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var manager = new Manager
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                await reader.CloseAsync();
                return manager;
            }
            else
            {
                return null;
            }
        }

    }
}
