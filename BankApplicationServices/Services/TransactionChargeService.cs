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
            Message message = new Message();
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
                        if(charges is null)
                        {
                            charges= new List<TransactionCharges>();
                        }
                       
                        var chargesList = charges.FindAll(c => c.IsDeleted == 1);
                        if (chargesList.Count == 1)
                        {
                            message.Result = true;
                            message.ResultMessage = "Charges Already Available";
                        }
                        else
                        {
                            TransactionCharges transactionCharges = new TransactionCharges()
                            {
                                RtgsSameBank = rtgsSameBank,
                                RtgsOtherBank = rtgsOtherBank,
                                ImpsSameBank = impsSameBank,
                                ImpsOtherBank = impsOtherBank,
                                IsDeleted = 0
                            };

                            charges.Add(transactionCharges);
                            branch.Charges = charges;
                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = $"Transaction Charges Added Successfully";
                        }

                    }
                }
            }
            return message;
        }

        public Message UpdateTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        if (branch.Charges == null)
                        {
                            branch.Charges = new List<TransactionCharges>();
                        }
                       
                        if (branch.Charges.Count == 1)
                        {
                            var charges = branch.Charges.Find(c=>c.IsDeleted == 0);
                            if(rtgsOtherBank != 101 && charges is null)
                            {
                                charges.RtgsOtherBank = rtgsOtherBank;
                            }

                            if (rtgsSameBank != 101 && charges != null)
                            {
                                charges.RtgsSameBank = rtgsSameBank;
                            }

                            if (impsSameBank != 101 && charges != null)
                            {
                                charges.ImpsSameBank = impsSameBank;
                            }

                            if (impsOtherBank != 101 && charges != null )
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
                }
            }
            return message;
        }

        public Message DeleteTransactionCharges(string bankId, string branchId)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        if (branch.Charges == null)
                        {
                            branch.Charges = new List<TransactionCharges>();
                        }

                        if (branch.Charges.Count == 1)
                        {
                            var  transactionCharges = branch.Charges.Find(c => c.IsDeleted == 0);
                            if(transactionCharges != null)
                            {
                                transactionCharges.IsDeleted = 1;
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
                }
            }
            return message;
        }
    }
}
