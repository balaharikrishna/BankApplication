using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class BranchDto
    {
        public string? BranchName { get; set; }
        public string? BranchId { get; set; }
        public string? BranchAddress { get; set; }
        public string? BranchPhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
