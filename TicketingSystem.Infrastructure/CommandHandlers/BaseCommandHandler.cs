using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public abstract class BaseCommandHandler<TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse>
        where TCommand : BaseCommand<TCommandResponse>
        where TCommandResponse : class
    {
        protected readonly IMediator _mediator;
        protected readonly TicketingSystemDbContext _dbContext;

        public BaseCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext)
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
