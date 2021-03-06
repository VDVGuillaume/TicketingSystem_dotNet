using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.Services.Interfaces
{
    public interface IContractService
    {
        Task<Contract> CreateContract(CreateContractCommand request);    
        Task<Contract> CancelContract(CancelContractCommand request);
      
    }
}
