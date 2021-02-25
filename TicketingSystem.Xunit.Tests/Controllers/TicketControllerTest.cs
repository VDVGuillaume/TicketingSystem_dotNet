using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Controllers;
using TicketingSystem.RazorWebsite.Models.Tickets;
using TicketingSystem.Xunit.Tests.Data;
using TicketingSystem.Xunit.Tests.Factories;
using TicketingSystem.Xunit.Tests.FakeObjects;
using Xunit;

namespace TicketingSystem.Xunit.Tests.Controllers
{
    public class TicketControllerTest
    {
        private readonly TicketController _controller;
        private readonly Mock<IMediator> _mediatr;        
        private readonly DummyApplicationDbContext _dummyContext;

        [Theory]
        [InlineData("customer", "title", "Aangemaakt", "description", "customer")]
        public async void Create_Ticket_Customer_Should_Be_Successful(string username, string title, string type, string description, string clientUsername)
        {
            //arrange
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<TicketViewModel>>();
            var usermanagerMock = new Mock<FakeUserManager>();
            var mapper = MapperTestFactory.GenerateMapper();
            var userMock = new Mock<ClaimsPrincipal>();

            userMock
                .Setup(x => x.IsInRole("Customer"))
                .Returns(true);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userMock.Object
                }
            };
            usermanagerMock
                .Setup(x => x.GetUserAsync(userMock.Object))
                .Returns(Task.FromResult(new IdentityUser(username)));

            CreateTicketCommand cmdSaved = null;
            mediatorMock.Setup(x =>
            x.Send(It.IsAny<CreateTicketCommand>(),
            It.IsAny<CancellationToken>()))
                .Callback<IRequest<Ticket>, CancellationToken>((ticket, token) => cmdSaved = ticket as CreateTicketCommand);

            var model = new TicketViewModel();

            model.Input = new TicketViewModel.InputModel
            {
                Title = title,
                Type = type,
                Description = description,
                ClientUsername = clientUsername
            };
            model.ReturnUrl = "~/Ticket/Index";

            //act
            var ticketController = new TicketController(loggerMock.Object, mediatorMock.Object, mapper, usermanagerMock.Object);
            ticketController.ControllerContext = context;
            var actionResult = await ticketController.CreateTicket(model);

            //assert
            // validate stuff
        }
    }
}
