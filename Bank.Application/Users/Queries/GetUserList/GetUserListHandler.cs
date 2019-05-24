using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Users.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserViewModel>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public GetUserListQueryHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserViewModel> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            var model = new UserViewModel
            {
                Users = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Claims = _userManager.GetClaimsAsync(u).Result.Select(c => new ClaimDto {Type = c.Type}).ToList(),
                }).ToList()
            };
            return model;
        }
    }
}
