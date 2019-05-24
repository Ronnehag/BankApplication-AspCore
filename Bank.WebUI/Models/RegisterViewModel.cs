using System.Collections.Generic;
using Bank.Application.Enumerations;
using Bank.Application.Users.Commands.RegisterUser;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.WebUI.Models
{
    public class RegisterViewModel
    {
        public RegisterUserCommand Command { get; set; }

        public List<SelectListItem> RoleSelect { get; set; }

        public RegisterViewModel()
        {
            RoleSelect = new List<SelectListItem>
            {
                new SelectListItem("Select role", string.Empty, true, true),
                new SelectListItem(Claims.Admin, Claims.Admin),
                new SelectListItem(Claims.Cashier, Claims.Cashier),
            };

        }


    }
}
