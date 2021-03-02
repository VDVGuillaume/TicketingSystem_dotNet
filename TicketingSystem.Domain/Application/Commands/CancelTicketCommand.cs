using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CancelTicketCommand : BaseCommand<Ticket>
    {
        public int Ticketnr;
    }
}
