
using BankApplicationModels;
using BankApplicationServices.IServices;
using System.Collections.Generic;
using System.Text.Json;

namespace BankApplicationServices.Services
{
    public class FileService : IFileService
    {
        private static string CheckFile()
        {
            string filePath = Path.ChangeExtension(Path.Combine("C:\\Core\\BankApplication\\BankDetails"), ".json");
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                return filePath;
            }
            else
            {
                return filePath;
            }
        }
        public string ReadFile()
        {
            string jsonData = string.Empty;
            if (CheckFile() != null)
            {
                jsonData = File.ReadAllText(CheckFile());
            }
            return jsonData;
        }

        public void WriteFile(List<Bank> banks)
        {
            string createBankJson = JsonSerializer.Serialize(banks);
            File.WriteAllText(CheckFile(), createBankJson);
            GetData();
        }

        public List<Bank> GetData()
        {
            List<Bank> data;
            if(ReadFile() != null && ReadFile() != string.Empty)
            {
                data =  JsonSerializer.Deserialize<List<Bank>>(ReadFile()) ?? new List<Bank>();
            }
            else
            {
                data = new List<Bank>();
                WriteFile(data);
                data = JsonSerializer.Deserialize<List<Bank>>(ReadFile()) ?? new List<Bank>();
            }
            return data;
        }
    }
}
