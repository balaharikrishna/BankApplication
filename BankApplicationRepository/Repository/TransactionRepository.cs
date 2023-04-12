using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;

namespace BankApplicationRepository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SqlConnection _connection;
        public TransactionRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactions(string fromCustomerAccountId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", fromCustomerAccountId);
            var transactions = new List<Transaction>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var transaction = new Transaction
                {
                    TransactionId = reader["TransactionId"].ToString(),
                    FromCustomerBankId = reader["FromCustomerBankId"].ToString(),
                    ToCustomerBankId = reader["ToCustomerBankId"].ToString(),
                    FromCustomerBranchId = reader["FromCustomerBranchId"].ToString(),
                    ToCustomerBranchId = reader["ToCustomerBranchId"].ToString(),
                    ToCustomerAccountId = reader["ToCustomerAccountId"].ToString(),
                    TransactionType = (TransactionType)reader["TransactionType"],
                    TransactionDate = reader["TransactionDate"].ToString(),
                    Debit = (decimal)reader["Debit"],
                    Credit = (decimal)reader["Credit"],
                    Balance = (decimal)reader["Balance"],
                    FromCustomerAccountId = reader["FromCustomerAccountId"].ToString(),
                };
                transactions.Add(transaction);
            }
            await reader.CloseAsync();
            return transactions;
        }
        public async Task<Transaction> GetTransactionById(string fromCustomerAccountId, string transactionId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId and TransactionId=@TransactionId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@fromCustomerAccountId", fromCustomerAccountId);
            command.Parameters.AddWithValue("@transactionId", transactionId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var transaction = new Transaction
                {
                    TransactionId = reader["TransactionId"].ToString(),
                    FromCustomerBankId = reader["FromCustomerBankId"].ToString(),
                    ToCustomerBankId = reader["ToCustomerBankId"].ToString(),
                    FromCustomerBranchId = reader["FromCustomerBranchId"].ToString(),
                    ToCustomerBranchId = reader["ToCustomerBranchId"].ToString(),
                    ToCustomerAccountId = reader["ToCustomerAccountId"].ToString(),
                    TransactionType = (TransactionType)reader["TransactionType"],
                    TransactionDate = reader["TransactionDate"].ToString(),
                    Debit = (decimal)reader["Debit"],
                    Credit = (decimal)reader["Credit"],
                    Balance = (decimal)reader["Balance"],
                    FromCustomerAccountId = reader["FromCustomerAccountId"].ToString(),
                };
                await reader.CloseAsync();
                return transaction;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> IsTransactionsExist(string fromCustomerAccountId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", fromCustomerAccountId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> AddTransaction(Transaction transaction)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Transactions (TransactionId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId" +
                "ToCustomerAccountId,TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId)" +
                " VALUES (@transactionId, @fromCustomerBankId,@toCustomerBankId,@fromCustomerBranchId,@toCustomerBranchId,@toCustomerAccountId," +
                "@transactionType, @transactionDate,@debit,@credit,@balance,@fromCustomerAccountId)";
            command.Parameters.AddWithValue("@transactionId", transaction.TransactionId);
            command.Parameters.AddWithValue("@fromCustomerBankId", transaction.FromCustomerBankId);
            command.Parameters.AddWithValue("@toCustomerBankId", transaction.ToCustomerBankId);
            command.Parameters.AddWithValue("@fromCustomerBranchId", transaction.FromCustomerBranchId);
            command.Parameters.AddWithValue("@toCustomerBranchId", transaction.ToCustomerBranchId);
            command.Parameters.AddWithValue("@toCustomerAccountId", transaction.ToCustomerAccountId);
            command.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("@transactionDate", transaction.TransactionDate);
            command.Parameters.AddWithValue("@debit", transaction.Debit);
            command.Parameters.AddWithValue("@credit", transaction.Credit);
            command.Parameters.AddWithValue("@balance", transaction.Balance);
            command.Parameters.AddWithValue("@fromCustomerAccountId", transaction.FromCustomerAccountId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}
