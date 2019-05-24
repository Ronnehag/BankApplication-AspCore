using System.ComponentModel.DataAnnotations;

namespace Bank.Application.Customers
{
    public class CustomerAdress
    {
        [Required]
        [Display(Name = "Street address")]
        public string StreetAdress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }
    }
}
