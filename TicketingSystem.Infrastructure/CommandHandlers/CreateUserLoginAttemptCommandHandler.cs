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
    public class CreateUserLoginAttemptCommandHandler : BaseCommandHandler<CreateUserLoginAttemptCommand, UserLoginAttempt>
    {
        public CreateUserLoginAttemptCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<UserLoginAttempt> ExecuteCommandAsync(CreateUserLoginAttemptCommand request, CancellationToken cancellationToken)
        {
            var userlogin = new UserLoginAttempt
            {
                Date = request.Date,
                Success = request.Success,
                Username = request.Username
            };
            await _dbContext.UserLoginAttempts.AddAsync(userlogin);
            _dbContext.SaveChanges();

            return userlogin;
        }
    }
}
