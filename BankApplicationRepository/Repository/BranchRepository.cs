using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly SqlConnection _connection;
        public BranchRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Branch>> GetAllBranches(string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Branches WHERE IsActive = 1 and BankId=@bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            var branches = new List<Branch>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var branch = new Branch
                {
                    BranchName = reader["BranchName"].ToString(),
                    BranchId = reader["BranchId"].ToString(),
                    BranchAddress = reader["BranchAddress"].ToString(),
                    BranchPhoneNumber = reader["BranchPhoneNumber"].ToString(),
                    IsActive = reader.GetBoolean(5)
                };
                branches.Add(branch);
            }
            await reader.CloseAsync();
            return branches;
        }

        public async Task<Branch> GetBranchById(string branchId)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Branches WHERE BranchId = @branchId AND IsActive = 1";
            command.Parameters.AddWithValue("@branchId", branchId);
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var branch = new Branch
                {
                    BranchName = reader["BranchName"].ToString(),
                    BranchId = reader["BranchId"].ToString(),
                    BranchAddress = reader["BranchAddress"].ToString(),
                    BranchPhoneNumber = reader["BranchPhoneNumber"].ToString(),
                    IsActive = reader.GetBoolean(5)
                };
                await reader.CloseAsync();
                return branch;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsBranchExist(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT BranchId FROM Branches WHERE BranchId = @branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> AddBranch(Branch branch,string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Branches (BranchName, BranchId, IsActive,BranchAddress,BranchPhoneNumber,BankId)" +
                " VALUES (@branchName, @branchId, @isActive,@branchAddress,@branchPhoneNumber,@bankId)";
            command.Parameters.AddWithValue("@branchName", branch.BranchName);
            command.Parameters.AddWithValue("@branchId", branch.BranchId);
            command.Parameters.AddWithValue("@isActive", branch.IsActive);
            command.Parameters.AddWithValue("@branchAddress", branch.BranchAddress);
            command.Parameters.AddWithValue("@branchPhoneNumber", branch.BranchPhoneNumber);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBranch(Branch branch)
        {
            var command = _connection.CreateCommand();
            var query = new StringBuilder("UPDATE Branches SET ");

            if (!string.IsNullOrEmpty(branch.BranchName))
            {
                query.Append("BranchName=@branchName,");
                command.Parameters.AddWithValue("@branchName", branch.BranchName);
            }

            if (!string.IsNullOrEmpty(branch.BranchAddress))
            {
                query.Append("BranchAddress=@branchAddress,");
                command.Parameters.AddWithValue("@branchAddress", branch.BranchAddress);
            }

            if (!string.IsNullOrEmpty(branch.BranchPhoneNumber))
            {
                query.Append("BranchPhoneNumber=@branchPhoneNumber,");
                command.Parameters.AddWithValue("@branchPhoneNumber", branch.BranchPhoneNumber);
            }

            query.Remove(query.Length - 1, 1); 
            query.Append(" WHERE BranchId=@branchId and IsActive=1");
            command.Parameters.AddWithValue("@branchId", branch.BranchId);

            command.CommandText = query.ToString();
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBranch(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Branches SET IsActive = 0 WHERE BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<Branch?> GetBranchByName(string branchName)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Branches WHERE BranchName = @branchName AND IsActive = 1";
            command.Parameters.AddWithValue("@branchName", branchName);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var branch = new Branch
                {
                    BranchName = reader["BranchName"].ToString(),
                    BranchId = reader["BranchId"].ToString(),
                    BranchAddress = reader["BranchAddress"].ToString(),
                    BranchPhoneNumber = reader["BranchPhoneNumber"].ToString(),
                    IsActive = reader.GetBoolean(5)
                };
                await reader.CloseAsync();
                return branch;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<TransactionCharges>> GetAllTransactionCharges(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM TransactionCharges WHERE IsActive = 1 and BranchId = @branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            var transactionChargesList = new List<TransactionCharges>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var transactionCharges = new TransactionCharges
                {
                    RtgsOtherBank = (ushort)reader["RtgsOtherBank"],
                    RtgsSameBank  = (ushort)reader["RtgsSameBank"],
                    ImpsOtherBank = (ushort)reader["ImpsOtherBank"],
                    ImpsSameBank  = (ushort)reader["ImpsSameBank"],
                    IsActive = reader.GetBoolean(5)
                };
                transactionChargesList.Add(transactionCharges);
            }
            await reader.CloseAsync();
            return transactionChargesList;
        }
    }
}
