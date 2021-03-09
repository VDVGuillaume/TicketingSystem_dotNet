using MediatR;
using Moq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Infrastructure;
using TicketingSystem.Infrastructure.Services;
using Xunit;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Exceptions;
using System.Threading;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.Xunit.Tests.Services
{
    public class TicketServiceTest
    {
        private readonly TicketService _service;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TicketingSystemDbContext _dbContext;

        public TicketServiceTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketingSystemDbContext>()
                .UseInMemoryDatabase(databaseName: "testing");
            
            _dbContext = new TicketingSystemDbContext(optionsBuilder.Options);
            _mediatorMock = new Mock<IMediator>();
            _service = new TicketService(_mediatorMock.Object, _dbContext);
        }

        [Fact]
        public async Task CreateTicket_InvalidCustomer_Should_Return_Exception() 
        {
            var description = "een beschrijving";
            var title = "een titel";
            var type = "Support";
            string expectedErrorMessage = Constants.ERROR_CLIENT_NOT_FOUND;

            //arrange
            var command = new CreateTicketCommand { 
                Client = null,
                AssignedEngineer  = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type 
            };

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public async Task CreateTicket_InvalidType_Should_Return_Exception()
        {
            // set expected
            var description = "een beschrijving";
            var title = "een titel";
            var type = "OngeldigType";
            string expectedErrorMessage = Constants.ERROR_TICKET_TYPE_NOT_FOUND;

            //arrange
            var command = new CreateTicketCommand
            {
                Client = new Client("Klant1"),
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult((TicketType)null));
            
            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CreateTicket_NoActiveContract_Should_Return_Exception()
        {
            // set expected
            var description = "een beschrijving";
            var title = "een titel";
            var type = "Support";
            var client = new Client("Klant1");
            string expectedErrorMessage = Constants.ERROR_ACTIVE_CONTRACT_NOT_FOUND;

            //arrange
            var command = new CreateTicketCommand
            {
                Client = client,
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetTicketTypeByNameQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new TicketType {Name = type }));

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetActiveContractByClientQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult((Contract)null));

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));

            //assert
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetActiveContractByClientQuery>(y => y.Client == client), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Theory]
        [InlineData("een titel", "een beschrijving", "Support", "Klant1")]
        [InlineData("", "", "Bug", "Klant1")]
        [InlineData("een supercoole andere titel", "", "Bug", "GigaKlant")]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa")]
        public async Task CreateTicket_Should_Succeed(string title, string description, string type, string clientName)
        {
            var expectedTicketStatus = TicketStatus.Aangemaakt;

            var client = new Client(clientName);
            var contractType = new ContractType("always", true, TicketCreationTime.Altijd);
            var contract = new Contract(contractType, ContractStatus.Lopend, DateTime.Today, DateTime.Today.AddYears(1), client);

            //arrange
            var command = new CreateTicketCommand
            {
                Client = client,
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetTicketTypeByNameQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new TicketType { Name = type }));

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetActiveContractByClientQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(contract));

            //act
            var createdTicket = await _service.CreateTicket(command);
            var ticketInDB = await _dbContext.Tickets.FirstOrDefaultAsync(x => x.Ticketnr == createdTicket.Ticketnr);

            //assert
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetActiveContractByClientQuery>(y => y.Client == client), It.IsAny<CancellationToken>()), Times.Once());

            
            Assert.NotNull(ticketInDB);
            Assert.Equal(contract, ticketInDB.Contract);
            Assert.Equal(description, ticketInDB.Description);
            Assert.Equal(title, ticketInDB.Title);
            Assert.Equal(expectedTicketStatus, ticketInDB.Status);
            Assert.Equal(client, ticketInDB.Client);
        }

        [Fact]
        public async Task CancelTicket_InvalidTicket_Should_Return_Exception()
        {
            var ticketnr = 999;
            string expectedErrorMessage = Constants.ERROR_TICKET_NOT_FOUND;

            //arrange
            var command = new CancelTicketCommand
            {
                Ticketnr = ticketnr
            };

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CancelTicket(command));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }
    }
}
