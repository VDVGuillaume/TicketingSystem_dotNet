﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CreateUserLoginAttemptCommand : BaseCommand<UserLoginAttempt>
    {
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
    }
}