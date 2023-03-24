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
        public Message AddTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            if (branchObjectIndex != -1)
            {
                TransactionCharges transactionCharges = new TransactionCharges()
                {
                    RtgsSameBank = rtgsSameBank,
                    RtgsOtherBank = rtgsOtherBank,
                    ImpsSameBank = impsSameBank,
                    ImpsOtherBank = impsOtherBank,

                };

                if (banks[bankObjectIndex].Branches[branchObjectIndex].Charges == null)
                {
                    banks[bankObjectIndex].Branches[branchObjectIndex].Charges = new List<TransactionCharges>();
                }

                banks[bankObjectIndex].Branches[branchObjectIndex].Charges.Add(transactionCharges);
                _fileService.WriteFile(banks);

                message.Result = true;
            }
            return message;
        }

        public Message UpdateTransactionCharges(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {

        }

        public Message DeleteTransactionCharges(string bankId, string branchId)
        {

        }
    }
}
