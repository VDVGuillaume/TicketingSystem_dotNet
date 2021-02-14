using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetUserLoginAttemptsByUsernameQueryHandler : BaseQueryHandler<GetUserLoginAttemptsByUsernameQuery, IQueryable<UserLoginAttempt>>
    {
        public GetUserLoginAttemptsByUsernameQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<UserLoginAttempt>> ExecuteCommandAsync(GetUserLoginAttemptsByUsernameQuery request, CancellationToken cancellationToken)
        {
            if(request.AmountToFetch.HasValue)
                return _dbContext.UserLoginAttempts.Where(x => x.Username == request.Username).OrderByDescending(x => x.Id).Take(request.AmountToFetch.Value).AsQueryable();
            return _dbContext.UserLoginAttempts.Where(x => x.Username == request.Username).OrderByDescending(x => x.Id).AsQueryable();
        }
    }
}
