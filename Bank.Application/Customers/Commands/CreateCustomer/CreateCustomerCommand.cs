using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Bank.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<bool>
    {
        [Required]
        [MaxLength(6)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "First name")]
        public string GivenName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Last name")]
        public string Surname { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Street address")]
        public string Streetadress { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Zip code")]
        public string Zipcode { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [MaxLength(2)]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        [Display(Name = "National ID")]
        public string NationalId { get; set; }

        [MaxLength(10)]
        [Display(Name = "Phone country code")]
        public string TelephoneCountryCode { get; set; }

        [MaxLength(25)]
        [Display(Name = "Phone number")]
        public string TelephoneNumber { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string EmailAdress { get; set; }




    }
}
