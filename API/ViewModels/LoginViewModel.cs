using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string AccountId { get; set; }
       
        [Required]
        public string Password { get; set; }
    }
}
