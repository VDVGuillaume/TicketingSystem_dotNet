using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetClientByUserQuery : BaseQuery<Client>
    {
        public string Username { get; set; }
    }
}
