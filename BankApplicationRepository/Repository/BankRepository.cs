using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
{
    public class BankRepository : IBankRepository
    {
        private readonly SqlConnection _connection;

        public BankRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Banks WHERE IsActive = 1";
            var banks = new List<Bank>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var bank = new Bank
                {
                    BankName = reader["BankName"].ToString(),
                    BankId = reader["BankId"].ToString(),
                    IsActive = reader.GetBoolean(2)
                };
                banks.Add(bank);
            }
            await reader.CloseAsync();
            return banks;
        }

        public async Task<Bank?> GetBankById(string id)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Banks WHERE BankId = @id AND IsActive = 1";
            command.Parameters.AddWithValue("@id", id);
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var bank = new Bank
                {
                    BankName = reader["BankName"].ToString(),
                    BankId = reader["BankId"].ToString(),
                    IsActive = reader.GetBoolean(2)
                };
                await reader.CloseAsync();
                return bank;
            }
            else
            {
                return null;
            }
        }

        public async Task<Bank?> GetBankByName(string bankName)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Banks WHERE BankName = @bankName AND IsActive = 1";
            command.Parameters.AddWithValue("@bankName", bankName);
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var bank = new Bank
                {
                    BankName = reader["BankName"].ToString(),
                    BankId = reader["BankId"].ToString(),
                    IsActive = reader.GetBoolean(2)
                };
                await reader.CloseAsync();
                return bank;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsBankExist(string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT BankID FROM Banks WHERE BankID = @BankId";
            command.Parameters.AddWithValue("@BankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> AddBank(Bank bank)
        {

            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Banks (BankName, BankId, IsActive) VALUES (@bankName, @bankId, @isActive)";
            await _connection.OpenAsync();
            command.Parameters.AddWithValue("@bankName", bank.BankName);
            command.Parameters.AddWithValue("@bankId", bank.BankId);
            command.Parameters.AddWithValue("@isActive", bank.IsActive);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBank(Bank bank)
        {
            var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder("UPDATE Banks SET ");

            if (bank.BankName is not null)
            {
                queryBuilder.Append("BankName = @bankName, ");
                command.Parameters.AddWithValue("@bankName", bank.BankName);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE BankId = @bankId and IsActive = 1");
            command.Parameters.AddWithValue("@bankId", bank.BankId);

            command.CommandText = queryBuilder.ToString();

            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }


        public async Task<bool> DeleteBank(string id)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Banks SET IsActive=0 WHERE BankId=@id";
            command.Parameters.AddWithValue("@id", id);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies(string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Currencies WHERE IsActive = 1 and bankId = @bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            var Currency = new List<Currency>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var currency = new Currency
                {
                    CurrencyCode = reader["CurrencyCode"].ToString(),
                    ExchangeRate = (decimal)reader["ExchangeRate"],
                    IsActive = reader.GetBoolean(2)
                };
                Currency.Add(currency);
            }
            await reader.CloseAsync();
            return Currency;
        }
    }
}
