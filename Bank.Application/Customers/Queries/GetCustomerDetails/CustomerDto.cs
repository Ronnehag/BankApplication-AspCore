using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Application.Accounts.Queries.GetCustomerAccounts;
using Bank.Application.Cards;
using Bank.Common.Extensions;

namespace Bank.Application.Customers.Queries.GetCustomerDetails
{
    public class CustomerDto
    {
        private string _gender;
        public int CustomerId { get; set; }
        public string FullName => GivenName + " " + Surname;
        public string WritePhoneNumber => $"({TelephoneCountryCode}) {TelephoneNumber}";
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string NationalId { get; set; }
        public DateTime? Birthday { get; set; }
        public string TelephoneNumber { get; set; }
        public string TelephoneCountryCode { get; set; }
        public string EmailAdress { get; set; }



        public string Gender
        {
            get => _gender.ToFirstLetterUpper();
            set => _gender = value;
        }

        public CustomerAddress Address { get; set; }

        public string PrintBirthday()
        {
            return Birthday.HasValue ? Birthday.Value.ToString("yyyy-MM-dd") : "";
        }

        public string TotalBalance => Accounts.Sum(a => a.Balance).ToSwedishKrona();
        public IEnumerable<CustomerAccountDto> Accounts { get; set; } = new List<CustomerAccountDto>();
        public IEnumerable<CardDto> Cards { get; set; } = new List<CardDto>();
    }




}
