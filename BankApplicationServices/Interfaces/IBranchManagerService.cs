using BankApplicationModels;
namespace BankApplicationServices.Interfaces
{
    public interface IBranchManagerService
    {
        Message AddTransactionCharges(ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank);
        Message OpenStaffAccount(string staffName, string staffPassword, ushort staffRole);
        Message ValidateBranchManagerAccount(string bankId, string branchId, string branchManagerAccountId, string branchManagerPassword);
    }
}