using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class StaffRepository : IStaffRepository
    {
        private readonly SqlConnection _connection;
        public StaffRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Staff>> GetAllStaffs(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Staffs WHERE IsActive = 1 AND BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            var staffs = new List<Staff>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var staff = new Staff
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(7)
                };
                staffs.Add(staff);
            }
            await reader.CloseAsync();
            return staffs;
        }
        public async Task<bool> AddStaffAccount(Staff staff, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Staffs (AccountId,Name,Salt,HashedPassword,IsActive,BranchId)" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@isActive,@branchId)";
            command.Parameters.AddWithValue("@accountId", staff.AccountId);
            command.Parameters.AddWithValue("@name", staff.Name);
            command.Parameters.AddWithValue("@salt", staff.Salt);
            command.Parameters.AddWithValue("@hasedPassword", staff.HashedPassword);
            command.Parameters.AddWithValue("@isActive", staff.IsActive);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
       
        public async Task<bool> UpdateStaffAccount(Staff staff, string branchId)
        {
            var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder("UPDATE Staffs SET ");

            if (staff.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", staff.Name);
            }

            if (staff.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", staff.Salt);

                if (staff.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", staff.HashedPassword);
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE AccountId = @accountId AND BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", staff.AccountId);
            command.Parameters.AddWithValue("@branchId", branchId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStaffAccount(string staffAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Staffs SET IsActive = 0 WHERE AccountId=@staffAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffAccountId", staffAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsStaffExist(string staffAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM Staffs WHERE AccountId=@staffAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffAccountId", staffAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<Staff> GetStaffById(string staffAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Staffs WHERE AccountId=@staffAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffAccountId", staffAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var staff = new Staff
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                await reader.CloseAsync();
                return staff;
            }
            else
            {
                return null;
            }
        }
        public async Task<Staff> GetStaffByName(string staffName, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Staffs WHERE AccountId=@staffName and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@staffName", staffName);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var staff = new Staff
                {
                    AccountId = reader["AccountId"].ToString(),
                    Name = reader["Name"].ToString(),
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                await reader.CloseAsync();
                return staff;
            }
            else
            {
                return null;
            }
        }
    }
}
