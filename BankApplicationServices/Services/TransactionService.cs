using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICustomerRepository _customerRepository;
        public TransactionService(ITransactionRepository transactionRepository, ICustomerRepository customerRepository)
        {
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionHistory(string fromCustomerAccountId)
        {
            return await _transactionRepository.GetAllTransactions(fromCustomerAccountId);
        }

        public async Task<Transaction> GetTransactionById(string fromCustomerAccountId, string transactionId)
        {
            Transaction transaction = await _transactionRepository.GetTransactionById(fromCustomerAccountId, transactionId);
            return transaction;
        }

        public async Task<Message> IsTransactionsAvailableAsync(string fromCustomerAccountId)
        {
            Message message = new();
            bool isTransactionsAvlable = await _transactionRepository.IsTransactionsExist(fromCustomerAccountId);
            if (isTransactionsAvlable)
            {
                message.Result = true;
                message.ResultMessage = "Transactions Available.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Transactions Available.";
            }
            return message;
        }

        
        public async Task<Message> TransactionHistoryAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount,
          decimal creditAmount, decimal fromCustomerbalance, TransactionType transactionType)
        {
            Message message = new();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string transactionId = string.Concat("TXN", fromBankId.AsSpan(0, 3), fromCustomerAccountId.AsSpan(0, 3), date);
            Transaction transaction = new()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                TransactionType = transactionType,
                TransactionId = transactionId,
                FromCustomerAccountId = fromCustomerAccountId,
                Debit = debitAmount,
                Credit = creditAmount,
                Balance = fromCustomerbalance,
                TransactionDate = date,
            };
            bool isTransactionAdded = await _transactionRepository.AddTransaction(transaction);
            if (isTransactionAdded)
            {
                message.Result = true;
                message.ResultMessage = "Transaction Added Successfully";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Failed to Add Transaction.";
            }
            return message;
        }

        public async Task<Message> TransactionHistoryFromAndToAsync(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId,
            decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, TransactionType transactionType)
        {
            Message message = new();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string transactionId = string.Concat("TXN", fromBankId.AsSpan(0, 3), fromCustomerAccountId.AsSpan(0, 3), date);

            Transaction fromCustomertransaction = new()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                FromCustomerAccountId = fromCustomerAccountId,
                ToCustomerAccountId = toCustomerAccountId,
                ToCustomerBankId = toBankId,
                ToCustomerBranchId = toBranchId,
                TransactionType = transactionType,
                TransactionId = transactionId,
                Debit = debitAmount,
                Credit = creditAmount,
                Balance = fromCustomerbalance,
                TransactionDate = date
            };

            Transaction toCustomertransaction = new()
            {
                FromCustomerBankId = fromBankId,
                FromCustomerBranchId = fromBranchId,
                FromCustomerAccountId = toCustomerAccountId,
                ToCustomerAccountId = fromCustomerAccountId,
                ToCustomerBankId = toBankId,
                ToCustomerBranchId = toBranchId,
                TransactionType = transactionType,
                TransactionId = transactionId,
                Debit = creditAmount,
                Credit = debitAmount,
                Balance = toCustomerBalance,
                TransactionDate = date
            };
            bool isFromTransactionAdded = await _transactionRepository.AddTransaction(fromCustomertransaction);
            bool isToTransactionAdded = await _transactionRepository.AddTransaction(toCustomertransaction);
            if (isFromTransactionAdded && isToTransactionAdded)
            {
                message.Result = true;
                message.ResultMessage = "Transactions Added Succesfully";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Failed to Add Transactions";
            }
            return message;
        }
        

        public async Task<Message> RevertTransactionAsync(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId,
          string toBranchId, string toCustomerAccountId)
        {
            Message message = new();
            Transaction fromCustomerTransaction = await _transactionRepository.GetTransactionById(fromCustomerAccountId, transactionId);
            if (fromCustomerTransaction is not null)
            {
                string location = "to";
                Transaction toCustomerTransaction = await _transactionRepository.GetTransactionById(toCustomerAccountId, transactionId, location);
                if (fromCustomerTransaction is not null)
                {
                    Customer toCustomer = await _customerRepository.GetCustomerById(toCustomerAccountId, toBranchId);
                    Customer fromCustomer = await _customerRepository.GetCustomerById(fromCustomerAccountId, fromBranchId);
                    decimal toCustomerAmount = toCustomer.Balance;
                    if (toCustomerAmount >= fromCustomerTransaction.Debit)
                    {
                        Customer fromCustomerObject = new()
                        {
                            Balance = fromCustomer.Balance + toCustomerTransaction.Credit
                        };

                        Customer toCustomerObject = new()
                        {
                            Balance = toCustomer.Balance - toCustomerTransaction.Credit
                        };
                        bool isFromAccUpdated = await _customerRepository.UpdateCustomerAccount(fromCustomerObject, fromBranchId);
                        bool isToAccUpdated = await _customerRepository.UpdateCustomerAccount(toCustomerObject, toBranchId);
                        if (isFromAccUpdated && isToAccUpdated)
                        {
                            message = await TransactionHistoryFromAndToAsync(fromBankId, fromBranchId, fromCustomerAccountId,
                            toBankId, toBranchId, toCustomerAccountId, 0, toCustomerTransaction.Credit,
                            fromCustomer.Balance, toCustomer.Balance, TransactionType.Revert);
                            if (message.Result)
                            {
                                message.Result = true;
                                message.ResultMessage = $"Account Id:{fromCustomerAccountId} Reverted with Amount :{fromCustomerTransaction.Debit} Updated Balance:{fromCustomer.Balance}";
                            }
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
                    message.ResultMessage = "To Customer Transaction Data Not Available";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "From Customer Transaction Data Not Available";
            }
            return message;
        }
    }
}
