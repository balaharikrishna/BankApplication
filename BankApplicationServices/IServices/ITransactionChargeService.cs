using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ITransactionChargeService
    {
        Message AddTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank);
        Message DeleteTransactionCharges(string bankId, string branchId);
        Message UpdateTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank);
    }
}