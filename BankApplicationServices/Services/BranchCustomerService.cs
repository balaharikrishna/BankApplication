using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.Interfaces;


namespace BankApplicationServices.Services
{
    public class BranchCustomerService : IBranchCustomerService
    {
        IFileService _fileService;
        List<Bank> banks;
        public BranchCustomerService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }

        Message message = new Message();

        private static bool isCustomerExist = false;
        private static bool isToCustomerExist = false;
        public static string fromBankId = string.Empty;
        public static string toBankId = string.Empty;
        public static string fromBranchId = string.Empty;
        public static string toBranchId = string.Empty;
        private static string fromCustomerAccountId = string.Empty;
        private static string toCustomerAccountId = string.Empty;
        private static int toCustomerbankObjectIndex = 0;
        private static int toCustomerbranchObjectIndex = 0;
        private static int toCustomerObjectIndex = 0;
        private static int fromCustomerBranchObjectIndex = 0;
        private static int fromCustomerBankObjectIndex = 0;
        public static int fromCustomerObjectIndex = 0;
        public bool ValidateCustomerAccount(string bankId, string branchid, string customerAccountId)
        {
            bool result = false;
            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    int bankObjectIndex = banks.IndexOf(bank);
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchid);
                    if (branch != null)
                    {
                        int branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.BranchId == branchid);
                        var customer = branch.Customers.FirstOrDefault(c => c.CustomerAccountID == customerAccountId);
                        if (customer != null)
                        {
                            isCustomerExist = banks[bankObjectIndex].Branches.Any(branch => branch.BranchId == branchid);
                            fromCustomerObjectIndex = banks[bankObjectIndex].Branches[branchObjectIndex].Customers.FindIndex(cust => cust.CustomerAccountID == customerAccountId);
                            fromBankId = bankId;
                            fromBranchId = branchid;
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

        public Message ValidateCustomerLogin(string bankId, string branchid, string customerAccountId, string customerAccountPassword)
        {
            bool isCustomerAccountValid = ValidateCustomerAccount(bankId, branchid, customerAccountId);
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

        public bool ValidateToCustomerAccount(string bankId, string branchid, string customerAccountId)
        {
            bool result = false;

            if (_fileService.ReadFile().Length > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    int bankObjectIndex = banks.IndexOf(bank);
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchid);
                    if (branch != null)
                    {
                        int branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.BranchId == branchid);
                        var customer = branch.Customers.FirstOrDefault(c => c.CustomerAccountID == customerAccountId);
                        if (customer != null)
                        {
                            isToCustomerExist = banks[bankObjectIndex].Branches.Any(branch => branch.BranchId == branchid);
                            toCustomerbankObjectIndex = bankObjectIndex;
                            toCustomerbranchObjectIndex = branchObjectIndex;
                            toCustomerAccountId = customerAccountId;
                            toBankId = bankId;
                            toBranchId = branchid;
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


        public Message CheckAccountBalance()
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
                TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, withDrawAmount, 0, balance, 2, transactionStatus);
            }

            return message;
        }

        public Message TransferAmount(string toBankId,
            string toBankBranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod)
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
                    TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBankBranchId, toCustomerAccountId, transferAmount, 0, fromCustomerBalanace, toCustomerBalance, 3, 1);
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
                    TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBranchId, toCustomerAccountId, 0, 0, fromCustomerBalanace, toCustomerBalance, 3, 2);

                }

            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Not Existed";
            }
            return message;
        }

        public void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount,
           decimal creditAmount, decimal fromCustomerbalance, int transactionType, int transactionStatus)
        {
            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
            string transactionId = "TXN" + fromBankId + fromCustomerAccountId + date;

            if (banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions == null)
            {
                banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions = new List<CustomerTransaction>();

            }
            CustomerTransaction transaction = new CustomerTransaction()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                TransactionType = (TransactionType)transactionType,
                TransactionId = transactionId,
                FromCustomerAccountId = fromCustomerAccountId,
                Debit = debitAmount,
                Credit = creditAmount,
                Balance = fromCustomerbalance,
                TransactionDate = currentDate,
                TransactionStatus = (TransactionStatus)transactionStatus,
            };

            banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions.Add(transaction);
            _fileService.WriteFile(banks);
        }

        public void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId,
            decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, int transactionType, int transactionStatus)
        {
            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
            string transactionId = "TXN" + fromBankId + fromCustomerAccountId + date;

            if (banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions == null)
            {
                banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions = new List<CustomerTransaction>();
            }
            else if (banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].Transactions == null)
            {
                banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].Transactions = new List<CustomerTransaction>();
            }

            CustomerTransaction fromCustomertransaction = new CustomerTransaction()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                FromCustomerAccountId = fromCustomerAccountId,
                ToCustomerAccountId = toBankId,
                ToCustomerBankId = toBankId,
                ToCustomerBranchId = toBranchId,
                TransactionType = (TransactionType)transactionType,
                TransactionId = transactionId,
                Debit = debitAmount,
                Credit = creditAmount,
                Balance = fromCustomerbalance,
                TransactionDate = currentDate,
                TransactionStatus = (TransactionStatus)transactionStatus,
            };

            CustomerTransaction toCustomertransaction = new CustomerTransaction()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                FromCustomerAccountId = fromCustomerAccountId,
                ToCustomerAccountId = toCustomerAccountId,
                ToCustomerBankId = toBankId,
                ToCustomerBranchId = toBranchId,
                TransactionType = (TransactionType)transactionType,
                TransactionId = transactionId,
                Debit = creditAmount,
                Credit = debitAmount,
                Balance = toCustomerBalance,
                TransactionDate = currentDate,
                TransactionStatus = (TransactionStatus)transactionStatus,
            };

            banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions.Add(fromCustomertransaction);
            banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].Transactions.Add(toCustomertransaction);
            _fileService.WriteFile(banks);
        }
        public List<string> GetTransactionHistory()
        {

            List<CustomerTransaction> transactionList = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions;
            return transactionList.Select(t => t.ToString()).ToList();

        }

        public string GetPassbook()
        {
            if (isCustomerExist)
            {
                BranchCustomer details = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex];
                return details.ToString();
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
