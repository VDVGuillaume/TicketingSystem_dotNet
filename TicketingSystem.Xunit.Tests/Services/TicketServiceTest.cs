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
using System.Globalization;

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
        public async Task UpdateTicket_InvalidTicket_Should_Return_Exception()
        {
            // set expected
            var ticketId = 1;
            var description = "een beschrijving";
            var title = "een titel";
            var type = "request";
            string expectedErrorMessage = Constants.ERROR_TICKET_NOT_FOUND;

            //arrange
            var command = new UpdateTicketCommand
            {
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type,
                Ticketnr = ticketId
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.Is<GetTicketByIdQuery>(y => y.Id == ticketId),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult((Ticket)null));

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateTicket(command));
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketByIdQuery>(y => y.Id == ticketId), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateTicket_InvalidType_Should_Return_Exception()
        {
            var oldTicketType = new TicketType { Name = "Request" };
            var oldClient = new Client("client1");
            var oldContractType = new ContractType("contracttype", true, TicketCreationTime.Altijd);
            var oldContract = new Contract(oldContractType, ContractStatus.Lopend, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10), oldClient);

            // set expected
            var ticketId = 1;
            var description = "een beschrijving";
            var title = "een titel";
            var ticketType = "request";
            string expectedErrorMessage = Constants.ERROR_TICKET_TYPE_NOT_FOUND;

            //arrange
            var command = new UpdateTicketCommand
            {
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = ticketType,
                Ticketnr = ticketId
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.Is<GetTicketByIdQuery>(y => y.Id == ticketId),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new Ticket("oude titel","oude beschrijving", oldTicketType, oldClient, oldContract)));

            _mediatorMock.Setup(x =>
                        x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == ticketType),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult((TicketType)null));

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateTicket(command));
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == ticketType), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateTicket_Should_Exceed()
        {
            var oldTicketType = new TicketType { Name = "Request" };
            var oldClient = new Client("client1");
            var oldContractType = new ContractType("contracttype", true, TicketCreationTime.Altijd);
            var oldContract = new Contract(oldContractType, ContractStatus.Lopend, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10), oldClient);

            // set expected
            var ticketId = 1;
            var description = "een beschrijving";
            var title = "een titel";
            var ticketType = "request";
            string expectedErrorMessage = Constants.ERROR_TICKET_TYPE_NOT_FOUND;

            //arrange
            var command = new UpdateTicketCommand
            {
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = ticketType,
                Ticketnr = ticketId
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.Is<GetTicketByIdQuery>(y => y.Id == ticketId),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new Ticket("oude titel", "oude beschrijving", oldTicketType, oldClient, oldContract)));

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetTicketTypeByNameQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new TicketType { Name = ticketType }));


            var updatedTicket = await _service.UpdateTicket(command);

            //assert
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == ticketType), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketByIdQuery>(y => y.Id == ticketId), It.IsAny<CancellationToken>()), Times.Once());


            Assert.NotNull(updatedTicket);
            Assert.Equal(description, updatedTicket.Description);
            Assert.Equal(title, updatedTicket.Title);
            Assert.Equal(ticketType, updatedTicket.Type.Name);
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
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "20/03/2021")]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "21/03/2021")]
        public async Task CreateTicket_InvalidTicketCreationTime_Weekend_Should_Return_Exception(string title, string description, string type, string clientName, string dateString)
        {
            var expectedTicketStatus = TicketStatus.Aangemaakt;
            var expectedErrorMessage = Constants.ERROR_TICKETCREATIONTIME_WEEKENDS;
            var date = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var client = new Client(clientName);
            var contractType = new ContractType("always", true, TicketCreationTime.Weekdagen);
            var ticketCreationType = new TicketCreationType(TicketCreationTypeName.Applicatie);
            contractType.TicketCreationTypes.Add(ticketCreationType);
            var contract = new Contract(contractType, ContractStatus.Lopend, DateTime.Today, DateTime.Today.AddYears(1), client);

            //arrange
            var command = new CreateTicketCommand
            {
                Client = client,
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type,
                DateRequested = date
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetTicketTypeByNameQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new TicketType { Name = type }));

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetActiveContractByClientQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(contract));

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));

            //assert
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetActiveContractByClientQuery>(y => y.Client == client), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Theory]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "19/03/2021T20:05:00")]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "15/03/2021T20:05:00")]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "15/03/2021T17:01:00")]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "15/03/2021T07:59:00")]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa", "15/03/2021T04:30:00")]
        public async Task CreateTicket_InvalidTicketCreationTime_OfficeHours_Should_Return_Exception(string title, string description, string type, string clientName, string dateString)
        {
            var expectedTicketStatus = TicketStatus.Aangemaakt;
            var expectedErrorMessage = Constants.ERROR_TICKETCREATIONTIME_OFFICE_HOURS;
            var date = DateTime.ParseExact(dateString, "dd/MM/yyyyTHH:mm:ss", CultureInfo.InvariantCulture);

            var client = new Client(clientName);
            var contractType = new ContractType("always", true, TicketCreationTime.Weekdagen);
            var ticketCreationType = new TicketCreationType(TicketCreationTypeName.Applicatie);
            contractType.TicketCreationTypes.Add(ticketCreationType);
            var contract = new Contract(contractType, ContractStatus.Lopend, DateTime.Today, DateTime.Today.AddYears(1), client);

            //arrange
            var command = new CreateTicketCommand
            {
                Client = client,
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type,
                DateRequested = date
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetTicketTypeByNameQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new TicketType { Name = type }));

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetActiveContractByClientQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(contract));

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));

            //assert
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetActiveContractByClientQuery>(y => y.Client == client), It.IsAny<CancellationToken>()), Times.Once());
        }



        [Theory]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa")]
        public async Task CreateTicket_InvalidTicketCreationType_Should_Return_Exception(string title, string description, string type, string clientName)
        {
            var expectedTicketStatus = TicketStatus.Aangemaakt;
            var expectedErrorMessage = Constants.ERROR_TICKET_CREATION_NOT_ALLOWED_APPLICATION;

            var client = new Client(clientName);
            var contractType = new ContractType("always", true, TicketCreationTime.Altijd);
            var ticketCreationTypeEmail = new TicketCreationType(TicketCreationTypeName.Email);
            contractType.TicketCreationTypes.Add(ticketCreationTypeEmail);
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

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));

            //assert
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetActiveContractByClientQuery>(y => y.Client == client), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Theory]
        [InlineData("Aanmaken nieuwe gebruiker", "Andrès toe te voegen als gebruiker", "Request", "DaDiDa")]
        public async Task CreateTicket_InvalidTicketCreationTime_Should_Return_Exception(string title, string description, string type, string clientName)
        {
            var expectedTicketStatus = TicketStatus.Aangemaakt;
            var expectedErrorMessage = Constants.ERROR_TICKET_CREATION_NOT_ALLOWED_APPLICATION;

            var client = new Client(clientName);
            var contractType = new ContractType("always", true, TicketCreationTime.Altijd);
            var ticketCreationTypeEmail = new TicketCreationType(TicketCreationTypeName.Email);
            contractType.TicketCreationTypes.Add(ticketCreationTypeEmail);
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

            //act && assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTicket(command));

            //assert
            Assert.Equal(expectedErrorMessage, exception.Message);
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(x => x.Send(It.Is<GetActiveContractByClientQuery>(y => y.Client == client), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Theory]
        [InlineData("een titel", "een beschrijving", "Support", "Klant1", "15/03/2021T08:00:00")]
        [InlineData("een titel", "een beschrijving", "Support", "Klant1", "15/03/2021T16:59:00")]
        [InlineData("een titel", "een beschrijving", "Support", "Klant1", "15/03/2021T12:00:00")]
        [InlineData("een titel", "een beschrijving", "Support", "Klant1", "15/03/2021T14:00:00")]
        public async Task CreateTicket_WeekDays_Should_Succeed(string title, string description, string type, string clientName, string dateString)
        {
            var expectedTicketStatus = TicketStatus.Aangemaakt;
            var date = DateTime.ParseExact(dateString, "dd/MM/yyyyTHH:mm:ss", CultureInfo.InvariantCulture);

            var client = new Client(clientName);
            var contractType = new ContractType("weekdagen", true, TicketCreationTime.Weekdagen);
            contractType.TicketCreationTypes.Add(new TicketCreationType(TicketCreationTypeName.Applicatie));
            var contract = new Contract(contractType, ContractStatus.Lopend, DateTime.Today, DateTime.Today.AddYears(1), client);

            //arrange
            var command = new CreateTicketCommand
            {
                Client = client,
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type,
                DateRequested = date
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
            contractType.TicketCreationTypes.Add(new TicketCreationType(TicketCreationTypeName.Applicatie));
            var contract = new Contract(contractType, ContractStatus.Lopend, DateTime.Today, DateTime.Today.AddYears(1), client);

            //arrange
            var command = new CreateTicketCommand
            {
                Client = client,
                AssignedEngineer = null,
                Attachments = null,
                Description = description,
                Title = title,
                Type = type,
                DateRequested = DateTime.Now
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

        [Fact]
        public async Task CancelTicket_Should_Succeed()
        {
            Ticket ticket = _dbContext.Tickets.FirstOrDefaultAsync(x => x.Status == TicketStatus.InBehandeling || x.Status == TicketStatus.Aangemaakt).Result;

            if (ticket == null)
            {
                var client1 = new Client("Klant1");
                _dbContext.Client.Add(client1);

                var ticketCreationTypeEmail = new TicketCreationType(TicketCreationTypeName.Email);
                var ticketCreationTypePhone = new TicketCreationType(TicketCreationTypeName.Telefonisch);
                var ticketCreationTypeApplication = new TicketCreationType(TicketCreationTypeName.Applicatie);
                _dbContext.TicketCreationTypes.Add(ticketCreationTypeEmail);
                _dbContext.TicketCreationTypes.Add(ticketCreationTypePhone);
                _dbContext.TicketCreationTypes.Add(ticketCreationTypeApplication);

                var contractType1 = new ContractType("Alle creatie types, 24/7", true, TicketCreationTime.Altijd);
                contractType1.TicketCreationTypes.Add(ticketCreationTypeEmail);
                contractType1.TicketCreationTypes.Add(ticketCreationTypePhone);
                contractType1.TicketCreationTypes.Add(ticketCreationTypeApplication);
                _dbContext.ContractTypes.Add(contractType1);

                var contract1 = new Contract(contractType1, ContractStatus.Lopend, new DateTime(2020, 01, 01), new DateTime(2020, 12, 31), client1);
                _dbContext.Contracts.Add(contract1);

                var ticketTypeBug = new TicketType { Name = "Bug", RequiredSLA = 1 };
                _dbContext.TicketTypes.Add(ticketTypeBug);

                var ticketBugCreated = new Ticket("TitleBug", "TestDescription", ticketTypeBug, client1, contract1);
                _dbContext.Tickets.Add(ticketBugCreated);

                _dbContext.SaveChanges();

                ticket = _dbContext.Tickets.FirstOrDefaultAsync(x => x.Status == TicketStatus.InBehandeling || x.Status == TicketStatus.Aangemaakt).Result;
            }
            
            //arrange
            var command = new CancelTicketCommand
            {
                Ticketnr = ticket.Ticketnr
            };

            _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetTicketByIdQuery>(),
                        It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(ticket));

            //act
            var canceledTicket = await _service.CancelTicket(command);
            var ticketInDB = await _dbContext.Tickets.FirstOrDefaultAsync(x => x.Ticketnr == canceledTicket.Ticketnr);

            //assert
            _mediatorMock.Verify(x => x.Send(It.Is<GetTicketByIdQuery>(y => y.Id == ticket.Ticketnr), It.IsAny<CancellationToken>()), Times.Once());

            Assert.NotNull(ticketInDB);
            Assert.Equal(ticket.Ticketnr, ticketInDB.Ticketnr);
            Assert.Equal(TicketStatus.Geannuleerd, ticketInDB.Status);
        }
    }
}
