using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Required]
        public string BranchId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string BranchName { get; set; }
        
        [Required]
        public string BranchAddress { get; set; }
        [Required]
        [RegularExpression("^\\d{10}$")]
        public string BranchPhoneNumber { get; set; }
        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }

        [ForeignKey("Bank")]
        public string BankId { get; set; }

        public virtual Bank Bank { get; set; }
    }
}
