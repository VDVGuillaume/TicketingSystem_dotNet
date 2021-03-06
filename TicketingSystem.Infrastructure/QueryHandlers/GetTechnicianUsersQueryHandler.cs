using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetTechnicianUsersQueryHandler : BaseQueryHandler<GetTechnicianUsersQuery, IQueryable<ApplicationUser>>
    {
        public GetTechnicianUsersQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<ApplicationUser>> ExecuteCommandAsync(GetTechnicianUsersQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Users;
        }
    }
}
