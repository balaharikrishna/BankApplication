using BankApplicationModels;
using BankApplicationServices.IServices;


namespace BankApplicationServices.Services
{
    public class TransactionChargeService : ITransactionChargeService
    {
        private readonly IBranchService _branchService;
        private readonly IFileService _fileService;
        
        List<Bank> banks;
        public TransactionChargeService(IFileService fileService,IBranchService branchService) {
            _fileService = fileService;
            _branchService = branchService;
            banks = new List<Bank>();
        }

        public Message AddTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<TransactionCharges> charges = branch.Charges;
                        charges??= new List<TransactionCharges>();
                       
                        var chargesList = charges.FindAll(c => c.IsActive == 1);
                        if (chargesList.Count.Equals(1))
                        {
                            message.Result = true;
                            message.ResultMessage = "Charges Already Available";
                        }
                        else
                        {
                            TransactionCharges transactionCharges = new ()
                            {
                                RtgsSameBank = rtgsSameBank,
                                RtgsOtherBank = rtgsOtherBank,
                                ImpsSameBank = impsSameBank,
                                ImpsOtherBank = impsOtherBank,
                                IsActive = 1
                            };

                            charges.Add(transactionCharges);
                            branch.Charges = charges;
                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = $"Transaction Charges Added Successfully";
                        }

                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Branch Authentiaction Failed";
            }
            return message;
        }

        public Message UpdateTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        branch.Charges ??= new List<TransactionCharges>();
                        if (branch.Charges.Count==1)
                        {
                            var charges = branch.Charges.Find(c=>c.IsActive == 1);
                            if(rtgsOtherBank is not 101 && charges is not null)
                            {
                                charges.RtgsOtherBank = rtgsOtherBank;
                            }

                            if (rtgsSameBank is not 101 && charges is not null)
                            {
                                charges.RtgsSameBank = rtgsSameBank;
                            }

                            if (impsSameBank is not 101 && charges is not null)
                            {
                                charges.ImpsSameBank = impsSameBank;
                            }

                            if (impsOtherBank is not 101 && charges is not null )
                            {
                                charges.ImpsOtherBank = impsOtherBank;
                            }

                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = "Transaction Charges Updated Successfully";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "No Charges Available to Update";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Branch Authentiaction Failed";
            }
        
            return message;
        }

        public Message DeleteTransactionCharges(string bankId, string branchId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        branch.Charges ??= new List<TransactionCharges>();
                        if (branch.Charges.Count.Equals(1))
                        {
                            var  transactionCharges = branch.Charges.Find(c => c.IsActive ==1);
                            if(transactionCharges is not null)
                            {
                                transactionCharges.IsActive = 0;
                            }
                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = "Charges Deleted Successfully";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "No Charges Available to Delete";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Branch Authentiaction Failed";
            }
            return message;
        }
    }
}
