using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;


namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class CreateCommentCommandHandler : BaseCommandHandler<CreateCommentCommand, Comment>
    {

        public CreateCommentCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {

        }

        public async override Task<Comment> ExecuteCommandAsync(CreateCommentCommand request, CancellationToken cancellationToken)
        {

            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.TicketId});            

            if (ticket == null)
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);

            if (!string.IsNullOrEmpty(request.Text))
                throw new ValidationException(Constants.ERROR_EMPTY_COMMENT);

            var comment = new Comment() {Text = request.Text, CreatedBy = request.Client , DateAdded = request.DateAdded};

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return comment;

        }


    }
}
