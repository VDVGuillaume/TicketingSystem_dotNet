using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public abstract class BaseQueryHandler<TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse>
        where TCommand : BaseQuery<TCommandResponse>
        where TCommandResponse : class
    {
        protected readonly IMediator _mediator;
        protected readonly TicketingSystemDbContext _dbContext;

        public BaseQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException();
            _dbContext = dbContext ?? throw new ArgumentNullException();
        }

        public virtual Task<TCommandResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return ExecuteCommandAsync(request, cancellationToken);
        }
        public abstract Task<TCommandResponse> ExecuteCommandAsync(TCommand request, CancellationToken cancellationToken);
    }
}
