using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;


namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetCommentsByTicketIdQueryHandler : BaseQueryHandler<GetCommentsByTicketIdQuery, IQueryable<Comment>>
    {
        public GetCommentsByTicketIdQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {

        }


        public async override Task<IQueryable<Comment>> ExecuteCommandAsync(GetCommentsByTicketIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Comments          
                .Where(x => x.TicketId == request.Id);
        }
    }
}
