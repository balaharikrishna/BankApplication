using BankApplicationModels;
using BankApplicationServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            banks = _fileService.GetData();
        }
        Message message= new Message(); 
        public Message AddTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<TransactionCharges> charges = branch.Charges;
                        if (charges.Count > 0)
                        { 
                            message.Result = false;
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

                            };

                            if (branch.Charges == null)
                            {
                                branch.Charges = new List<TransactionCharges>();
                            }

                            charges.Add(transactionCharges);
                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = $"Transaction Charges RtgsSameBank:{rtgsSameBank}, RtgsOtherBank:{rtgsOtherBank}, ImpsSameBank:{impsSameBank}, ImpsOtherBank:{impsOtherBank} Added Successfully";
                        }

                    }
                }
            }
            return message;
        }

        public Message UpdateTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
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
                            TransactionCharges charges = branch.Charges[0];
                            if(charges.RtgsOtherBank != 0)
                            {
                                charges.RtgsOtherBank = rtgsOtherBank;
                            }

                            if (charges.RtgsSameBank != 0)
                            {
                                charges.RtgsSameBank = rtgsSameBank;
                            }

                            if (charges.ImpsOtherBank != 0)
                            {
                                charges.ImpsOtherBank = impsOtherBank;
                            }

                            if (charges.ImpsSameBank != 0)
                            {
                                charges.ImpsSameBank = impsOtherBank;
                            }

                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = $"Transaction Charges RtgsSameBank:{rtgsSameBank}, RtgsOtherBank:{rtgsOtherBank}, ImpsSameBank:{impsSameBank}, ImpsOtherBank:{impsOtherBank} Added Successfully";
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
                            branch.Charges.RemoveAt(0);
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
