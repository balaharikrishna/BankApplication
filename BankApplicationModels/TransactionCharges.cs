using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels
{
    public class TransactionCharges
    {
        public ushort RtgsSameBank { get; set; }
        public ushort RtgsOtherBank { get; set; }
        public ushort ImpsSameBank { get; set; }
        public ushort ImpsOtherBank { get; set; }
        public ushort IsDeleted { get; set; }
        public override string ToString()
        {
            return $"TransactionCharges: RtgsSameBank:{RtgsSameBank}% , RtgsOtherBank:{RtgsOtherBank}% , ImpsSameBank:{ImpsSameBank}% , ImpsOtherBank:{ImpsOtherBank}%";
        }
    }


}
