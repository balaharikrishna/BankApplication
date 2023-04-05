namespace BankApplicationModels
{
    public class TransactionCharges
    {
        public ushort RtgsSameBank { get; set; }
        public ushort RtgsOtherBank { get; set; }
        public ushort ImpsSameBank { get; set; }
        public ushort ImpsOtherBank { get; set; }
        public ushort IsActive { get; set; }
        public override string ToString()
        {
            return $"TransactionCharges: RtgsSameBank:{RtgsSameBank}% , RtgsOtherBank:{RtgsOtherBank}% , ImpsSameBank:{ImpsSameBank}% , ImpsOtherBank:{ImpsOtherBank}%";
        }
    }


}
