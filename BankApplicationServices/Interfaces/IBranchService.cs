namespace BankApplicationServices.Interfaces
{
    public interface IBranchService
    {
        string GetTransactionCharges(string bankId, string branchId);
    }
}