using API.ViewModels.Branch;

namespace API.ViewModels.Bank
{
    public class UpdateBranchViewModel 
    {
        public string? BranchName { get; set; }
        public string? BranchId { get; set; }
        public string? BranchAddress { get; set; }
        public string? BranchPhoneNumber { get; set; }
    }
}
