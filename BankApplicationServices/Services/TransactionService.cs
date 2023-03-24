using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationServices.Services
{
    public class TransactionService : ITransactionService
    {
        public void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount,
          decimal creditAmount, decimal fromCustomerbalance, int transactionType, int transactionStatus)
        {
            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
            string transactionId = "TXN" + fromBankId + fromCustomerAccountId + date;

            if (banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions == null)
            {
                banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions = new List<Transaction>();

            }
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
                banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions = new List<Transaction>();
            }
            else if (banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].Transactions == null)
            {
                banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].Transactions = new List<Transaction>();
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

            banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions.Add(fromCustomertransaction);
            banks[toCustomerbankObjectIndex].Branches[toCustomerbranchObjectIndex].Customers[toCustomerObjectIndex].Transactions.Add(toCustomertransaction);
            _fileService.WriteFile(banks);
        }
        public List<string> GetTransactionHistory()
        {

            List<Transaction> transactionList = banks[fromCustomerBankObjectIndex].Branches[fromCustomerBranchObjectIndex].Customers[fromCustomerObjectIndex].Transactions;
            return transactionList.Select(t => t.ToString()).ToList();

        }

        public Message RevertTransaction(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId,
          string toBranchId, string toCustomerAccountId)
        {
            Customer? fromBranchCustomer = null;
            Transaction? fromCustomerTransaction = null;
            Bank bank = banks[bankObjectIndex];
            if (bank != null)
            {
                BankBranch branch = bank.Branches[branchObjectIndex];
                if (branch != null)
                {
                    List<Customer> customers = bank.Branches[branchObjectIndex].Customers;
                    if (customers != null)
                    {
                        var customerData = customers.Find(cu => cu.CustomerAccountID == fromCustomerAccountId);
                        if (customerData != null)
                        {
                            fromBranchCustomer = customerData;
                            var fromCustomerTransactions = customerData.Transactions.Find(tr => tr.TransactionId == transactionId);
                            if (fromCustomerTransactions != null)
                            {
                                fromCustomerTransaction = fromCustomerTransactions;
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
                            var toCustomerData = toCustomers.Find(cu => cu.CustomerAccountID == toCustomerAccountId);
                            if (toCustomerData != null)
                            {

                                toBranchCustomer = toCustomerData;

                                var toCustomerTransactions = toCustomerData.Transactions.Find(tr => tr.TransactionId == transactionId);
                                if (toCustomerTransactions != null)
                                {
                                    toCustomerTransaction = toCustomerTransactions;
                                }
                            }

                        }
                    }
                }

            }

            if (toBranchCustomer != null && toCustomerTransaction != null && fromCustomerTransaction != null && fromBranchCustomer != null && toCustomerAccountId != null)
            {
                decimal toActualCustomerAmount = toBranchCustomer.CustomerAmount;
                if (toActualCustomerAmount > fromCustomerTransaction.Debit)
                {
                    if (toCustomerTransaction.TransactionId == fromCustomerTransaction.TransactionId)
                    {
                        fromBranchCustomer.CustomerAmount += toCustomerTransaction.Credit;
                        toBranchCustomer.CustomerAmount -= toCustomerTransaction.Credit;
                        _fileService.WriteFile(banks);
                        _branchCustomerService.TransactionHistory(fromBankId, fromBranchId, fromCustomerAccountId, toBankId, toBranchId, toCustomerAccountId, 0, toCustomerTransaction.Credit, fromBranchCustomer.CustomerAmount, toBranchCustomer.CustomerAmount, 4, 1);
                        message.Result = true;
                        message.ResultMessage = $"Account Id:{fromCustomerAccountId} Reverted with Amount :{fromCustomerTransaction.Debit} Updated Balance:{fromBranchCustomer.CustomerAmount}";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Transaction Id are Mismatching.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"To Customer doesn't have the Required Amount to be Deducted.";
                }

            }

            return message;
        }
    }
}
