using BankApplicationModels;
using BankApplicationModels.Enums;

namespace BankApplicationServices.IServices
{
    public interface IStaffService
    {
        Message IsStaffExist(string bankId, string branchId);
        Message AuthenticateStaffAccount(string bankId, string branchid, string staffAccountId, string staffAccountPassword);
        Message DeleteStaffAccount(string bankId, string branchId , string staffAccountId);
        Message OpenStaffAccount(string bankId, string branchId, string staffName, string staffPassword, StaffRole staffRole);
        Message UpdateStaffAccount(string bankId, string branchId, string staffAccountId, string staffName, string staffPassword, ushort staffRole);
        Message IsAccountExist(string bankId, string branchId, string staffAccountId);
        public string GetStaffDetails(string bankId, string branchId, string staffAccountId);
    }
}