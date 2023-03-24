using BankApplicationModels;
using BankApplicationServices.IServices;


namespace BankApplicationServices.Services
{
    public class BankService : IBankService
    {
        IFileService _fileService;
        List<Bank> banks;
        Message message = new Message();
        public BankService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }

        public Message AuthenticateBankId(string bankId)
        {
            if (banks.Count < 0)
            {
                bool bank = banks.Any(b => b.BankId == bankId);
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

                Bank bank = new Bank() { BankName = bankName, BankId = bankId, IsActive = 1 };
                banks.Add(bank);

                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Bank {bankName} is Created with {bankId}";
            }
            return message;
        }

        public Message UpdateBank(string bankId, string bankName)
        {
            if (banks.Count < 0)
            {
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
                else
                {
                    message.Result = false;
                    message.ResultMessage = "BankId: {bankId} Doesn't Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Banks Available";
            }
            return message;
        }

        public Message DeleteBank(string bankId)
        {
            if (banks.Count < 0)
            {
                message = AuthenticateBankId(bankId);
                if (message.Result)
                {
                    banks[banks.FindIndex(bank => bank.BankId == bankId)].IsActive = 0;
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Bank Id :{bankId} Succesfully Deleted.";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "BankId: {bankId} Doesn't Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Banks Available";
            }
            return message;
        }

       

        public Message GetExchangeRates(string bankId)
        {
            Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();
            message = AuthenticateBankId(bankId);
            if (message.Result)
            {
                
                int bankIndex = banks.FindIndex(bank => bank.BankId == bankId);
                if (bankIndex > -1)
                {
                    List<Currency> rates = banks[bankIndex].Currency;
                    for (int i = 0; i < rates.Count; i++)
                    {
                        exchangeRates.Add(rates[i].CurrencyCode, rates[i].ExchangeRate);
                        message.Data = exchangeRates.ToString();
                    }
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
                message.ResultMessage = $"BankId :{bankId} Is Not Available;";
            }

            return message;
        }
    }
}
