using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Application.Commands
{
    public abstract class BaseCommand<TResponse> : IRequest<TResponse> where TResponse : class
    {
    }
}
