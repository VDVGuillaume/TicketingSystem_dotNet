using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services.Interfaces;

namespace TicketingSystem.Infrastructure.Services
{
    public class ContractService : IContractService
    {
        public Task<Contract> CancelContract(CancelContractCommand request)
        {
            throw new NotImplementedException();
        }

        public Task<Contract> CreateContract(CreateContractCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
