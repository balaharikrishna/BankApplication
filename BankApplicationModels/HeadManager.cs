using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class HeadManager 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public byte[] Salt { get; set; }

        [Required]
        public byte[] HashedPassword { get; set; }

        [Key]
        [Required]
        public string AccountId { get; set; }

        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }

        public Roles Role = Roles.HeadManager;

        [ForeignKey("Branch")]
        public string BranchId { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
