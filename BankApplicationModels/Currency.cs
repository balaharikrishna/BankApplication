using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class Currency
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public decimal ExchangeRate { get; set; }
        
        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }

        [ForeignKey("Bank")]
        public string BankId { get; set; }

        public virtual Bank Bank { get; set; }


        public string DefaultCurrencyCode = "INR";

        public short DefaultCurrencyExchangeRate = 1;

    }
}
