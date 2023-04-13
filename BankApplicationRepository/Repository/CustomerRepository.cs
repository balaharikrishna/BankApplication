using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using System.Data.SqlClient;
using System.Text;

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
            var queryBuilder = new StringBuilder("UPDATE Customers SET ");

            if (customer.Name is not null)
            {
                queryBuilder.Append("Name = @name, ");
                command.Parameters.AddWithValue("@name", customer.Name);
            }

            if (customer.Salt is not null)
            {
                queryBuilder.Append("Salt = @salt, ");
                command.Parameters.AddWithValue("@salt", customer.Salt);

                if (customer.HashedPassword is not null)
                {
                    queryBuilder.Append("HashedPassword = @hashedPassword, ");
                    command.Parameters.AddWithValue("@hashedPassword", customer.HashedPassword);
                }
            }

            if (customer.Balance >= 0)
            {
                queryBuilder.Append("Balance = @balance, ");
                command.Parameters.AddWithValue("@balance", customer.Balance);
            }

            if (Enum.IsDefined(typeof(Gender), customer.Gender))
            {
                queryBuilder.Append("Gender = @gender, ");
                command.Parameters.AddWithValue("@gender", customer.Gender);
            }

            if (Enum.IsDefined(typeof(AccountType), customer.AccountType))
            {
                queryBuilder.Append("AccountType = @accountType, ");
                command.Parameters.AddWithValue("@accountType", customer.AccountType);
            }

            if (customer.Address is not null)
            {
                queryBuilder.Append("Address = @address, ");
                command.Parameters.AddWithValue("@address", customer.Address);
            }

            if (customer.DateOfBirth is not null)
            {
                queryBuilder.Append("DateOfBirth = @dateOfBirth, ");
                command.Parameters.AddWithValue("@dateOfBirth", customer.DateOfBirth);
            }

            if (customer.EmailId is not null)
            {
                queryBuilder.Append("EmailId = @emailId, ");
                command.Parameters.AddWithValue("@emailId", customer.EmailId);
            }

            if (customer.PhoneNumber is not null)
            {
                queryBuilder.Append("PhoneNumber = @phoneNumber, ");
                command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            }

            if (customer.PassbookIssueDate is not null)
            {
                queryBuilder.Append("PassbookIssueDate = @passbookIssueDate, ");
                command.Parameters.AddWithValue("@passbookIssueDate", customer.PassbookIssueDate);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);

            queryBuilder.Append(" WHERE AccountId = @accountId AND BranchId = @branchId AND IsActive = 1");
            command.Parameters.AddWithValue("@accountId", customer.AccountId);
            command.Parameters.AddWithValue("@branchId", branchId);

            command.CommandText = queryBuilder.ToString();

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
