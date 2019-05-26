using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Users.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<IResult>
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = " Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
