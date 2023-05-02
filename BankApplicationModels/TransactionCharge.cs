using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("TransactionCharges")]
    public class TransactionCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        [StringLength(18)]
        [Column(TypeName = "varchar")]
        public string BranchId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}
