using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Models;
using Xunit;

namespace TicketingSystem.Xunit.Tests.Domain
{
    public class TicketUnitTest
    {


        [Fact]
        public void CreateTicket_ValidSupportContract_TicketCreated()
        {
            
        }


        [Fact]
        public void CreateTicket_InvalidSupportContract_ThrowsException()
        {

            TicketType bug = new TicketType();
           // Assert.Throws<InvalidOperationException>(() => new Ticket("test","Dit is een testTicket",bug,));
        }





    }
}
