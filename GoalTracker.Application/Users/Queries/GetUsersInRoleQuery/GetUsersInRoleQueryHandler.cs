using GoalTracker.Application.Users.Dtos;
using GoalTracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.Users.Queries.GetUsersInRoleQuery
{
    public class GetUsersInRoleQueryHandler : IRequestHandler<GetUsersInRoleQuery, List<UserDto>>
    {
        private readonly UserManager<User> _userManager;

        public GetUsersInRoleQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDto>> Handle(GetUsersInRoleQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.GetUsersInRoleAsync(request.Role);
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.UserName,
                Email = u.Email
            }).ToList();
        }
    }
}