using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.RazorWebsite.Controllers;
using TicketingSystem.Xunit.Tests.Data;

namespace TicketingSystem.Xunit.Tests.Controllers
{
    public class TicketControllerTest
    {
        private readonly TicketController _controller;
        private readonly Mock<IMediator> _mediatr;        
        private readonly DummyApplicationDbContext _dummyContext;



    }
}
