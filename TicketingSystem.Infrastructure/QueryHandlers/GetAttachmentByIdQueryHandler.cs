using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetAttachmentByIdQueryHandler : BaseQueryHandler<GetAttachmentByIdQuery, Attachment>
    {
        public GetAttachmentByIdQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Attachment> ExecuteCommandAsync(GetAttachmentByIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Attachments.FirstOrDefault(x => x.AttachmentId == request.AttachmentId);
        }
    }
}
