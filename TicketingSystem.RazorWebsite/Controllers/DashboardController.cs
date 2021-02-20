using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.ViewModels;

namespace TicketingSystem.RazorWebsite.Controllers
{

    public class DashboardController : Controller
    {
        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator;

        public DashboardController(
            ILogger<TicketViewModel> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
