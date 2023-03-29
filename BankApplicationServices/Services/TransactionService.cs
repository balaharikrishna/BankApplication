using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IFileService _fileService;
        private readonly ICustomerService _customerService;
        List<Bank> banks;
        public TransactionService(IFileService fileService, ICustomerService customerService)
        {
            _fileService = fileService;
            _customerService = customerService;
            banks = _fileService.GetData();
        }
        Message message = new Message();

        public void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount,
          decimal creditAmount, decimal fromCustomerbalance, int transactionType, int transactionStatus)
        {
            int fromBankIndex = banks.FindIndex(b => b.BankId == fromBankId);
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId == fromBranchId);
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId == fromCustomerAccountId);

            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
            string transactionId = "TXN" + fromBankId.Substring(0, 3) + fromCustomerAccountId.Substring(0, 3) + date;
            var transactions = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            if (transactions == null) transactions = new List<Transaction>();

            Transaction transaction = new Transaction()
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

            transactions.Add(transaction);
            _fileService.WriteFile(banks);
        }

        public void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId,
            decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, int transactionType, int transactionStatus)
        {
            int fromBankIndex = banks.FindIndex(b => b.BankId == fromBankId);
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId == fromBranchId);
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId == fromCustomerAccountId);
            int toBankIndex = banks.FindIndex(b => b.BankId == toBankId);
            int toBranchIndex = banks[toBankIndex].Branches.FindIndex(br => br.BranchId == toBranchId);
            int toCustomerIndex = banks[toBankIndex].Branches[toBranchIndex].Customers.FindIndex(c => c.AccountId == toCustomerAccountId);

            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
            string transactionId = "TXN" + fromBankId.Substring(0, 3) + fromCustomerAccountId.Substring(0, 3) + date;
            var fromCustomertransactions = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            var toCustomertransactions = banks[toBankIndex].Branches[toBranchIndex].Customers[toCustomerIndex].Transactions;

            if (fromCustomertransactions == null)
            {
                fromCustomertransactions = new List<Transaction>();
            }
            else if (toCustomertransactions == null)
            {
                toCustomertransactions = new List<Transaction>();
            }

            Transaction fromCustomertransaction = new Transaction()
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

            Transaction toCustomertransaction = new Transaction()
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

            fromCustomertransactions.Add(fromCustomertransaction);
            toCustomertransactions.Add(toCustomertransaction);
            _fileService.WriteFile(banks);
        }
        public List<string> GetTransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId)
        {
            int fromBankIndex = banks.FindIndex(b => b.BankId == fromBankId);
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId == fromBranchId);
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId == fromCustomerAccountId);

            List<Transaction> transactionList = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            return transactionList.Select(t => t.ToString()).ToList();

        }

        public Message RevertTransaction(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId,
          string toBranchId, string toCustomerAccountId)
        {
            Customer? fromBranchCustomer = null;
            Transaction? fromCustomerTransaction = null;

            Message fromCustomerExist = _customerService.IsAccountExist(fromBankId, fromBranchId, fromCustomerAccountId);
            Message toCustomerExist = _customerService.IsAccountExist(toBankId, toBranchId, toCustomerAccountId);
            if (fromCustomerExist.Result && toCustomerExist.Result)
            {
                var bank = banks.Find(bk => bk.BankId == fromBankId);
                if (bank != null)
                {
                    var branch = bank.Branches.Find(br=>br.BranchId == fromBranchId);
                    if (branch != null)
                    {
                        List<Customer> customers = branch.Customers;
                        if (customers != null)
                        {
                            var customerData = customers.Find(cu => cu.AccountId == fromCustomerAccountId);
                            if (customerData != null)
                            {
                                var fromCustomerTransactionData = customerData.Transactions.Find(tr => tr.TransactionId == transactionId);
                                if (fromCustomerTransactionData != null)
                                {
                                    fromCustomerTransaction = fromCustomerTransactionData;
                                }
                            }
                        }
                    }
                }

                Customer? toBranchCustomer = null;
                Transaction? toCustomerTransaction = null;
                var toBank = banks.Find(bk => bk.BankId == toBankId);
                if (toBank != null)
                {
                    var toBranches = toBank.Branches;
                    if (toBranches != null)
                    {
                        var tobranch = toBranches.Find(br => br.BranchId == toBranchId);
                        if (tobranch != null)
                        {
                            var toCustomers = tobranch.Customers;
                            if (toCustomerAccountId != null)
                            {
                                var toCustomerData = toCustomers.Find(cu => cu.AccountId == toCustomerAccountId);
                                if (toCustomerData != null)
                                {
                                    toBranchCustomer = toCustomerData; // entire Tocustomer Data

                                    var toCustomerTransactionData = toCustomerData.Transactions.Find(tr => tr.TransactionId == transactionId);
                                    if (toCustomerTransactionData != null)
                                    {
                                        toCustomerTransaction = toCustomerTransactionData; //entire Transaction Data
                                    }
                                }

                            }
                        }
                    }

                }

                if (toBranchCustomer != null && toCustomerTransaction != null && fromCustomerTransaction != null && fromBranchCustomer != null && toCustomerAccountId != null)
                {
                    decimal toCustomerAmount = toBranchCustomer.Amount;
                    if (toCustomerAmount > fromCustomerTransaction.Debit)
                    {
                        if (toCustomerTransaction.TransactionId == fromCustomerTransaction.TransactionId)
                        {
                            fromBranchCustomer.Amount += toCustomerTransaction.Credit;
                            toBranchCustomer.Amount -= toCustomerTransaction.Credit;
                            _fileService.WriteFile(banks);
                            TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBranchId, toCustomerAccountId, 0, toCustomerTransaction.Credit, fromBranchCustomer.Amount, toBranchCustomer.Amount, 4, 1);
                            message.Result = true;
                            message.ResultMessage = $"Account Id:{fromCustomerAccountId} Reverted with Amount :{fromCustomerTransaction.Debit} Updated Balance:{fromBranchCustomer.Amount}";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Transaction Id are Mismatching.";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "To Customer doesn't have the Required Amount to be Deducted.";
                    }

                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "One of the Customer Account Doesnt Exist.";
            }
            return message;
        }
    }
}
