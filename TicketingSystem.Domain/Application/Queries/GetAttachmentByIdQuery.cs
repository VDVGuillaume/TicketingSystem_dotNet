using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetAttachmentByIdQuery : BaseQuery<Attachment>
    {
        public int AttachmentId { get; set; }
    }
}
