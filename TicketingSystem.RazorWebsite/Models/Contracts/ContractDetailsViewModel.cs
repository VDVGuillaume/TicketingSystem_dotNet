using Microsoft.AspNetCore.Mvc;
using System;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.RazorWebsite.Models.Contracts
{
    public class ContractDetailsViewModel
    {
        public ContractDetailInfoViewModel Contract { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

    }

    public class ContractDetailInfoViewModel
    {
        public int Id { get; set; }
        public ContractType Type { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Client Client { get; set; }
    }
}