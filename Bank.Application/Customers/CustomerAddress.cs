using System.ComponentModel.DataAnnotations;

namespace Bank.Application.Customers
{
    public class CustomerAddress
    {
        [Display(Name = "Street address")]
        public string StreetAdress { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Country code")]
        public string CountryCode { get; set; }
    }
}
