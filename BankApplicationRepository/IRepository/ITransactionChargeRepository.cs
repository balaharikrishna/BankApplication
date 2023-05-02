using BankApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationRepository.IRepository
{
    public interface ITransactionChargeRepository
    {
        Task<bool> AddTransactionCharges(TransactionCharge transactionCharges, string branchId);
        Task<bool> UpdateTransactionCharges(TransactionCharge transactionCharges, string branchId);
        Task<bool> DeleteTransactionCharges(string branchId);
        Task<bool> IsTransactionChargesExist(string branchId);
        Task<TransactionCharge?> GetTransactionCharges(string branchId);
    }
}
