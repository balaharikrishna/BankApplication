using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("TransactionCharges")]
    public class TransactionCharges
    {
        [Required]
        [Range(1,100)]
        [Column(TypeName = "Smallint")]
        public ushort RtgsSameBank { get; set; }

        [Required]
        [Range(1,100)]
        [Column(TypeName = "Smallint")]
        public ushort RtgsOtherBank { get; set; }

        [Required]
        [Range(1,100)]
        [Column(TypeName = "Smallint")]
        public ushort ImpsSameBank { get; set; }

        [Required]
        [Range(1,100)]
        [Column(TypeName = "Smallint")]
        public ushort ImpsOtherBank { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("Branch")]
        [StringLength(17)]
        [Column(TypeName = "varchar")]
        public string BranchId { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
