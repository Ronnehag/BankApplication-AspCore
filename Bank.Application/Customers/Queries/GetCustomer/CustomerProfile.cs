using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.Customers.Queries.GetCustomer
{
    public class CustomerProfile
    {
        public int CustomerId { get; set; }

        [Display(Name = "First name")]
        public string GivenName { get; set; }

        [Display(Name = "Last name")]
        public string Surname { get; set; }

        [Display(Name = "National ID")]
        public string NationalId { get; set; }

        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }
        
        [Display(Name = "Phone country code")]
        public string TelephoneCountryCode { get; set; }
        
        [Display(Name = "Email adress")]
        [DataType(DataType.EmailAddress)]
        public string EmailAdress { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public CustomerAddress Address { get; set; }
    }
}
