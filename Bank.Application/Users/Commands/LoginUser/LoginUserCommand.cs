using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Bank.Application.Users.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserResult>
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
