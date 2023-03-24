using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IStaffService
    {
        Message AuthenticateBranchStaffAccount(string bankId, string branchid, string staffAccountId, string staffAccountPassword);
        Message DeleteStaffAccount(string bankId, string branchId, object , string staffAccountId);
        Message OpenStaffAccount(string bankId, string branchId, string staffName, string staffPassword, ushort staffRole);
        Message UpdateStaffAccount(string bankId, string branchId, string staffAccountId, string staffName, string staffPassword, ushort staffRole);
    }
}