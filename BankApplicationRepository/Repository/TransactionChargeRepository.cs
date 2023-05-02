using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository.Repository
{
    public class TransactionChargeRepository : ITransactionChargeRepository
    {
        private readonly BankDBContext _context;
        public TransactionChargeRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<TransactionCharge?> GetTransactionCharges(string branchId)
        {
            return _context.TransactionCharges.FirstOrDefault(c => c.BranchId.Equals(branchId) && c.IsActive.Equals(true));
        }
        public async Task<bool> AddTransactionCharges(TransactionCharge transactionCharges, string branchId)
        {
            transactionCharges.BranchId = branchId;
            await _context.TransactionCharges.AddAsync(transactionCharges);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateTransactionCharges(TransactionCharge transactionCharges, string branchId)
        {
            TransactionCharge transactionChargesObj = await GetTransactionCharges(branchId);

            if (transactionCharges.RtgsSameBank >= 0 && transactionCharges.RtgsSameBank <= 100)
            {
                transactionChargesObj.RtgsSameBank = transactionCharges.RtgsSameBank;
            }

            if (transactionCharges.RtgsOtherBank >= 0 && transactionCharges.RtgsOtherBank <= 100)
            {
                transactionChargesObj.RtgsOtherBank = transactionCharges.RtgsOtherBank;
            }

            if (transactionCharges.ImpsSameBank >= 0 && transactionCharges.ImpsSameBank <= 100)
            {
                transactionChargesObj.ImpsSameBank = transactionCharges.ImpsSameBank;
            }

            if (transactionCharges.ImpsOtherBank >= 0 && transactionCharges.ImpsOtherBank <= 100)
            {
                transactionChargesObj.ImpsOtherBank = transactionCharges.ImpsOtherBank;
            }
            _context.TransactionCharges.Update(transactionChargesObj);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTransactionCharges(string branchId)
        {
            TransactionCharge transactionChargesObj = await GetTransactionCharges(branchId);
            transactionChargesObj.IsActive = false;
            _context.TransactionCharges.Update(transactionChargesObj);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> IsTransactionChargesExist(string branchId)
        {
            return await _context.TransactionCharges.AnyAsync(c => c.BranchId.Equals(branchId) && c.IsActive.Equals(true));
        }
    }
}
