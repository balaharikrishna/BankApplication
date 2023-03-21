namespace BankApplicationServices.Interfaces
{
    public interface IBankService
    {
        Dictionary<string, decimal> GetExchangeRates(string bankId);
    }
}