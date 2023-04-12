using BankApplicationModels;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;

namespace BankApplicationRepository.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly SqlConnection _connection;
        public CurrencyRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrency(string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Currencies WHERE IsActive = 1 AND BankId=@bankId";
            command.Parameters.AddWithValue("@bankId", bankId);
            var currencies = new List<Currency>();
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
                currencies.Add(currency);
            }
            await reader.CloseAsync();
            return currencies;
        }
        public async Task<bool> AddCurrency(Currency currency, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Currencies (CurrencyCode,ExchangeRate,IsActive,BankId)" +
                " VALUES (@currencyCode, @exchangeRate, @isActive,@bankId)";
            command.Parameters.AddWithValue("@currencyCode", currency.CurrencyCode);
            command.Parameters.AddWithValue("@exchangeRate", currency.ExchangeRate);
            command.Parameters.AddWithValue("@isActive", currency.IsActive);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateCurrency(Currency currency, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Currencies SET CurrencyCode=@currencyCode,ExchangeRate=@exchangeRate WHERE BankId=@bankId AND IsActive = 1";
            command.Parameters.AddWithValue("@currencyCode", currency.CurrencyCode);
            command.Parameters.AddWithValue("@exchangeRate", currency.ExchangeRate);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteCurrency(string currencyCode, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Currencies SET IsActive = 0 WHERE CurrencyCode=@currencyCode and BankId=@bankId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@currencyCode", currencyCode);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> IsCurrencyExist(string currencyCode, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT CurrencyCode FROM Currencies WHERE BankId = @bankId and CurrencyCode=@currencyCode AND IsActive = 1 ";
            command.Parameters.AddWithValue("@currencyCode", currencyCode);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<Currency> GetCurrencyByCode(string currencyCode, string bankId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Currencies WHERE CurrencyCode = @currencyCode AND IsActive = 1 AND BankId=@bankId";
            command.Parameters.AddWithValue("@currencyCode", currencyCode);
            command.Parameters.AddWithValue("@bankId", bankId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var currency = new Currency
                {
                    CurrencyCode = reader["CurrencyCode"].ToString(),
                    ExchangeRate = (decimal)reader["ExchangeRate"],
                    IsActive = reader.GetBoolean(2)
                };
                await reader.CloseAsync();
                return currency;
            }
            else
            {
                return null;
            }
        }
    }
}
