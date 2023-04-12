using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class BankDto
    {
        public string? BankName { get; set; }
        
        public string? BankId { get; set; }
       
        public bool IsActive { get; set; }
    }
}
