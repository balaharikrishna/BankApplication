using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class TransactionChargeRepository : ITransactionChargeRepository
    {
        private readonly SqlConnection _connection;
        public TransactionChargeRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<TransactionCharges?> GetTransactionCharges(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT ImpsSameBank,ImpsOtherBank,RtgsSameBank,RtgsOtherBank,IsActive FROM TransactionCharges WHERE  BranchId = @bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                TransactionCharges transactionCharges = new()
                {
                    ImpsSameBank = (ushort)reader[0],
                    ImpsOtherBank = (ushort)reader[1],
                    RtgsSameBank = (ushort)reader[2],
                    RtgsOtherBank = (ushort)reader[3],
                    IsActive = reader.GetBoolean(4)
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return transactionCharges;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<bool> AddTransactionCharges(TransactionCharges transactionCharges, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO TransactionCharges (RtgsSameBank,RtgsOtherBank,ImpsSameBank,ImpsOtherBank,IsActive,BranchId)" +
                " VALUES (@rtgsSameBank, @rtgsOtherBank,@impsSameBank,@impsOtherBank,@isActive,@branchId)";
            command.Parameters.AddWithValue("@rtgsSameBank", transactionCharges.RtgsSameBank);
            command.Parameters.AddWithValue("@rtgsOtherBank", transactionCharges.RtgsOtherBank);
            command.Parameters.AddWithValue("@impsSameBank", transactionCharges.ImpsSameBank);
            command.Parameters.AddWithValue("@impsOtherBank", transactionCharges.ImpsOtherBank);
            command.Parameters.AddWithValue("@isActive", transactionCharges.IsActive);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateTransactionCharges(TransactionCharges transactionCharges, string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("UPDATE TransactionCharges SET ");

            if (transactionCharges.RtgsSameBank >= 0 && transactionCharges.RtgsSameBank <= 100)
            {
                queryBuilder.Append("RtgsSameBank = @rtgsSameBank, ");
                command.Parameters.AddWithValue("@rtgsSameBank", transactionCharges.RtgsSameBank);
            }

            if (transactionCharges.RtgsOtherBank >= 0 && transactionCharges.RtgsOtherBank <= 100)
            {
                queryBuilder.Append("RtgsOtherBank = @rtgsOtherBank, ");
                command.Parameters.AddWithValue("@rtgsOtherBank", transactionCharges.RtgsOtherBank);
            }

            if (transactionCharges.ImpsSameBank >= 0 && transactionCharges.ImpsSameBank <= 100)
            {
                queryBuilder.Append("ImpsSameBank = @impsSameBank, ");
                command.Parameters.AddWithValue("@impsSameBank", transactionCharges.ImpsSameBank);
            }

            if (transactionCharges.ImpsOtherBank >= 0 && transactionCharges.ImpsOtherBank <= 100)
            {
                queryBuilder.Append("ImpsOtherBank = @impsOtherBank, ");
                command.Parameters.AddWithValue("@impsOtherBank", transactionCharges.ImpsOtherBank);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            queryBuilder.Append(" WHERE BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@branchId", branchId);
            command.CommandText = queryBuilder.ToString();
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTransactionCharges(string branchId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "UPDATE TransactionCharges SET IsActive = 0 WHERE BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsTransactionChargesExist(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM TransactionCharges WHERE BranchId = @bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isTransactionChargesExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isTransactionChargesExist;
        }
    }
}
