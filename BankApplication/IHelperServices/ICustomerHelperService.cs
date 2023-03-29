namespace BankApplication.IHelperServices
{
    public interface ICustomerHelperService
    {
        void SelectedOption(ushort Option, string bankId, string branchId, string accountId);
    }
}