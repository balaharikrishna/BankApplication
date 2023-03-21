using BankApplicationModels;
namespace BankApplicationServices.Interfaces
{
    public interface IFileService
    {
        string ReadFile();
        void WriteFile(List<Bank> banks);
        List<Bank> GetData();
    }
}