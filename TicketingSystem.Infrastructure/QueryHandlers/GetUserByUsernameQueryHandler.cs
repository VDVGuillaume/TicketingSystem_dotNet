using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetUserByUsernameQueryHandler : BaseQueryHandler<GetUserByUsernameQuery, ApplicationUser>
    {
        public GetUserByUsernameQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<ApplicationUser> ExecuteCommandAsync(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Users.FirstOrDefault(x => x.UserName == request.Username);
        }
    }
}
