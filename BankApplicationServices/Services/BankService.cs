using BankApplicationModels;
using BankApplicationServices.IServices;
using Newtonsoft.Json;


namespace BankApplicationServices.Services
{
    public class BankService : IBankService
    {
        private readonly IFileService _fileService;
        List<Bank> banks;
        public BankService(IFileService fileService)
        {
            _fileService = fileService;
            banks = new List<Bank>();
        }

        public Message AuthenticateBankId(string bankId)
        {
            Message message = new();
            banks = _fileService.GetData();
            if (banks.Count > 0)
            {
                bool bank = banks.Any(b => b.BankId.Equals(bankId) && b.IsActive == 1);
                if (bank)
                {
                    message.Result = true;
                    message.ResultMessage = $"Bank Id:'{bankId}' Exist.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Bank Id:'{bankId}' Doesn't Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Banks Available";
            }
            return message;

        }
        public Message CreateBank(string bankName)
        {
            Message message = new();
            banks = _fileService.GetData();
            bool isBankAlreadyRegistered = false;

            isBankAlreadyRegistered = banks.Any(bank => bank.BankName.Equals(bankName));

            if (isBankAlreadyRegistered)
            {
                message.Result = false;
                message.ResultMessage = $"BankName:{bankName} is Already Registered.";
            }
            else
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string bankFirstThreeCharecters = bankName.Substring(0, 3);
                string bankId = bankFirstThreeCharecters + date + "M";

                Bank bank = new()
                {
                    BankName = bankName,
                    BankId = bankId,
                    IsActive = 1
                };
                banks.Add(bank);

                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Bank {bankName} is Created with {bankId}";
            }
            return message;
        }

        public Message UpdateBank(string bankId, string bankName)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                string bankNameReceived = banks[banks.FindIndex(bank => bank.BankId.Equals(bankId))].BankName;
                if (!bankNameReceived.Equals(bankName))
                {
                    bankNameReceived = bankName;
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"bankId :{bankId} is Updated with BankName : {bankName} Successfully.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"BankName :{bankName} Is Matching with the Existing BankName.,Please Change the Bank Name.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Authentication Failed";
            }

            return message;
        }

        public Message DeleteBank(string bankId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                banks[banks.FindIndex(bank => bank.BankId.Equals(bankId))].IsActive = 0;
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Bank Id :{bankId} Succesfully Deleted.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Authentication Failed";
            }
            return message;
        }

        public Message GetExchangeRates(string bankId)
        {
            Message message = new();
            banks = _fileService.GetData();
            Dictionary<string, decimal> exchangeRates = new();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bank => bank.BankId.Equals(bankId));
                if (bankIndex > -1)
                {
                    List<Currency> rates = banks[bankIndex].Currency;
                    if (rates is not null)
                    {
                        rates.FindAll(cu => cu.IsActive == 1);
                        for (int i = 0; i < rates.Count; i++)
                        {
                            exchangeRates.Add(rates[i].CurrencyCode, rates[i].ExchangeRate);
                        }
                        message.Data = JsonConvert.SerializeObject(exchangeRates);
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"No Currencies Available for BankId:{bankId}";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Authentication Failed";
            }
            return message;
        }
    }
}
