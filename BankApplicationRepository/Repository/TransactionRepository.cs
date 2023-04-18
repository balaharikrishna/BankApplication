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
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId,ToCustomerAccountId," +
                "TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", fromCustomerAccountId);
            List<Transaction> transactions = new();
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Transaction transaction = new()
                {
                    TransactionId = reader[0].ToString(),
                    FromCustomerBankId = reader[1].ToString(),
                    ToCustomerBankId = reader[2].ToString(),
                    FromCustomerBranchId = reader[3].ToString(),
                    ToCustomerBranchId = reader[4].ToString(),
                    ToCustomerAccountId = reader[5].ToString(),
                    TransactionType = (TransactionType)reader[6],
                    TransactionDate = reader[7].ToString(),
                    Debit = (decimal)reader[8],
                    Credit = (decimal)reader[9],
                    Balance = (decimal)reader[10],
                    FromCustomerAccountId = reader[11].ToString(),
                };
                transactions.Add(transaction);
            }
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return transactions;
        }
        public async Task<Transaction?> GetTransactionById(string fromCustomerAccountId, string transactionId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId,ToCustomerAccountId," +
                "TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId" +
                " and TransactionId=@TransactionId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@fromCustomerAccountId", fromCustomerAccountId);
            command.Parameters.AddWithValue("@transactionId", transactionId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Transaction transaction = new()
                {
                    TransactionId = reader[0].ToString(),
                    FromCustomerBankId = reader[1].ToString(),
                    ToCustomerBankId = reader[2].ToString(),
                    FromCustomerBranchId = reader[3].ToString(),
                    ToCustomerBranchId = reader[4].ToString(),
                    ToCustomerAccountId = reader[5].ToString(),
                    TransactionType = (TransactionType)reader[6],
                    TransactionDate = reader[7].ToString(),
                    Debit = (decimal)reader[8],
                    Credit = (decimal)reader[9],
                    Balance = (decimal)reader[10],
                    FromCustomerAccountId = reader[11].ToString(),
                };
                await reader.CloseAsync();
                await _connection.CloseAsync();
                return transaction;
            }
            else
            {
                await _connection.CloseAsync();
                return null;
            }
        }
        public async Task<bool> IsTransactionsExist(string fromCustomerAccountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", fromCustomerAccountId);
            await _connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            bool isTransactionsExist = reader.HasRows;
            await reader.CloseAsync();
            await _connection.CloseAsync();
            return isTransactionsExist;
        }
        public async Task<bool> AddTransaction(Transaction transaction)
        {
            SqlCommand command = _connection.CreateCommand();
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
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected > 0;
        }
    }
}
