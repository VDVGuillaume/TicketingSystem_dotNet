using System.Linq;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetCommentsByTicketIdQuery : BaseQuery<IQueryable<Comment>>
    {
        public int Id { get; set; }
    }
}
