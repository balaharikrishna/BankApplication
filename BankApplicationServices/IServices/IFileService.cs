using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IFileService
    {
        List<Bank> GetData();
        string ReadFile();
        void WriteFile(List<Bank> banks);
    }
}