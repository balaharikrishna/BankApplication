using BankApplicationModels;
using Newtonsoft.Json;
using BankApplicationServices.IServices;


namespace BankApplicationServices.Services
{
    public class BankService : IBankService
    {
        private readonly IFileService _fileService;
        List<Bank> banks;
        public BankService(IFileService fileService)
        {
            _fileService = fileService;
        }
        public List<Bank> GetBankData()
        {
            if (_fileService.GetData() != null)
            {
                banks = _fileService.GetData();
            }
            return banks;
        }
        Message message = new Message();
        public Message AuthenticateBankId(string bankId)
        {
            GetBankData();
            if (banks.Count > 0)
            {
                bool bank = banks.Any(b => b.BankId == bankId && b.IsActive == 1);
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
            GetBankData();
            bool isBankAlreadyRegistered = false;

            isBankAlreadyRegistered = banks.Any(bank => bank.BankName == bankName);

            if (isBankAlreadyRegistered)
            {
                message.Result = false;
                message.ResultMessage = $"BankName:{bankName} is Already Registered.";
            }
            else
            {
                DateTime currentDate = DateTime.Today;
                string date = currentDate.ToString().Substring(0, 10).Replace("-", "");
                string bankFirstThreeCharecters = bankName.Substring(0, 3);
                string bankId = bankFirstThreeCharecters + date + "M";

                Bank bank = new Bank() {
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
            GetBankData();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                string bankNameReceived = banks[banks.FindIndex(bank => bank.BankId == bankId)].BankName;
                if (bankNameReceived != bankName)
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

            return message;
        }

        public Message DeleteBank(string bankId)
        {
            GetBankData();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                banks[banks.FindIndex(bank => bank.BankId == bankId)].IsActive = 0;
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Bank Id :{bankId} Succesfully Deleted.";
            }
            return message;
        }

        public Message GetExchangeRates(string bankId)
        {
            GetBankData();
            Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bank => bank.BankId == bankId );
                if (bankIndex > -1)
                {
                    List<Currency> rates = banks[bankIndex].Currency.FindAll(cu=>cu.IsDeleted == 0);
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
            return message;
        }
    }
}
