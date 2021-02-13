using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Application.Queries
{
    public abstract class BaseQuery<TResponse> : IRequest<TResponse> where TResponse : class
    {
    }
}
