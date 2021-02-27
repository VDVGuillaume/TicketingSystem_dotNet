using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Controllers;
using TicketingSystem.RazorWebsite.Models.Tickets;
using TicketingSystem.Xunit.Tests.Factories;
using TicketingSystem.Xunit.Tests.FakeObjects;
using Xunit;

namespace TicketingSystem.Xunit.Tests.Controllers
{
    public class TicketControllerTest
    {
        private readonly TicketController _controller;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<TicketViewModel>> _loggerMock;
        private readonly Mock<FakeUserManager> _userManagerMock;
        private readonly IMapper _mapper;
        private readonly Mock<ClaimsPrincipal> _userMock;
        private readonly IdentityUser Customer;


        public TicketControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<TicketViewModel>>();
            _userManagerMock = new Mock<FakeUserManager>();
            _mapper = MapperTestFactory.GenerateMapper();
            _userMock = new Mock<ClaimsPrincipal>();
            _controller = new TicketController(_loggerMock.Object, _mediatorMock.Object, _mapper, _userManagerMock.Object);


        }

        #region -- Create Get --
        [Fact]
        public void Create_PassesNewTicketDetailsViewModel()
        {

        }


        #endregion 

        #region -- Create Post --

        [Theory]
        [InlineData("customer", "title", "Aangemaakt", "description", "customer")]
        public async void Create_ValidTicket_CreatesTicketAndPersistsTicketAndRedirectsToActionIndex(string username, string title, string type, string description, string clientUsername)
        {
            _userMock
                  .Setup(x => x.IsInRole("Customer"))
                  .Returns(true);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = _userMock.Object
                }
            };
            _userManagerMock
                .Setup(x => x.GetUserAsync(_userMock.Object))
                .Returns(Task.FromResult(new IdentityUser(username)));



            CreateTicketCommand cmdSaved = null;
            _mediatorMock.Setup(x =>
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

            _controller.ControllerContext = context;
            var actionResult = await _controller.CreateTicket(model);


            //assert
            var result = Assert.IsType<RedirectToActionResult>(_controller.CreateTicket(model));
            Assert.Equal("Index", result?.ActionName);

            // validate stuff
        }


        [Fact]
        public void Create_DomainErrors_DoesNotCreateNorPersistTicketAndRedirectsToActionIndex()
        {
            //waiting for implementation of contracts and changes to be made with the client class
        }

        [Fact]
        public void Create_ModelStateErrors_DoesNotCreateNorPersistTicketAndRedirectsToActionIndex()
        {
            //waiting for implementation of contracts and changes to be made with the client class
        }



        #endregion

    }
}

