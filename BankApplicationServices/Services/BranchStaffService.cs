using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.Interfaces;

namespace BankApplicationServices.Services
{
    public class BranchStaffService : IBranchStaffService
    {
        IFileService _fileService;
        List<Bank> banks;
        public BranchStaffService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }
        private static int bankObjectIndex = 0;
        private static int branchObjectIndex = 0;
        private static string staffBankId = string.Empty;
        private static string staffBranchId = string.Empty;
        
        Message message = new Message();
        public Message ValidateBranchStaffAccount(string bankId, string branchid,
           string staffAccountId, string staffAccountPassword)
        {

            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchid);
                    if (branch != null)
                    {
                        bankObjectIndex = banks.IndexOf(bank);
                        var staff = branch.Staffs.FirstOrDefault(s => s.StaffAccountId == staffAccountId && s.StaffPassword == staffAccountPassword);
                        if (staff != null)
                        {
                            staffBankId = bankId;
                            staffBranchId = branchid;

                            message.Result = true;
                            message.ResultMessage = $"Validation Succesful";

                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"Validation Failed! Please check Accound Id and Password";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Entered branchId '{branchid}' is not found. Please Check Again.";
                    }
                    branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.BranchId == branchid);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Entered bankId '{bankId}' is not found. Please Check Again.";
                }
            }
            return message;
        }
        public Message OpenCustomerAccount(string customerName, string customerPassword,
           string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress,
           string customerDateOfBirth, int customerGender)
        {
            bool isCustomerAlreadyRegistered = false;

            if (banks[bankObjectIndex].Branches[branchObjectIndex].Customers == null)
            {
                banks[bankObjectIndex].Branches[branchObjectIndex].Customers = new List<BranchCustomer>();
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

                BranchCustomer branchCustomer = new BranchCustomer()
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

        public Message RevertTransaction(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId,
            string toBranchId, string toCustomerAccountId)
        {
            BranchCustomerService branchCustomerService = new BranchCustomerService();

            BranchCustomer branchCustomer = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.Find(cu => cu.CustomerAccountID == fromCustomerAccountId);
            CustomerTransaction fromCustomerTransaction = branchCustomer.Transactions.Find(tr => tr.TransactionId == transactionId);

            BranchCustomer toBranchCustomer = banks.Find(bk => bk.BankId == toBankId).Branches.Find(br => br.BranchId == toBranchId).Customers.Find(cu => cu.CustomerAccountID == toCustomerAccountId);
            CustomerTransaction toCustomerTransaction = toBranchCustomer.Transactions.Find(tr => tr.TransactionId == transactionId);
            decimal toActualCustomerAmount = toBranchCustomer.CustomerAmount;
            if (toActualCustomerAmount > fromCustomerTransaction.Debit)
            {
                if (toCustomerTransaction.TransactionId == fromCustomerTransaction.TransactionId)
                {
                    banks[bankObjectIndex].Branches[branchObjectIndex].Customers.Find(cu => cu.CustomerAccountID == fromCustomerAccountId).CustomerAmount += toCustomerTransaction.Credit;
                    banks.Find(bk => bk.BankId == toBankId).Branches.Find(br => br.BranchId == toBranchId).Customers.Find(cu => cu.CustomerAccountID == toCustomerAccountId).CustomerAmount -= toCustomerTransaction.Credit;
                    _fileService.WriteFile(banks);
                    branchCustomerService.TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBranchId, toCustomerAccountId, 0, toCustomerTransaction.Credit, branchCustomer.CustomerAmount, toBranchCustomer.CustomerAmount, 4, 1);
                    message.Result = true;
                    message.ResultMessage = $"Account Id:{fromCustomerAccountId} Reverted with Amount :{fromCustomerTransaction.Debit} Updated Balance:{branchCustomer.CustomerAmount}";
                }
                else
                {
                    message = branchCustomerService.CheckAccountBalance();
                    decimal fromCustomerBalanace = decimal.Parse(message.Data);

                    message = branchCustomerService.CheckToCustomerAccountBalance();
                    decimal toCustomerBalanace = decimal.Parse(message.Data);
                    branchCustomerService.TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBranchId, toCustomerAccountId, 0, 0, fromCustomerBalanace, toCustomerBalanace, 4, 2);
                    message.Result = false;
                    message.ResultMessage = $"Transaction Id are Mismatching.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"To Customer doesn't have the Requrequired Amount to be Deducted.";
            }



            return message;
        }

        public Message UpdateCustomerAccount(string customerAccountId, string customerName, string customerPassword,
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

        public Message DeleteCustomerAccount(string customerAccountId)
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

        public Message DepositAmount(string customerAccountId, decimal depositAmount, string currencyCode)
        {
            Message message = new Message();
            int transactionStatus = 2;
            bool isCurrencyAvailable = false;

            BranchCustomerService branchCustomerService = new BranchCustomerService();
            bool isValidCustomer = branchCustomerService.ValidateCustomerAccount(staffBankId, staffBranchId, customerAccountId);
            if (isValidCustomer)
            {
                int customerObjectIndex = BranchCustomerService.fromCustomerObjectIndex;
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
                    branchCustomerService.TransactionHistory(staffBankId, staffBranchId, customerAccountId, 0, exchangedAmount, balance, 1, transactionStatus);
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Not Existed";
            }
            return message;
        }


    }
}
