using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IFileService _fileService;
        private List<Bank> banks;
        public TransactionService(IFileService fileService)
        {
            _fileService = fileService;
            banks = new List<Bank>();
        }

        public Task<Message> IsTransactionsAvailableAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            int fromBankIndex = banks.FindIndex(b => b.BankId.Equals(fromBankId));
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId.Equals(fromBranchId));
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(fromCustomerAccountId));

            List<Transactions> transactions = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            if (transactions is null || transactions.Count < 0)
            {
                message.Result = false;
                message.ResultMessage = "No Transactions Available.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = "Transactions Available.";
            }
            return message;
        }

        public async void TransactionHistoryAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount,
          decimal creditAmount, decimal fromCustomerbalance, int transactionType)
        {
            banks = _fileService.GetData();
            int fromBankIndex = banks.FindIndex(b => b.BankId.Equals(fromBankId));
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId.Equals(fromBranchId));
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(fromCustomerAccountId));

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string transactionId = string.Concat("TXN", fromBankId.AsSpan(0, 3), fromCustomerAccountId.AsSpan(0, 3), date);
            var transactions = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            if (transactions is null)
            {
                transactions = new List<Transactions>();
            }

            Transactions transaction = new()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                TransactionType = (TransactionType)transactionType,
                TransactionId = transactionId,
                FromCustomerAccountId = fromCustomerAccountId,
                Debit = debitAmount,
                Credit = creditAmount,
                Balance = fromCustomerbalance,
                TransactionDate = date,
            };

            transactions.Add(transaction);
            banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions = transactions;
            _fileService.WriteFile(banks);
        }

        public async void TransactionHistoryAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId,
            decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, int transactionType)
        {
            banks = _fileService.GetData();
            int fromBankIndex = banks.FindIndex(b => b.BankId.Equals(fromBankId));
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId.Equals(fromBranchId));
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(fromCustomerAccountId));
            int toBankIndex = banks.FindIndex(b => b.BankId.Equals(toBankId));
            int toBranchIndex = banks[toBankIndex].Branches.FindIndex(br => br.BranchId.Equals(toBranchId));
            int toCustomerIndex = banks[toBankIndex].Branches[toBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(toCustomerAccountId));

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string transactionId = string.Concat("TXN", fromBankId.AsSpan(0, 3), fromCustomerAccountId.AsSpan(0, 3), date);
            var fromCustomertransactions = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            var toCustomertransactions = banks[toBankIndex].Branches[toBranchIndex].Customers[toCustomerIndex].Transactions;

            if (fromCustomertransactions is null)
            {
                fromCustomertransactions = new List<Transactions>();
            }
            else if (toCustomertransactions is null)
            {
                toCustomertransactions = new List<Transactions>();
            }

            Transactions fromCustomertransaction = new()
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
                TransactionDate = date
            };

            Transactions toCustomertransaction = new()
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
                TransactionDate = date
            };

            fromCustomertransactions.Add(fromCustomertransaction);
            toCustomertransactions.Add(toCustomertransaction);
            banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions = fromCustomertransactions;
            banks[toBankIndex].Branches[toBranchIndex].Customers[toCustomerIndex].Transactions = toCustomertransactions;
            _fileService.WriteFile(banks);
        }
        public Task<List<string>> GetTransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId)
        {
            banks = _fileService.GetData();
            int fromBankIndex = banks.FindIndex(b => b.BankId.Equals(fromBankId));
            int fromBranchIndex = banks[fromBankIndex].Branches.FindIndex(br => br.BranchId.Equals(fromBranchId));
            int fromCustomerIndex = banks[fromBankIndex].Branches[fromBranchIndex].Customers.FindIndex(c => c.AccountId.Equals(fromCustomerAccountId));

            List<Transactions> transactionList = banks[fromBankIndex].Branches[fromBranchIndex].Customers[fromCustomerIndex].Transactions;
            return transactionList.Select(t => t.ToString()).ToList();
        }

        public Task<Message> RevertTransactionAsync(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId,
          string toBranchId, string toCustomerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            Customer? fromBranchCustomer = null;
            Transactions? fromCustomerTransaction = null;

            var bank = banks.Find(bk => bk.BankId.Equals(fromBankId));
            if (bank is not null)
            {
                var branch = bank.Branches.Find(br => br.BranchId.Equals(fromBranchId));
                if (branch is not null)
                {
                    List<Customer> customers = branch.Customers;
                    if (customers is not null)
                    {
                        var customerData = customers.Find(cu => cu.AccountId.Equals(fromCustomerAccountId));
                        if (customerData is not null)
                        {
                            fromBranchCustomer = customerData;
                            var fromCustomerTransactionData = customerData.Transactions.Find(tr => tr.TransactionId.Equals(transactionId));
                            if (fromCustomerTransactionData is not null)
                            {
                                fromCustomerTransaction = fromCustomerTransactionData;
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Transaction Data Not Available";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Customer Doesn't Exist";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "No Customers Available in the Branch";
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

            Customer? toBranchCustomer = null;
            Transactions? toCustomerTransaction = null;
            var toBank = banks.Find(bk => bk.BankId.Equals(toBankId));
            if (toBank is not null)
            {
                var toBranches = toBank.Branches;
                if (toBranches is not null)
                {
                    var tobranch = toBranches.Find(br => br.BranchId.Equals(toBranchId));
                    if (tobranch is not null)
                    {
                        var toCustomers = tobranch.Customers;
                        if (toCustomerAccountId is not null)
                        {
                            var toCustomerData = toCustomers.Find(cu => cu.AccountId.Equals(toCustomerAccountId));
                            if (toCustomerData is not null)
                            {
                                toBranchCustomer = toCustomerData;

                                var toCustomerTransactionData = toCustomerData.Transactions.Find(tr => tr.TransactionId.Equals(transactionId));
                                if (toCustomerTransactionData is not null)
                                {
                                    toCustomerTransaction = toCustomerTransactionData; //entire Transaction Data
                                }
                                else
                                {
                                    message.Result = false;
                                    message.ResultMessage = "To Customer Transaction Data Not Available";
                                }
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Customer Doesn't Exist";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "No Customers Available in the Branch";
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
                    message.ResultMessage = "No Branches Available in Bank";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Not Found";
            }


            if (toBranchCustomer is not null && toCustomerTransaction is not null && fromCustomerTransaction is not null && fromBranchCustomer is not null && toCustomerAccountId is not null)
            {
                decimal toCustomerAmount = toBranchCustomer.Balance;
                if (toCustomerAmount >= fromCustomerTransaction.Debit)
                {
                    if (toCustomerTransaction.TransactionId.Equals(fromCustomerTransaction.TransactionId))
                    {
                        fromBranchCustomer.Balance += toCustomerTransaction.Credit;
                        toBranchCustomer.Balance -= toCustomerTransaction.Credit;
                        _fileService.WriteFile(banks);
                        TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBranchId, toCustomerAccountId, 0, toCustomerTransaction.Credit, fromBranchCustomer.Balance, toBranchCustomer.Balance, 4);
                        message.Result = true;
                        message.ResultMessage = $"Account Id:{fromCustomerAccountId} Reverted with Amount :{fromCustomerTransaction.Debit} Updated Balance:{fromBranchCustomer.Balance}";
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
            else
            {
                message.Result = false;
                message.ResultMessage = "Customer Transactions are not Matching.";
            }
            return message;
        }
    }
}
