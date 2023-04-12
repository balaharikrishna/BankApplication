using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace BankApplicationRepository.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlConnection _connection;
        public CustomerRepository(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Customer>> GetAllCustomers(string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Customers WHERE IsActive = 1 AND BranchId=@branchId";
            command.Parameters.AddWithValue("@branchId", branchId);
            var customers = new List<Customer>();
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    AccountId = reader["AccountId"].ToString(),
                    AccountType = (AccountType)reader["AccountType"],
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    EmailId = reader["EmailId"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    DateOfBirth = reader["DateOfBirth"].ToString(),
                    Gender = (Gender)reader["Gender"],
                    PassbookIssueDate = reader["PassbookIssueDate"].ToString(),
                    Balance = (decimal)reader["Balance"],
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                customers.Add(customer);
            }
            await reader.CloseAsync();
            return customers;
        }
        public async Task<bool> AddCustomerAccount(Customer customer, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Customers (AccountId,Name,Salt,HashedPassword,Balance,Gender," +
                "AccountType,Address,DateOfBirth,EmailId,PhoneNumber,PassbookIssueDate,IsActive,BranchId)" +
                " VALUES (@accountId, @name, @salt,@hasedPassword,@balance,@gender,@accountType,@address,@dateOfBirth" +
                "@emailId,@phoneNumber,@passbookIssueDate,@isActive,@branchId)";
            command.Parameters.AddWithValue("@accountId", customer.AccountId);
            command.Parameters.AddWithValue("@name", customer.Name);
            command.Parameters.AddWithValue("@salt", customer.Salt);
            command.Parameters.AddWithValue("@hasedPassword", customer.HashedPassword);
            command.Parameters.AddWithValue("@balance", customer.Balance);
            command.Parameters.AddWithValue("@gender", customer.Gender);
            command.Parameters.AddWithValue("@accountType", customer.AccountType);
            command.Parameters.AddWithValue("@address", customer.Address);
            command.Parameters.AddWithValue("@dateOfBirth", customer.DateOfBirth);
            command.Parameters.AddWithValue("@emailId", customer.EmailId);
            command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            command.Parameters.AddWithValue("@passbookIssueDate", customer.PassbookIssueDate);
            command.Parameters.AddWithValue("@isActive", customer.IsActive);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateCustomerAccount(Customer customer, string branchId)
        {
            var command = _connection.CreateCommand();
            var text = "UPDATE Customers SET ";
            if(!string.IsNullOrEmpty(customer.Name))
            {
                command.Parameters.AddWithValue("@name", customer.Name);
                text.Concat("Name=@name,");
            }
            text.Concat(" WHERE AccountId=@accountId And BranchId=@branchId AND IsActive = 1");
            command.CommandText = "UPDATE Customers SET Name=@name,Salt=@salt,HashedPassword=@hasedPassword,Balance=@balance,Gender=@gender," +
                "AccountType=@accountType,Address=@address,DateOfBirth=@dateOfBirth,EmailId=@emailId,PhoneNumber=@phoneNumber," +
                "PassbookIssueDate=@passbookIssueDate WHERE AccountId=@accountId And BranchId=@branchId AND IsActive = 1";
            command.Parameters.AddWithValue("@accountId", customer.AccountId);
            
            command.Parameters.AddWithValue("@salt", customer.Salt);
            command.Parameters.AddWithValue("@hasedPassword", customer.HashedPassword);
            command.Parameters.AddWithValue("@balance", customer.Balance);
            command.Parameters.AddWithValue("@gender", customer.Gender);
            command.Parameters.AddWithValue("@accounType", customer.AccountType);
            command.Parameters.AddWithValue("@address", customer.Address);
            command.Parameters.AddWithValue("@dateOfBirth", customer.DateOfBirth);
            command.Parameters.AddWithValue("@emailId", customer.EmailId);
            command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            command.Parameters.AddWithValue("@passbookIssueDate", customer.PassbookIssueDate);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteCustomerAccount(string customerAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE Customers SET IsActive = 0 WHERE AccountId=@customerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerAccountId", customerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsCustomerExist(string customerAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT AccountId FROM Customers WHERE AccountId=@customerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerAccountId", customerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<Customer?> GetCustomerById(string customerAccountId, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Customers WHERE AccountId=@customerAccountId and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerAccountId", customerAccountId);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    AccountId = reader["AccountId"].ToString(),
                    AccountType = (AccountType)reader["AccountType"],
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    EmailId = reader["EmailId"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    DateOfBirth = reader["DateOfBirth"].ToString(),
                    Gender = (Gender)reader["Gender"],
                    PassbookIssueDate = reader["PassbookIssueDate"].ToString(),
                    Balance = (decimal)reader["Balance"],
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                await reader.CloseAsync();
                return customer;
            }
            else
            {
                return null;
            }
        }
        public async Task<Customer?> GetCustomerByName(string customerName, string branchId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM Customers WHERE Name=@customerName and BranchId=@branchId AND IsActive = 1 ";
            command.Parameters.AddWithValue("@customerName", customerName);
            command.Parameters.AddWithValue("@branchId", branchId);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    AccountId = reader["AccountId"].ToString(),
                    AccountType = (AccountType)reader["AccountType"],
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    EmailId = reader["EmailId"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    DateOfBirth = reader["DateOfBirth"].ToString(),
                    Gender = (Gender)reader["Gender"],
                    PassbookIssueDate = reader["PassbookIssueDate"].ToString(),
                    Balance = (decimal)reader["Balance"],
                    Salt = (byte[])reader["Salt"],
                    HashedPassword = (byte[])reader["HashedPassword"],
                    IsActive = reader.GetBoolean(14)
                };
                await reader.CloseAsync();
                return customer;
            }
            else
            {
                return null;
            }
        }
    }
}
