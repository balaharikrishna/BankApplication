using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SqlConnection _connection;
        public TransactionRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactions(string accountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId,ToCustomerAccountId," +
                "TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", accountId);
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
                    TransactionType = (TransactionType)Convert.ToUInt16(reader[6]),
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
        public async Task<Transaction?> GetTransactionById(string accountId, string transactionId, string location = "from")
        {
            SqlCommand command = _connection.CreateCommand();
            StringBuilder queryBuilder = new("SELECT TransactionId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId,ToCustomerAccountId," +
                "TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId FROM Transactions WHERE TransactionId=@TransactionId and ");

            if (location.Equals("from"))
            {
                queryBuilder.Append("FromCustomerAccountId=@accountId");
                command.Parameters.AddWithValue("@accountId", accountId);
            }
            else
            {
                queryBuilder.Append("ToCustomerAccountId=@accountId");
                command.Parameters.AddWithValue("@accountId", accountId);
            }

            command.Parameters.AddWithValue("@transactionId", transactionId);
            command.CommandText = queryBuilder.ToString();
            await _connection.OpenAsync();
            try
            {
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
                        TransactionType = (TransactionType)Convert.ToUInt16(reader[6]),
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
            }catch(Exception ex)
            {

            }
            return null;
        }
        public async Task<bool> IsTransactionsExist(string accountId)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT TransactionId FROM Transactions WHERE FromCustomerAccountId=@fromCustomerAccountId";
            command.Parameters.AddWithValue("@fromCustomerAccountId", accountId);
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
            command.CommandText = "INSERT INTO Transactions (TransactionId,FromCustomerBankId,ToCustomerBankId,FromCustomerBranchId,ToCustomerBranchId," +
                "ToCustomerAccountId,TransactionType,TransactionDate,Debit,Credit,Balance,FromCustomerAccountId)" +
                " VALUES (@transactionId,@fromCustomerBankId,@toCustomerBankId,@fromCustomerBranchId,@toCustomerBranchId,@toCustomerAccountId," +
                "@transactionType, @transactionDate,@debit,@credit,@balance,@fromCustomerAccountId)";
            command.Parameters.AddWithValue("@transactionId", transaction.TransactionId);
            command.Parameters.AddWithValue("@fromCustomerBankId", transaction.FromCustomerBankId);
            command.Parameters.AddWithValue("@toCustomerBankId", (object)transaction.ToCustomerBankId ?? DBNull.Value);
            command.Parameters.AddWithValue("@fromCustomerBranchId", transaction.FromCustomerBranchId);
            command.Parameters.AddWithValue("@toCustomerBranchId", (object)transaction.ToCustomerBranchId ?? DBNull.Value);
            command.Parameters.AddWithValue("@toCustomerAccountId", (object)transaction.ToCustomerAccountId ?? DBNull.Value);
            command.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("@transactionDate", transaction.TransactionDate);
            command.Parameters.AddWithValue("@debit", transaction.Debit);
            command.Parameters.AddWithValue("@credit", transaction.Credit);
            command.Parameters.AddWithValue("@balance", transaction.Balance);
            command.Parameters.AddWithValue("@fromCustomerAccountId", transaction.FromCustomerAccountId);
            await _connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return rowsAffected>0;
        }
    }
}
