using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class CreateUserLoginCommandHandler : BaseCommandHandler<CreateUserLoginCommand, UserLogin>
    {
        public CreateUserLoginCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<UserLogin> ExecuteCommandAsync(CreateUserLoginCommand request, CancellationToken cancellationToken)
        {
            var userlogin = new UserLogin
            {
                Date = request.Date,
                Success = request.Success,
                Username = request.Username
            };
            await _dbContext.UserLogins.AddAsync(userlogin);
            _dbContext.SaveChanges();

            return userlogin;
        }
    }
}
