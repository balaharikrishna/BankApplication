using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class TransactionCharges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ushort RtgsSameBank { get; set; }
        [Required]
        public ushort RtgsOtherBank { get; set; }
        [Required]
        public ushort ImpsSameBank { get; set; }
        [Required]
        public ushort ImpsOtherBank { get; set; }
        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("Branch")]
        public string BranchId { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
