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
        List<Bank> banks;
        public CustomerService(IFileService fileService, IEncryptionService encryptionService,
            IBranchService branchService, ITransactionService transactionService)
        {
            _fileService = fileService;
            _encryptionService = encryptionService;
            _branchService = branchService; 
            _transactionService = transactionService;
            banks = _fileService.GetData();
        }

        Message message = new Message();

        public Message AuthenticateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerPassword)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            byte[] salt = new byte[32];
                            var customer = customers.Find(m => m.AccountId == customerAccountId);
                            if (customer != null)
                            {
                                salt = customer.Salt;
                            }

                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(customerPassword, salt);
                            bool isCustomerAvilable = customers.Any(m => m.AccountId == customerAccountId && m.HashedPassword == hashedPasswordToCheck && m.IsActive == 1);
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
                }
            }
            return message;
        }

        public Message IsAccountExist(string bankId, string branchId, string customerAccountId)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            bool isCustomerAvilable = customers.Any(m => m.AccountId == customerAccountId && m.IsActive == 1);
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
                }
            }
            return message;
        }

        public Message OpenCustomerAccount(string bankId, string branchId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
          string customerDateOfBirth, int customerGender)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                bool isCustomerAlreadyAvailabe = false;
                List<Customer>? customers = null;
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches;
                if (branches != null)
                {
                    customers = branches[branches.FindIndex(br => br.BranchId == branchId)].Customers;
                    isCustomerAlreadyAvailabe = customers.Any(m => m.Name == customerName && m.IsActive == 1);
                }

                if (!isCustomerAlreadyAvailabe)
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string UserFirstThreeCharecters = customerName.Substring(0, 3);
                    string customerAccountId = UserFirstThreeCharecters + date;

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(customerPassword, salt);

                    Customer customer = new Customer()
                    {
                        Name = customerName,
                        AccountType = (AccountType)customerAccountType,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = customerAccountId,
                        PassbookIssueDate = currentDate,
                        Address = customerAddress,
                        DateOfBirth = customerDateOfBirth,
                        Gender = (Gender)customerGender,
                        EmailId = customerEmailId,
                        PhoneNumber = customerPhoneNumber,
                        IsActive = 1
                    };

                    if (customers == null)
                    {
                        customers = new List<Customer>();
                    }

                    customers.Add(customer);
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
            return message;
        }


        public Message AuthenticateToCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            bool isToCustomerAvilable = customers.Any(m => m.AccountId == customerAccountId && m.IsActive == 1);
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
                }
            }
            return message;
        }
        public Message UpdateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
          string customerDateOfBirth, int customerGender)
        {
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
                    bool doAllInputsValid = true;
                    if (customerName != string.Empty)
                    {
                        customer.Name = customerName;
                    }

                    if (customerPassword != string.Empty)
                    {
                        byte[] salt = new byte[32];
                        salt = customer.Salt;
                        byte[] hashedPasswordToCheck = _encryptionService.HashPassword(customerPassword, salt);
                        if (customer.HashedPassword == hashedPasswordToCheck)
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
                    if (customerPhoneNumber != string.Empty)
                    {
                        customer.PhoneNumber = customerPhoneNumber;
                    }

                    if (customerEmailId != string.Empty)
                    {
                        customer.EmailId = customerEmailId;
                    }

                    if (customerAccountType != 0)
                    {
                        customer.AccountType = (AccountType)customerAccountType;
                    }

                    if (customerAddress != string.Empty)
                    {
                        customer.Address = customerAddress;
                    }

                    if (customerDateOfBirth != string.Empty)
                    {
                        customer.DateOfBirth = customerDateOfBirth;
                    }

                    if (customerGender != 0)
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
            return message;

        }

        public Message DeleteCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
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
            return message;
        }

        public Message DepositAmount(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode)
        {
            Message message = new Message();
            int transactionStatus = 2;
            bool isCurrencyAvailable = false;

            message = IsAccountExist(bankId, branchId, customerAccountId);
            if (message.Result)
            {
                if (depositAmount > 0)
                {
                    decimal exchangedAmount = 0;
                    decimal balance = 0;
                    Currency currency = new Currency();
                    if (currency.DefaultCurrencyCode == currencyCode)
                    {
                        exchangedAmount = depositAmount * currency.DefaultCurrencyExchangeRate;
                    }
                    else if (currency.DefaultCurrencyCode != currencyCode)
                    {
                        isCurrencyAvailable = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency.Any(cur => cur.CurrencyCode == currencyCode);
                        if (isCurrencyAvailable)
                        {
                            int currenyCodeIndex = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency.FindIndex(cur => cur.CurrencyCode == currencyCode);
                            if (currenyCodeIndex != -1)
                            {
                                decimal exchangeRateValue = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency[currenyCodeIndex].ExchangeRate;
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
                        decimal avlbalance;
                        bool isBalanceAvailable = decimal.TryParse(message.Data, out avlbalance);
                        avlbalance += exchangedAmount;

                        var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                        if (bank != null)
                        {
                            var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                            if (branch != null)
                            {
                                List<Customer> customers = branch.Customers;
                                if (customers != null)
                                {
                                    var customer = customers.Find(m => m.AccountId == customerAccountId);
                                    if (customer != null)
                                    {
                                        customer.Amount = avlbalance;
                                        _fileService.WriteFile(banks);
                                        message.Result = true;
                                        message.ResultMessage = $"Amount:'{exchangedAmount}' Added Succesfully";
                                        transactionStatus = 1;
                                        _transactionService.TransactionHistory(bankId, branchId, customerAccountId, 0, exchangedAmount, balance, 1, transactionStatus);
                                    }
                                }
                            }
                        }

                    }

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
            message = IsAccountExist(bankId, branchId, customerAccountId);
            decimal customerBalance = 0;

            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            var customer = customers.Find(c => c.AccountId == customerAccountId);
                            if (customer != null)
                            {
                                customerBalance = customer.Amount;
                                message.Result = true;
                                message.ResultMessage = $"Available Balance :{customerBalance}";
                                message.Data = $"{customerBalance}";
                            }
                        }
                    }
                }
            }
            return message;
        }

        public Message CheckToCustomerAccountBalance(string bankId, string branchId, string customerAccountId)
        {
            message = IsAccountExist(bankId, branchId, customerAccountId);
            decimal customerBalance = 0;

            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            var customer = customers.Find(c => c.AccountId == customerAccountId);
                            if (customer != null)
                            {
                                customerBalance = customer.Amount;
                                message.Result = true;
                                message.ResultMessage = $"Available Balance :{customerBalance}";
                                message.Data = $"{customerBalance}";
                            }
                        }
                    }
                }
            }
            return message;
        }

        public Message WithdrawAmount(string bankId, string branchId, string customerAccountId, decimal withDrawAmount)
        {
            message = CheckAccountBalance(bankId, branchId, customerAccountId);
            decimal balance;
            decimal.TryParse(message.Data, out balance);
            if (balance == 0)
            {
                message.Result = false;
                message.ResultMessage = "Failed ! Account Balance: 0 Rupees";
            }
            else if (balance < withDrawAmount)
            {
                message.Result = false;
                message.ResultMessage = $"Insufficient funds !! Aval.Bal is {balance} Rupees";
            }
            else
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            var customer = customers.Find(c => c.AccountId == customerAccountId);
                            if (customer != null)
                            {
                                customer.Amount -= withDrawAmount;
                                _fileService.WriteFile(banks);
                                message.Result = true;
                                message.ResultMessage = $"Withdraw Successful!! Aval.Bal is {customer.Amount}Rupees";
                                int transactionStatus = 1;
                                _transactionService.TransactionHistory(bankId, branchId, customerAccountId, withDrawAmount, 0, customer.Amount, 2, transactionStatus);
                            }
                        }
                    }
                }
            }
            return message;
        }

        public Message TransferAmount(string bankId, string branchId, string customerAccountId, string toBankId,
            string toBranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod)
        {
            Message fromCustomer = IsAccountExist(bankId, branchId, customerAccountId);
            Message toCustomer = IsAccountExist(toBankId, toBranchId, toCustomerAccountId);
            int bankInterestRate = 0;
            int fromBankIndex = banks.FindIndex(b => b.BankId == bankId);
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId == branchId);
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId == customerAccountId);
            int toBankIndex = banks.FindIndex(b => b.BankId == toBankId);
            int toBranchIndex = banks[toBankIndex].Branches.FindIndex(br => br.BranchId == toBranchId);
            int toCustomerIndex = banks[toBankIndex].Branches[toBranchIndex].Customers.FindIndex(c => c.AccountId == toCustomerAccountId);
            if (fromCustomer.Result && toCustomer.Result)
            {
                if (bankId.Substring(0, 3) == toBankId.Substring(0, 3))

                    if (transferMethod == 1)
                    {
                        bankInterestRate = banks[fromBankIndex].Branches[fromBranchIndex].Charges[0].RtgsSameBank;
                    }
                    else if (transferMethod == 2)
                    {
                        bankInterestRate = banks[fromBankIndex].Branches[fromBranchIndex].Charges[0].ImpsSameBank;
                    }

            }
            else if (bankId.Substring(0, 3) != toBankId.Substring(0, 3))
            {
                if (transferMethod == 1)
                {
                    bankInterestRate = banks[fromBankIndex].Branches[fromBranchIndex].Charges[0].RtgsOtherBank;
                }
                else if (transferMethod == 2)
                {
                    bankInterestRate = banks[fromBankIndex].Branches[fromBranchIndex].Charges[0].ImpsOtherBank;
                }
            }

            decimal transferAmountInterest = transferAmount * (bankInterestRate / 100.0m);
            decimal transferAmountWithInterest = transferAmount + transferAmountInterest;

            message = CheckAccountBalance(bankId, branchId, customerAccountId);
            decimal fromCustomerBalanace = decimal.Parse(message.Data);
            if (fromCustomerBalanace > transferAmountInterest + transferAmount)
            {
                banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Amount -= transferAmountWithInterest;
                banks[toBankIndex].Branches[toBranchIndex].Customers[toCustomerIndex].Amount += transferAmount;
                _fileService.WriteFile(banks);
                message = CheckAccountBalance(bankId, branchId, customerAccountId);
                fromCustomerBalanace = decimal.Parse(message.Data);

                message = CheckToCustomerAccountBalance(toBankId, toBranchId, toCustomerAccountId);
                decimal toCustomerBalance = decimal.Parse(message.Data);
                _transactionService.TransactionHistory(bankId, branchId, customerAccountId, toBankId, toBranchId, toCustomerAccountId, transferAmount, 0, fromCustomerBalanace, toCustomerBalance, 3, 1);
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
            message = IsAccountExist(bankId, branchId, customerAccountId);
            if (message.Result)
            {
                int fromBankIndex = banks.FindIndex(b => b.BankId == bankId);
                int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId == branchId);
                int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId == customerAccountId);

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
