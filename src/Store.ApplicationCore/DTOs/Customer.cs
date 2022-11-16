using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Text;

namespace Store.ApplicationCore.DTOs
{
   
    public class CreateCustomerRequest
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public ulong PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(13, MinimumLength = 0)]
        [RegularExpression(@"^[0-9]{7,14}$")]  // USA Bank accnum
        public string BankAccountNumber { get; set; }

    }

    public class UpdateCustomerRequest : CreateCustomerRequest
    {
        public string Id { get; set; }
    }

    public class CustomerResponse
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public ulong PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }

    public class SingleCustomerResponse : CustomerResponse
    {
        public string Id { get; set; }
    }

}
