using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Branches")]
    public class Branch
    {
        [Key]
        [Required]
        [StringLength(17)]
        [Column(TypeName = "varchar")]
        public string BranchId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string BranchName { get; set; }
        
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string BranchAddress { get; set; }

        [Required]
        [RegularExpression("^\\d{10}$")]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string BranchPhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("Bank")]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string BankId { get; set; }

        public virtual Bank Bank { get; set; }
    }
}
