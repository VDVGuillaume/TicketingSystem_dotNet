using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Exceptions;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services.Interfaces;

namespace TicketingSystem.Infrastructure.Services
{
    public class ContractService : IContractService
    {
        private readonly IMediator _mediator;
        private readonly TicketingSystemDbContext _dbContext;


        public ContractService(IMediator mediator, TicketingSystemDbContext dbContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException();
            _dbContext = dbContext ?? throw new ArgumentNullException();
        }

        private void ValidateContractStatus(Contract contract)
        {
            if (contract.Status == ContractStatus.Beëindigd)
            {
                throw new ValidationException(Constants.ERROR_CONTRACT_STATUS_CLOSED);
            }
        }

        public async Task<Contract> CancelContract(CancelContractCommand request)
        {
            var contract = await _mediator.Send(new GetContractByIdQuery { Id = request.ContractId });

            //validate
            if (contract == null)
                throw new ValidationException(Constants.ERROR_CONTRACT_NOT_FOUND);
            ValidateContractStatus(contract);

            contract.Status = ContractStatus.Beëindigd;

            await _dbContext.SaveChangesAsync();

            return contract;
        }

        public async Task<Contract> CreateContract(CreateContractCommand request)
        {

            var contractType = await _mediator.Send(new GetContractTypeByNameQuery { Name = request.Type });
            if (contractType == null)
            {
                throw new ValidationException(Constants.ERROR_CONTRACT_TYPE_NOT_FOUND);
            }                        

            //Throw exception on validTo < validfrom

            if(request.ValidFrom > request.ValidTo || request.ValidFrom < DateTime.Today)
            {
                throw new ValidationException(Constants.ERROR_CONTRACT_FUTURE_DATE);
            }
          

            // create new ticket
            var contract = new Contract( contractType, request.Status, request.ValidFrom, request.ValidTo, request.Client);
            await _dbContext.Contracts.AddAsync(contract);

            // save ticket
            // this step comes before adding the attachments because we want to link the attachment location to the ticket ID
            _dbContext.SaveChanges();

            //create attachments          

            return contract;
        }
    }
}

