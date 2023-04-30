using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class User
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[] Salt { get; set; }

        [Required]
        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[] HashedPassword { get; set; }

        [Key]
        [Required]
        [StringLength(17)]
        [Column(TypeName = "varchar")]
        public string AccountId { get; set; }

        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }

        [Required]
        [Range(1, 5)]
        [Column(TypeName = "Smallint")]
        public Roles Role { get; set; }

    }
}
