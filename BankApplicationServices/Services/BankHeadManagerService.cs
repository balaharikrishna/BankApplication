using BankApplicationModels;
using BankApplicationServices.Interfaces;

namespace BankApplicationServices.Services
{
    public class BankHeadManagerService : IBankHeadManagerService
    {
        IFileService _fileService;
        List<Bank> banks;
        public BankHeadManagerService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }

        private static int bankObjectIndex = 0;
        Message message = new Message();
        public Message ValidateBankHeadManager(string bankName, string bankId, string headManagerAccountId, string headManagerPassword)
        {
            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankName == bankName && b.BankId == bankId);
                if (bank != null)
                {
                    var headManager = bank.HeadManagers?.FirstOrDefault(hm => hm.HeadManagerAccountId == headManagerAccountId && hm.HeadManagerPassword == headManagerPassword);
                    if (headManager != null)
                    {
                        bankObjectIndex = banks.IndexOf(bank);
                        message.Result = true;
                        message.ResultMessage = "Head Manager Validation Successful.";
                    }
                }
            }
            return message;
        }
        public Message CreateBankBranch(string branchName, string branchPhoneNumber, string branchAddress)
        {
            bool isBranchAlreadyRegistered = false;
            if (banks[bankObjectIndex].Branches == null)
            {
                banks[bankObjectIndex].Branches = new List<BankBranch>();
            }

            isBranchAlreadyRegistered = banks[bankObjectIndex].Branches.Any(branch => branch.BranchName == branchName);

            if (isBranchAlreadyRegistered)
            {
                message.Result = false;
                message.ResultMessage = $"Branch:{branchName} already Registered";
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                string branchNameFirstThreeCharecters = branchName.Substring(0, 3);
                string branchId = branchNameFirstThreeCharecters + date;

                BankBranch bankBranch = new BankBranch();
                {
                    bankBranch.BranchName = branchName;
                    bankBranch.BranchId = branchId;
                    bankBranch.BranchAddress = branchAddress;
                    bankBranch.BranchPhoneNumber = branchPhoneNumber;
                }

                banks[bankObjectIndex].Branches.Add(bankBranch);
                _fileService.WriteFile(banks);

                message.Result = true;
                message.ResultMessage = $"Branch Created Successfully with BranchName:{branchName} BranchId:{branchId}";


            }
            return message;
        }

        public Message OpenBranchManagerAccount(string branchid, string branchManagerName, string branchManagerPassword)
        {
            bool isBranchManagerAlreadyRegistered = false;
            int branchObjectIndex;

            branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.BranchId == branchid);
            if (banks[bankObjectIndex].Branches[branchObjectIndex].Managers == null)
            {
                banks[bankObjectIndex].Branches[branchObjectIndex].Managers = new List<BranchManager>();
            }

            isBranchManagerAlreadyRegistered = banks[bankObjectIndex].Branches[branchObjectIndex].Managers.Any(ma => ma.BranchManagerName == branchManagerName);
            if (isBranchManagerAlreadyRegistered)
            {
                message.Result = false;
                message.ResultMessage = $"Branch Manager {branchManagerName} is already Registered";
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                string UserFirstThreeCharecters = branchManagerName.Substring(0, 3);
                string branchManagerAccountId = UserFirstThreeCharecters + date;

                BranchManager branchManager = new BranchManager()
                {
                    BranchManagerAccountId = branchManagerAccountId,
                    BranchManagerName = branchManagerName,
                    BranchMangerPassword = branchManagerPassword
                };


                banks[bankObjectIndex].Branches[branchObjectIndex].Managers.Add(branchManager);
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Branch Manager Account Opened with AccountId:{branchManagerAccountId}";
            }
            return message;
        }

        //public Message CheckCurrencyExistance(string currencyCode)
        //{
        //    bool isCurrencyCodeExist =  banks[bankObjectIndex].Currency.Any(cur => cur.CurrencyCode == currencyCode);
        //    if (isCurrencyCodeExist)
        //    {
        //        message.Result = true;
        //        message.ResultMessage = "Currency:{currencyCode} Already Exists.";
        //    }

        //    return message;
        //}   

        public Message AddCurrencyWithExchangeRate(string currencyCode, decimal exchangeRate)
        {

            Currency currency = new Currency()
            {
                ExchangeRate = exchangeRate,
                CurrencyCode = currencyCode
            };

            if (banks[bankObjectIndex].Currency == null)
            {
                banks[bankObjectIndex].Currency = new List<Currency>();
            }

            banks[bankObjectIndex].Currency.Add(currency);
            _fileService.WriteFile(banks);
            message.Result = true;
            message.ResultMessage = $"Added Currency Code:{currencyCode} with Exchange Rate:{exchangeRate}";
            return message;
        }
    }
}
