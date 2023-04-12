using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class HeadManagerDto
    {
       
        public string? Name { get; set; }

        public byte[]? Salt { get; set; }

        public byte[]? HashedPassword { get; set; }

        public string? AccountId { get; set; }

        public bool IsActive { get; set; }
    }
}
