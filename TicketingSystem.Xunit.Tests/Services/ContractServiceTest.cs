﻿using MediatR;
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
    public class ContractServiceTest
    {

        private readonly ContractService _service;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TicketingSystemDbContext _dbContext;

        public ContractServiceTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketingSystemDbContext>()
                .UseInMemoryDatabase(databaseName: "testing");

            _dbContext = new TicketingSystemDbContext(optionsBuilder.Options);
            _mediatorMock = new Mock<IMediator>();
            _service = new ContractService(_mediatorMock.Object, _dbContext);
        }

        [Theory]
        [InlineData("Email/Telefonisch, 24/7", "klant1", "2021-8-25", "2021-8-29","Email")]
        [InlineData("Alle creatie types, 24/7", "klant2", "2021-10-05", "2021-10-21","Telefonisch")]
        [InlineData("Email/Applicatie, Weekdagen", "klant3", "2021-9-15", "2021-11-16","Applicatie")]       
        public async Task CreateContract_Should_Succeed(string type, string clientName, string validFrom, string validTo,string creationType)
        {
            var expectedContractStatus = ContractStatus.InAanvraag;
            var client = new Client(clientName);
            var ticketCreationType= new TicketCreationType(creationType);

            //arrange
            var command = new CreateContractCommand
            {
                Client = client,                
                Type = type,
                ValidFrom = DateTime.Parse(validFrom) ,
                ValidTo = DateTime.Parse(validTo),
            };

           _mediatorMock.Setup(x =>
                        x.Send(It.IsAny<GetContractTypeByNameQuery>(),
                        It.IsAny<CancellationToken>()))
                      .Returns(Task.FromResult(new ContractType (type, true, TicketCreationTime.Altijd )));

            //act
            var createdContract = await _service.CreateContract(command);
            var contractInDB = await _dbContext.Contracts.FirstOrDefaultAsync(x => x.ContractId == createdContract.ContractId);

            //assert
            _mediatorMock.Verify(x => x.Send(It.Is<GetContractTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());
           

            Assert.NotNull(contractInDB);           
            Assert.Equal(DateTime.Parse(validFrom), contractInDB.ValidFrom);
            Assert.Equal(DateTime.Parse(validTo), contractInDB.ValidTo);
            Assert.Equal(expectedContractStatus, contractInDB.Status);
            Assert.Equal(client, contractInDB.Client);
        }




        [Theory]
        [InlineData("Email/Telefonisch, 24/7", "klant1", "2020-02-25", "2020-8-29","Email")]
        [InlineData("Alle creatie types, 24/7", "klant2", "2020-01-05", "2020-10-21","Telefonisch")]
        [InlineData("Email/Applicatie, Weekdagen", "klant3", "2021-03-08", "2020-11-16","Applicatie")]
        public async Task CreateContract_InvalidStartDate_ThrowsException(string type, string clientName, string validFrom, string validTo,string creationType)
        {
            
            var client = new Client(clientName);
            var ticketCreationType = new TicketCreationType(creationType);
            string expectedErrorMessage = Constants.ERROR_CONTRACT_FUTURE_DATE;

            //arrange
            var command = new CreateContractCommand
            {
                Client = client,
                Type = type,
                ValidFrom = DateTime.Parse(validFrom),
                ValidTo = DateTime.Parse(validTo),
            };

            _mediatorMock.Setup(x =>
                         x.Send(It.IsAny<GetContractTypeByNameQuery>(),
                         It.IsAny<CancellationToken>()))
                       .Returns(Task.FromResult(new ContractType(type, true, TicketCreationTime.Altijd)));                   
             
            //act & assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.CreateContract(command));
            Assert.Equal(expectedErrorMessage, exception.Message);

            _mediatorMock.Verify(x => x.Send(It.Is<GetContractTypeByNameQuery>(y => y.Name == type), It.IsAny<CancellationToken>()), Times.Once());

        }







    }
}