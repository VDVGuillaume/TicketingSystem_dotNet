﻿using System;

namespace TicketingSystem.Domain.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
    }
}
