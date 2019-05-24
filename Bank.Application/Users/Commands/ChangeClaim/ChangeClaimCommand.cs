using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Users.Commands.ChangeClaim
{
    public class ChangeClaimCommand : IRequest<IResult>
    {
        public string UserId { get; set; }
        public string NewClaim { get; set; }
        public string CurrentClaim { get; set; }
    }
}
