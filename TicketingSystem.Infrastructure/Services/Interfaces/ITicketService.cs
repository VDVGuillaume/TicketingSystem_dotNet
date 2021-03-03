using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.Services.Interfaces
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicket(CreateTicketCommand request);
        Task<Ticket> UpdateTicket(UpdateTicketCommand request);
        Task<Ticket> CancelTicket(CancelTicketCommand request);
        Task<Comment> PostComment(PostCommentCommand request);
    }
}
