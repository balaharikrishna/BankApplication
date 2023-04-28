using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class Bank
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Required]
        public string BankId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string BankName { get; set; }

        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }
    }
}
