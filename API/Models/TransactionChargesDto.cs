using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class TransactionChargesDto
    {
        public ushort RtgsSameBank { get; set; }

        public ushort RtgsOtherBank { get; set; }

        public ushort ImpsSameBank { get; set; }

        public ushort ImpsOtherBank { get; set; }

        public bool IsActive { get; set; }
    }
}
