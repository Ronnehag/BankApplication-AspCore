using System.Collections.Generic;
using MediatR;

namespace Bank.Application.Users.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<UserViewModel>
    {
    }
}
