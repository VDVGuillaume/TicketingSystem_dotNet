using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetEngineerUsersQueryHandler : BaseQueryHandler<GetEngineerUsersQuery, IQueryable<ApplicationUser>>
    {
        public GetEngineerUsersQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<ApplicationUser>> ExecuteCommandAsync(GetEngineerUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _dbContext.Users;
            var userRoles = _dbContext.UserRoles;
            var roles = _dbContext.Roles;

            var user_userRole = users.Join(
                userRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    User = u, ur.RoleId
                });
            var user_userRole_role = user_userRole.Join(
                roles,
                uur => uur.RoleId,
                r => r.Id,
                (uur, r) => new
                {
                    uur.User, RoleName = r.Name
                });

            return user_userRole_role.Where(x => x.RoleName == "Technician").Select(x => x.User).OrderBy(x => x.UserName);
        }
    }
}
