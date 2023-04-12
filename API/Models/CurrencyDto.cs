using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CurrencyDto
    {
        public string? CurrencyCode { get; set; }
       
        public decimal ExchangeRate { get; set; }
      
        public bool IsActive { get; set; }
    }
}
