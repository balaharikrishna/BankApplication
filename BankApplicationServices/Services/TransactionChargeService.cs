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
        Message message = new Message();
        List<Bank> banks;
        public TransactionChargeService(IFileService fileService,IBranchService branchService) {
            _fileService = fileService;
            _branchService = branchService;
        }

        public List<Bank> GetBankData()
        {
            if (_fileService.GetData() != null)
            {
                banks = _fileService.GetData();
            }
            return banks;
        }
        public Message AddTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
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
                        List<TransactionCharges> charges = branch.Charges;
                        if(charges == null)
                        {
                            charges= new List<TransactionCharges>();
                        }
                        if (charges.Count > 0)
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
                            };

                            charges.Add(transactionCharges);
                            branch.Charges = charges;
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
                            TransactionCharges charges = branch.Charges[0];
                            if(rtgsOtherBank != 101)
                            {
                                charges.RtgsOtherBank = rtgsOtherBank;
                            }

                            if (rtgsSameBank != 101)
                            {
                                charges.RtgsSameBank = rtgsSameBank;
                            }

                            if (impsSameBank != 101)
                            {
                                charges.ImpsSameBank = impsSameBank;
                            }

                            if (impsOtherBank != 101)
                            {
                                charges.ImpsOtherBank = impsOtherBank;
                            }

                            _fileService.WriteFile(banks);
                            message.Result = true;
                            message.ResultMessage = $"Transaction Charges RtgsSameBank:{charges.RtgsSameBank}, RtgsOtherBank:{charges.RtgsOtherBank}, ImpsSameBank:{charges.ImpsSameBank}, ImpsOtherBank:{charges.ImpsOtherBank} Updated Successfully";
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
