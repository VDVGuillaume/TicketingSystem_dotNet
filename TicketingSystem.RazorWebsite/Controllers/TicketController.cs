using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.ViewModels;

namespace TicketingSystem.RazorWebsite.Controllers
{
    public class TicketController : Controller
    {

        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator;

        public TicketController(
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



        [HttpPost]
        public IActionResult CreateTicket(TicketViewModel model)
        {
            
            
            return View(model);
        }

    }
}
