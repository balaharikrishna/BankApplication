using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IFileService _fileService;
        private readonly IEncryptionService _encryptionService;
        private readonly IBranchService _branchService;
        private readonly ITransactionService _transactionService;
        private List<Bank> banks;
        public CustomerService(IFileService fileService, IEncryptionService encryptionService,
            IBranchService branchService, ITransactionService transactionService)
        {
            _fileService = fileService;
            _encryptionService = encryptionService;
            _branchService = branchService;
            _transactionService = transactionService;
            banks = new List<Bank>();
        }

        public Message IsCustomersExist(string bankId, string branchId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is null)
                        {
                            customers = new List<Customer>();
                            customers.FindAll(c => c.IsActive == 1);
                        }
                        if (customers is not null && customers.Count > 0)
                        {
                            message.Result = true;
                            message.ResultMessage = "Customers Exist in Branch";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Customers Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }

            return message;
        }
        public Message AuthenticateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is not null)
                        {
                            byte[] salt = new byte[32];
                            var customer = customers.Find(m => m.AccountId.Equals(customerAccountId));
                            if (customer is not null)
                            {
                                salt = customer.Salt;
                            }

                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(customerPassword, salt);
                            bool isCustomerAvilable = customers.Any(m => m.AccountId.Equals(customerAccountId) && Convert.ToBase64String(m.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)) && m.IsActive == 1);
                            if (isCustomerAvilable)
                            {
                                message.Result = true;
                                message.ResultMessage = "Customer Validation Successful.";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Customer Validation Failed.";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Customers Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }

            return message;
        }

        public Message IsAccountExist(string bankId, string branchId, string customerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is not null)
                        {
                            bool isCustomerAvilable = customers.Any(m => m.AccountId.Equals(customerAccountId) && m.IsActive == 1);
                            if (isCustomerAvilable)
                            {
                                message.Result = true;
                                message.ResultMessage = "Customer Validation Successful.";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Customer Validation Failed.";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Customers Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }

            return message;
        }

        public Message OpenCustomerAccount(string bankId, string branchId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
          string customerDateOfBirth, int customerGender)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Customer>? customers = null;
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    customers = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Customers;
                }

                customers ??= new List<Customer>();

                bool isCustomerAlreadyAvailabe = customers.Any(m => m.Name.Equals(customerName) && m.IsActive == 1); ;
                if (!isCustomerAlreadyAvailabe)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = customerName.Substring(0, 3);
                    string customerAccountId = UserFirstThreeCharecters + date;

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(customerPassword, salt);

                    Customer customer = new()
                    {
                        Name = customerName,
                        AccountType = (AccountType)customerAccountType,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = customerAccountId,
                        PassbookIssueDate = date,
                        Address = customerAddress,
                        DateOfBirth = customerDateOfBirth,
                        Gender = (Gender)customerGender,
                        EmailId = customerEmailId,
                        PhoneNumber = customerPhoneNumber,
                        IsActive = 1
                    };

                    customers.Add(customer);
                    if (branches is not null)
                    {
                        banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Customers = customers;
                    }
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {customerName} with Account Id:{customerAccountId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Customer: {customerName} Already Existed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message AuthenticateToCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is not null)
                        {
                            bool isToCustomerAvilable = customers.Any(m => m.AccountId.Equals(customerAccountId) && m.IsActive == 1);
                            if (isToCustomerAvilable)
                            {
                                message.Result = true;
                                message.ResultMessage = "ToCustomer Validation Successful.";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "ToCustomer Validation Failed.";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Customers Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }
        public Message UpdateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
          string customerDateOfBirth, int customerGender)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Customer>? customers = null;
                Customer? customer = null;
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    customers = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Customers;
                    if (customers is not null)
                    {
                        customer = customers.Find(m => m.AccountId.Equals(customerAccountId) && m.IsActive == 1);
                    }
                }

                if (customer is not null)
                {
                    bool doAllInputsValid = true;
                    if (!string.IsNullOrEmpty(customerName))
                    {
                        customer.Name = customerName;
                    }

                    if (!string.IsNullOrEmpty(customerPassword))
                    {
                        byte[] salt = new byte[32];
                        salt = customer.Salt;
                        byte[] hashedPasswordToCheck = _encryptionService.HashPassword(customerPassword, salt);
                        if (Convert.ToBase64String(customer.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                        {
                            doAllInputsValid = false;
                            message.Result = false;
                            message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        }
                        else
                        {
                            salt = _encryptionService.GenerateSalt();
                            byte[] hashedPassword = _encryptionService.HashPassword(customerPassword, salt);
                            customer.Salt = salt;
                            customer.HashedPassword = hashedPassword;
                            message.Result = true;
                            message.ResultMessage = "Updated Password Sucessfully";
                        }
                    }
                    if (!string.IsNullOrEmpty(customerPhoneNumber))
                    {
                        customer.PhoneNumber = customerPhoneNumber;
                    }

                    if (!string.IsNullOrEmpty(customerEmailId))
                    {
                        customer.EmailId = customerEmailId;
                    }

                    if (customerAccountType is not 0)
                    {
                        customer.AccountType = (AccountType)customerAccountType;
                    }

                    if (!string.IsNullOrEmpty(customerAddress))
                    {
                        customer.Address = customerAddress;
                    }

                    if (!string.IsNullOrEmpty(customerDateOfBirth))
                    {
                        customer.DateOfBirth = customerDateOfBirth;
                    }

                    if (customerGender is not 0)
                    {
                        customer.Gender = (Gender)customerGender;
                    }

                    if (doAllInputsValid)
                    {
                        _fileService.WriteFile(banks);
                        message.Result = true;
                        message.ResultMessage = $"Account '{customerAccountId}' Succesfully updated";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Customer: {customerName} with AccountId:{customerAccountId} Doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message DeleteCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Customer>? customers = null;
                Customer? customer = null;
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches;
                if (branches != null)
                {
                    customers = branches[branches.FindIndex(br => br.BranchId == branchId)].Customers;
                    if (customers != null)
                    {
                        customer = customers.Find(m => m.AccountId == customerAccountId && m.IsActive == 1);
                    }
                }

                if (customer != null)
                {
                    customer.IsActive = 0;
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{customerAccountId} Successfully.";
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"{customerAccountId} Doesn't Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message DepositAmount(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode)
        {
            Message message = new();
            banks = _fileService.GetData();
            bool isCurrencyAvailable = false;

            message = IsAccountExist(bankId, branchId, customerAccountId);
            if (message.Result)
            {
                if (depositAmount > 0)
                {
                    var bankIndex = banks.FindIndex(bk => bk.BankId.Equals(bankId));
                    var currency = banks[bankIndex].Currency.Find(cr => cr.IsActive == 1)!;

                    decimal exchangedAmount = 0;
                    if (currency.DefaultCurrencyCode.Equals(currencyCode))
                    {
                        exchangedAmount = depositAmount * currency.ExchangeRate;
                    }
                    else if (!currency.CurrencyCode.Equals(currencyCode))
                    {
                        isCurrencyAvailable = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Currency.Any(cur => cur.CurrencyCode.Equals(currencyCode));
                        if (isCurrencyAvailable)
                        {
                            int currenyCodeIndex = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Currency.FindIndex(cur => cur.CurrencyCode.Equals(currencyCode));
                            if (currenyCodeIndex != -1)
                            {
                                decimal exchangeRateValue = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Currency[currenyCodeIndex].ExchangeRate;
                                exchangedAmount = depositAmount * exchangeRateValue;
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Currency code not available in the bank.please add Currencies with exchange Rates.";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Entered Currency is not Acceptable by Bank.Please Kindly Consult Branch Manager";
                        }
                    }

                    if (exchangedAmount > 0)
                    {
                        message = CheckAccountBalance(bankId, branchId, customerAccountId);
                        bool isBalanceAvailable = decimal.TryParse(message.Data, out decimal avlbalance);
                        avlbalance += exchangedAmount;

                        var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                        if (bank is not null)
                        {
                            var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                            if (branch is not null)
                            {
                                List<Customer> customers = branch.Customers;
                                if (customers is not null)
                                {
                                    var customer = customers.Find(m => m.AccountId.Equals(customerAccountId));
                                    if (customer is not null)
                                    {
                                        customer.Balance = avlbalance;
                                        _fileService.WriteFile(banks);
                                        message.Result = true;
                                        message.ResultMessage = $"Amount:'{exchangedAmount}' Added Succesfully";
                                        _transactionService.TransactionHistory(bankId, branchId, customerAccountId, 0, exchangedAmount, customer.Balance, 1);
                                    }
                                    else
                                    {
                                        message.Result = false;
                                        message.ResultMessage = "Customer Doest Exist";
                                    }
                                }
                                else
                                {
                                    message.Result = false;
                                    message.ResultMessage = "No Customers Available";
                                }
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Branch Not Found";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Bank Not Found";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Amount less than 0";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Amount less than 0";
                }

            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Not Existed";
            }
            return message;
        }

        public Message CheckAccountBalance(string bankId, string branchId, string customerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = IsAccountExist(bankId, branchId, customerAccountId);
            decimal customerBalance = 0;

            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is not null)
                        {
                            var customer = customers.Find(c => c.AccountId.Equals(customerAccountId));
                            if (customer is not null)
                            {
                                customerBalance = customer.Balance;
                                message.Result = true;
                                message.ResultMessage = $"Available Balance :{customerBalance}";
                                message.Data = $"{customerBalance}";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Customer Doest Exist";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "No Customers Available";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Doest Exist";
            }

            return message;
        }

        public Message CheckToCustomerAccountBalance(string bankId, string branchId, string customerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = IsAccountExist(bankId, branchId, customerAccountId);
            decimal customerBalance = 0;

            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is not null)
                        {
                            var customer = customers.Find(c => c.AccountId.Equals(customerAccountId));
                            if (customer is not null)
                            {
                                customerBalance = customer.Balance;
                                message.Result = true;
                                message.ResultMessage = $"Available Balance :{customerBalance}";
                                message.Data = $"{customerBalance}";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Customer Doest Exist";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "No Customers Available";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Doest Exist";
            }

            return message;
        }

        public Message WithdrawAmount(string bankId, string branchId, string customerAccountId, decimal withDrawAmount)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = CheckAccountBalance(bankId, branchId, customerAccountId);
            bool isValid = decimal.TryParse(message.Data, out decimal balance);
            if (balance == 0)
            {
                message.Result = false;
                message.ResultMessage = "Failed ! Account Balance: 0 Rupees";
            }
            else if (isValid && balance < withDrawAmount)
            {
                message.Result = false;
                message.ResultMessage = $"Insufficient funds !! Aval.Bal is {balance} Rupees";
            }
            else
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers is not null)
                        {
                            var customer = customers.Find(c => c.AccountId.Equals(customerAccountId));
                            if (customer is not null)
                            {
                                customer.Balance -= withDrawAmount;
                                _fileService.WriteFile(banks);
                                message.Result = true;
                                message.ResultMessage = $"Withdraw Successful!! Aval.Bal is {customer.Balance}Rupees";
                                _transactionService.TransactionHistory(bankId, branchId, customerAccountId, withDrawAmount, 0, customer.Balance, 2);
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Customer Doest Exist";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "No Customers Available";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            return message;
        }

        public Message TransferAmount(string bankId, string branchId, string customerAccountId, string toBankId,
            string toBranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod)
        {
            Message message = new();
            banks = _fileService.GetData();
            Message fromCustomer = IsAccountExist(bankId, branchId, customerAccountId);
            int bankInterestRate = 0;
            int fromBankIndex = banks.FindIndex(b => b.BankId.Equals(bankId));
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId.Equals(branchId));
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(customerAccountId));
            Message toCustomer = IsAccountExist(toBankId, toBranchId, toCustomerAccountId);
            int toBankIndex = banks.FindIndex(b => b.BankId.Equals(toBankId));
            int toBranchIndex = banks[toBankIndex].Branches.FindIndex(br => br.BranchId.Equals(toBranchId));
            int toCustomerIndex = banks[toBankIndex].Branches[toBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(toCustomerAccountId));
            if (fromCustomer.Result && toCustomer.Result)
            {
                if (bankId.Substring(0, 3).Equals(toBankId.Substring(0, 3)))
                {
                    if (transferMethod == 1)
                    {
                        var charges = banks[fromBankIndex].Branches[fromBranchIndex].Charges.Find(c => c.IsActive == 1);
                        if (charges is not null)
                        {
                            bankInterestRate = charges.RtgsSameBank;
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Chareges Not Available";
                        }
                    }
                    else if (transferMethod == 2)
                    {
                        var charges = banks[fromBankIndex].Branches[fromBranchIndex].Charges.Find(c => c.IsActive == 1);
                        if (charges is not null)
                        {
                            bankInterestRate = charges.ImpsSameBank;
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Chareges Not Available";
                        }
                    }
                }
                else if (bankId.Substring(0, 3) != toBankId.Substring(0, 3))
                {
                    if (transferMethod == 1)
                    {
                        var charges = banks[fromBankIndex].Branches[fromBranchIndex].Charges.Find(c => c.IsActive == 1);
                        if (charges is not null)
                        {
                            bankInterestRate = charges.RtgsOtherBank;
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Chareges Not Available";
                        }
                    }
                    else if (transferMethod == 2)
                    {
                        var charges = banks[fromBankIndex].Branches[fromBranchIndex].Charges.Find(c => c.IsActive == 1);
                        if (charges is not null)
                        {
                            bankInterestRate = charges.ImpsOtherBank;
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Chareges Not Available";
                        }
                    }
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Account Doesn't Exist";
            }

            decimal transferAmountInterest = transferAmount * (bankInterestRate / 100.0m);
            decimal transferAmountWithInterest = transferAmount + transferAmountInterest;

            message = CheckAccountBalance(bankId, branchId, customerAccountId);
            decimal fromCustomerBalanace = decimal.Parse(message.Data);
            if (fromCustomerBalanace >= transferAmountInterest + transferAmount)
            {
                banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Balance -= transferAmountWithInterest;
                banks[toBankIndex].Branches[toBranchIndex].Customers[toCustomerIndex].Balance += transferAmount;
                _fileService.WriteFile(banks);
                message = CheckAccountBalance(bankId, branchId, customerAccountId);
                fromCustomerBalanace = decimal.Parse(message.Data);

                message = CheckToCustomerAccountBalance(toBankId, toBranchId, toCustomerAccountId);
                decimal toCustomerBalance = decimal.Parse(message.Data);
                _transactionService.TransactionHistory(bankId, branchId, customerAccountId, toBankId, toBranchId, toCustomerAccountId, transferAmount, 0, fromCustomerBalanace, toCustomerBalance, 3);
                message.Result = true;
                message.ResultMessage = $"Transfer of {transferAmount} Rupees Sucessfull.,Deducted Amout :{transferAmount + transferAmountInterest}, Avl.Bal: {fromCustomerBalanace}";
            }
            else
            {
                decimal requiredAmount = Math.Abs(fromCustomerBalanace - transferAmountInterest + transferAmount);
                message.Result = false;
                message.ResultMessage = $"Insufficient Balance.,Avl.Bal:{fromCustomerBalanace},Add {requiredAmount} or Reduce the Transfer Amount Next Time.";
            }
            return message;
        }
        public string GetPassbook(string bankId, string branchId, string customerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = IsAccountExist(bankId, branchId, customerAccountId);
            if (message.Result)
            {
                int fromBankIndex = banks.FindIndex(b => b.BankId.Equals(bankId));
                int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId.Equals(branchId));
                int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(customerAccountId));

                Customer details = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex];
                return details.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
