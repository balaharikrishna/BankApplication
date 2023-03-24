using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CustomerService : ICustomerService
    {
        IFileService _fileService;
        List<Bank> banks;
        public CustomerService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }

        Message message = new Message();

        private static bool isCustomerExist = false;
        private static bool isToCustomerExist = false;
        public static string fromBankId = string.Empty;
        public static string toBankId = string.Empty;
        public static string frombranchId = string.Empty;
        public static string tobranchId = string.Empty;
        private static string fromCustomerAccountId = string.Empty;
        private static string toCustomerAccountId = string.Empty;
        private static int toCustomerbankObjectIndex = 0;
        private static int toCustomerbranchObjectIndex = 0;
        private static int toCustomerObjectIndex = 0;
        private static int fromCustomerBranchObjectIndex = 0;
        private static int fromCustomerBankObjectIndex = 0;
        public static int fromCustomerObjectIndex = 0;
        public bool AuthenticateCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
            bool result = false;
            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    int bankObjectIndex = banks.IndexOf(bank);
                    var branch = bank.Branches.FirstOrDefault(br => br.branchId == branchId);
                    if (branch != null)
                    {
                        int branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.branchId == branchId);
                        var customer = branch.Customers.FirstOrDefault(c => c.CustomerAccountID == customerAccountId);
                        if (customer != null)
                        {
                            isCustomerExist = banks[bankObjectIndex].Branches.Any(branch => branch.branchId == branchId);
                            fromCustomerObjectIndex = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.FindIndex(cust => cust.CustomerAccountID == customerAccountId);
                            fromBankId = bankId;
                            frombranchId = branchId;
                            fromCustomerAccountId = customerAccountId;
                            fromCustomerBankObjectIndex = bankObjectIndex;
                            fromCustomerBranchObjectIndex = branchObjectIndex;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }


        public Message AuthenticateCustomerLogin(string bankId, string branchId, string customerAccountId, string customerAccountPassword)
        {
            bool isCustomerAccountValid = ValidateCustomerAccount(bankId, branchId, customerAccountId);
            if (isCustomerAccountValid)
            {
                var customer = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].CustomerPassword == customerAccountPassword;
                message.Result = true;
                message.ResultMessage = $"Customer '{customerAccountId}' Validation Successgull.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Customer '{customerAccountId}' Validation Failed.";
            }
            return message;
        }

        public Message OpenCustomerAccount(string bankId, string branchId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
          string customerDateOfBirth, int customerGender)
        {
            bool isCustomerAlreadyRegistered = false;

            if (banks[bankObjectIndex].Branches[branchObjectIndex].Customers == null)
            {
                banks[bankObjectIndex].Branches[branchObjectIndex].Customers = new List<Customer>();
            }

            isCustomerAlreadyRegistered = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.Any(cn => cn.CustomerName == customerName);
            if (isCustomerAlreadyRegistered)
            {
                Console.WriteLine($"Customer '{customerName}' is already Registered");
                message.Result = false;
            }
            else if (!isCustomerAlreadyRegistered)
            {
                DateTime currentDate = DateTime.Now;
                string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                string UserFirstThreeCharecters = customerName.Substring(0, 3);
                string customerAccountId = UserFirstThreeCharecters + date;

                Customer branchCustomer = new Customer()
                {
                    CustomerAccountID = customerAccountId,
                    CustomerPassword = customerPassword,
                    CustomerAccountType = (AccountType)customerAccountType,
                    CustomerName = customerName,
                    CustomerPassbookIssueDate = currentDate,
                    CustomerAddress = customerAddress,
                    CustomerDateOfBirth = customerDateOfBirth,
                    CustomerGender = (Gender)customerGender,
                    CustomerEmailId = customerEmailId,
                    CustomerPhoneNumber = customerPhoneNumber
                };

                banks[bankObjectIndex].Branches[branchObjectIndex].Customers.Add(branchCustomer);
                _fileService.WriteFile(banks);

                message.Result = true;
                message.ResultMessage = $"Account Opened for Customer '{customerName} with AccountId '{customerAccountId}' Successfully.";
            }
            return message;
        }


        public bool AuthenticateToCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
            bool result = false;

            if (_fileService.ReadFile().Length > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    int bankObjectIndex = banks.IndexOf(bank);
                    var branch = bank.Branches.FirstOrDefault(br => br.branchId == branchId);
                    if (branch != null)
                    {
                        int branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.branchId == branchId);
                        var customer = branch.Customers.FirstOrDefault(c => c.CustomerAccountId == customerAccountId);
                        if (customer != null)
                        {
                            isToCustomerExist = banks[bankObjectIndex].Branches.Any(branch => branch.branchId == branchId);
                            toCustomerbankObjectIndex = bankObjectIndex;
                            toCustomerbranchObjectIndex = branchObjectIndex;
                            toCustomerAccountId = customerAccountId;
                            toBankId = bankId;
                            tobranchId = branchId;
                            toCustomerObjectIndex = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.FindIndex(cust => cust.CustomerAccountID == customerAccountId);
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        public Message UpdateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerName, string customerPassword,
          string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
          string customerDateOfBirth, int customerGender)
        {

            bool isCustomerAvailable = false;
            isCustomerAvailable = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.Any(cn => cn.CustomerAccountID == customerAccountId);
            if (isCustomerAvailable)
            {
                int customerObjectIndex = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.FindIndex(cn => cn.CustomerAccountID == customerAccountId);
                var customer = banks[bankObjectIndex].Branches[branchObjectIndex].Customers[customerObjectIndex];

                if (customerName != string.Empty)
                {
                    customer.CustomerName = customerName;
                }

                if (customerPassword != string.Empty)
                {
                    customer.CustomerPassword = customerPassword;
                }

                if (customerPhoneNumber != string.Empty)
                {
                    customer.CustomerPhoneNumber = customerPhoneNumber;
                }

                if (customerEmailId != string.Empty)
                {
                    customer.CustomerEmailId = customerEmailId;
                }

                if (customerAccountType != 0)
                {
                    customer.CustomerAccountType = (AccountType)customerAccountType;
                }

                if (customerAddress != string.Empty)
                {
                    customer.CustomerAddress = customerAddress;
                }

                if (customerDateOfBirth != string.Empty)
                {
                    customer.CustomerDateOfBirth = customerDateOfBirth;
                }

                if (customerGender != 0)
                {
                    customer.CustomerGender = (Gender)customerGender;
                }

                _fileService.WriteFile(banks);

                message.Result = true;
                message.ResultMessage = $"Account '{customerAccountId}' Succesfully updated";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Account '{customerAccountId}' is Not Available in Branch";
            }
            return message;
        }

        public Message DeleteCustomerAccount(string bankId, string branchId, string customerAccountId)
        {
            bool isCustomerAvailable = false;

            isCustomerAvailable = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.Any(cn => cn.CustomerAccountID == customerAccountId);
            if (isCustomerAvailable)
            {
                int deleteObjectIndex = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.FindIndex(cn => cn.CustomerAccountID == customerAccountId);
                banks[bankObjectIndex].Branches[branchObjectIndex].Customers.RemoveAt(deleteObjectIndex);

                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Account with Id '{customerAccountId}' is Deleted Succesfully.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Failed to Delete AccountId :'{customerAccountId}'";
            }

            return message;
        }

        public Message DepositAmount(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode)
        {
            Message message = new Message();
            int transactionStatus = 2;
            bool isCurrencyAvailable = false;

            bool isValidCustomer = _branchCustomerService.ValidateCustomerAccount(staffBankId, staffbranchId, customerAccountId);
            if (isValidCustomer)
            {
                int customerObjectIndex = CustomerService.fromCustomerObjectIndex;
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
                        isCurrencyAvailable = banks[bankObjectIndex].Currency.Any(cur => cur.CurrencyCode == currencyCode);
                        if (isCurrencyAvailable)
                        {
                            int currenyCodeIndex = banks[bankObjectIndex].Currency.FindIndex(cur => cur.CurrencyCode == currencyCode);
                            if (currenyCodeIndex != -1)
                            {
                                decimal exchangeRateValue = banks[bankObjectIndex].Currency[currenyCodeIndex].ExchangeRate;
                                exchangedAmount = depositAmount * exchangeRateValue;
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "No Currency code not available in the bank.please add Currencies with exchange rates.";
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
                        balance = banks[bankObjectIndex].Branches[branchObjectIndex].Customers[customerObjectIndex].CustomerAmount;
                        balance += exchangedAmount;
                        banks[bankObjectIndex].Branches[branchObjectIndex].Customers[customerObjectIndex].CustomerAmount = balance;
                        _fileService.WriteFile(banks);
                        message.Result = true;
                        message.ResultMessage = $"Amount:'{exchangedAmount}' Added Succesfully";
                        transactionStatus = 1;
                    }
                    _branchCustomerService.TransactionHistory(staffBankId, staffbranchId, customerAccountId, 0, exchangedAmount, balance, 1, transactionStatus);
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
            decimal customerBalance = 0;

            if (isCustomerExist)
            {
                customerBalance = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].CustomerAmount;
                message.Result = true;
                message.ResultMessage = $"Available Balance :{customerBalance}";
                message.Data = $"{customerBalance}";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Account '{fromCustomerAccountId}' does not Exist";
            }
            return message;
        }

        public Message CheckToCustomerAccountBalance()
        {
            decimal customerBalance = 0;

            if (isCustomerExist)
            {
                customerBalance = banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].CustomerAmount;
                message.Result = true;
                message.ResultMessage = $"Available Balance :{customerBalance}";
                message.Data = $"{customerBalance}";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Account '{toCustomerAccountId}' does not Exist";
            }
            return message;
        }

        public Message WithdrawAmount(decimal withDrawAmount)
        {
            message = CheckAccountBalance();
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
                balance = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].CustomerAmount -= withDrawAmount;
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Withdraw Successful!! Aval.Bal is {balance}Rupees";
                int transactionStatus = message.Result ? 1 : 2;
                TransactionHistory(fromBankId, frombranchId, fromCustomerAccountId, withDrawAmount, 0, balance, 2, transactionStatus);
            }

            return message;
        }

        public Message TransferAmount(string toBankId,
            string toBankbranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod)
        {
            if (isToCustomerExist && isCustomerExist)
            {
                int bankInterestRate = 0;

                if (fromBankId.Substring(0, 3) == toBankId.Substring(0, 3))
                {
                    if (transferMethod == 1)
                    {
                        bankInterestRate = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Charges[0].RtgsSameBank;
                    }
                    else if (transferMethod == 2)
                    {
                        bankInterestRate = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Charges[0].ImpsSameBank;
                    }

                }
                else if (fromBankId.Substring(0, 3) != toBankId.Substring(0, 3))
                {
                    if (transferMethod == 1)
                    {
                        bankInterestRate = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Charges[0].RtgsOtherBank;
                    }
                    else if (transferMethod == 2)
                    {
                        bankInterestRate = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Charges[0].ImpsOtherBank;
                    }
                }

                decimal transferAmountInterest = transferAmount * (bankInterestRate / 100.0m);
                decimal transferAmountWithInterest = transferAmount + transferAmountInterest;

                message = CheckAccountBalance();
                decimal fromCustomerBalanace = decimal.Parse(message.Data);
                if (fromCustomerBalanace > transferAmountInterest + transferAmount)
                {
                    banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].CustomerAmount -= transferAmountWithInterest;
                    banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].CustomerAmount += transferAmount;
                    _fileService.WriteFile(banks);
                    message = CheckAccountBalance();
                    fromCustomerBalanace = decimal.Parse(message.Data);

                    message = CheckToCustomerAccountBalance();
                    decimal toCustomerBalance = decimal.Parse(message.Data);
                    TransactionHistory(fromBankId, frombranchId, fromCustomerAccountId, toBankId, toBankbranchId, toCustomerAccountId, transferAmount, 0, fromCustomerBalanace, toCustomerBalance, 3, 1);
                    message.Result = true;
                    message.ResultMessage = $"Transfer of {transferAmount} ₹ Sucessfull.,Deducted Amout :{transferAmount + transferAmountInterest}, Avl.Bal: {fromCustomerBalanace}";


                }
                else
                {
                    decimal requiredAmount = Math.Abs(fromCustomerBalanace - transferAmountInterest + transferAmount);
                    message.Result = false;
                    message.ResultMessage = $"Insufficient Balance.,Avl.Bal:{fromCustomerBalanace},Add {requiredAmount} or Reduce the Transfer Amount Next Time.";
                    message = CheckAccountBalance();
                    fromCustomerBalanace = decimal.Parse(message.Data);
                    message = CheckToCustomerAccountBalance();
                    decimal toCustomerBalance = decimal.Parse(message.Data);
                    TransactionHistory(fromBankId, frombranchId, fromCustomerAccountId, toBankId, tobranchId, toCustomerAccountId, 0, 0, fromCustomerBalanace, toCustomerBalance, 3, 2);

                }

            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Not Existed";
            }
            return message;
        }

        public string GetPassbook()
        {
            if (isCustomerExist)
            {
                Customer details = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex];
                return details.ToString();
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
