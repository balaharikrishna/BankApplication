using API.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class StaffDto : HeadManagerDto
    {
        public StaffRole Role { get; set; }
    }
}
