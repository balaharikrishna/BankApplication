﻿using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Customers")]
    public class Customer : HeadManager
    {

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [RegularExpression("^\\d{10}$")]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string EmailId { get; set; }

        [Required]
        [Range(1, 2)]
        [Column(TypeName = "Smallint")]
        public AccountType AccountType { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$")]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string DateOfBirth { get; set; }

        [Required]
        [Range(1, 3)]
        [Column(TypeName = "Smallint")]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string PassbookIssueDate { get; set; }

        [Required]
        [Range(1, 5)]
        [Column(TypeName = "Smallint")]
        public new Roles Role = Roles.Customer;
    }
}
