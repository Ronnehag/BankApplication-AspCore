using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Bank.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<RegisterUserResult>
    {

        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string SelectedRole { get; set; }
    }
}
