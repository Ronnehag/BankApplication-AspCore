using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.Customers.Queries.GetCustomer
{
    public class CustomerProfile
    {
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string GivenName { get; set; }

        [Display(Name = "Last name")]
        public string Surname { get; set; }

        [Display(Name = "National ID")]
        public string NationalId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Phone number")]
        public string TelephoneNumber { get; set; }
        
        [Display(Name = "Phone country code")]
        public string TelephoneCountryCode { get; set; }
        
        [Display(Name = "Email adress")]
        public string EmailAdress { get; set; }

        public CustomerAdress Adress { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }
}
