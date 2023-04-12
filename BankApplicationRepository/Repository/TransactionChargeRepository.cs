using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.Common;
using System.Data.SqlClient;

namespace BankApplicationRepository.Repository
{
    public class TransactionChargeRepository : ITransactionChargeRepository
    {
        private readonly SqlConnection _connection;
        public TransactionChargeRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<TransactionCharges> GetTransactionCharges(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM TransactionCharges WHERE  BranchId = @bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var transactionCharges = new TransactionCharges
                {
                    ImpsSameBank = (ushort)reader["ImpsSameBank"],
                    ImpsOtherBank = (ushort)reader["ImpsOtherBank"],
                    RtgsSameBank = (ushort)reader["ImpsOtherBank"],
                    RtgsOtherBank = (ushort)reader["RtgsOtherBank"],
                    IsActive = reader.GetBoolean(6)
                };
                await reader.CloseAsync();
                return transactionCharges;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> AddTransactionCharges(TransactionCharges transactionCharges, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO TransactionCharges (RtgsSameBank,RtgsOtherBank,ImpsSameBank,ImpsOtherBank,IsActive,BranchId)" +
                " VALUES (@rtgsSameBank, @rtgsOtherBank,@impsSameBank,@impsOtherBank,@isActive,@branchId)";
            command.Parameters.AddWithValue("@rtgsSameBank", transactionCharges.RtgsSameBank);
            command.Parameters.AddWithValue("@rtgsOtherBank", transactionCharges.RtgsOtherBank);
            command.Parameters.AddWithValue("@impsSameBank", transactionCharges.ImpsSameBank);
            command.Parameters.AddWithValue("@impsOtherBank", transactionCharges.ImpsOtherBank);
            command.Parameters.AddWithValue("@isActive", transactionCharges.IsActive);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateTransactionCharges(TransactionCharges transactionCharges, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE TransactionCharges SET RtgsSameBank=@rtgsSameBank,RtgsOtherBank=@rtgsOtherBank," +
                "ImpsSameBank=@impsSameBank,ImpsOtherBank=@impsOtherBank WHERE BranchId=@branchId AND IsActive = 1";
            command.Parameters.AddWithValue("@rtgsSameBank", transactionCharges.RtgsSameBank);
            command.Parameters.AddWithValue("@rtgsOtherBank", transactionCharges.RtgsOtherBank);
            command.Parameters.AddWithValue("@impsSameBank", transactionCharges.ImpsSameBank);
            command.Parameters.AddWithValue("@impsOtherBank", transactionCharges.ImpsOtherBank);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteTransactionCharges(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE TransactionCharges SET IsActive = 0 WHERE BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsTransactionChargesExist(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM TransactionCharges WHERE BranchId = @bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        
    }
}
