using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.ViewModels;
using TicketingSystem.RazorWebsite.Models;
using TicketingSystem.RazorWebsite.Models.Contracts;
using TicketingSystem.RazorWebsite.Models.Reports;
using TicketingSystem.RazorWebsite.Models.Tickets;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.RazorWebsite.Controllers
{
    [Authorize(Roles = "Customer,SupportManager")]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportViewModel> _logger;
        private readonly IMediator _mediator; 
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(
            ILogger<ReportViewModel> logger,
            IMediator mediator, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator; 
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> OpenTickets()
        {
            IQueryable<Ticket> tickets;

            if (User.IsInRole("SupportManager"))
            {
                tickets = await _mediator.Send(new GetTicketsQuery());
            }
            else
            {
                var client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
                tickets = await _mediator.Send(new GetTicketsByClientIdQuery { ClientId = client.Id });
            }

            SortedDictionary<DateTime, int> dateIncrements = new SortedDictionary<DateTime, int>();
            foreach (Ticket ticket in tickets)
            {
                if (ticket.DateAdded > DateTime.MinValue)
                {
                    if (dateIncrements.ContainsKey(ticket.DateAdded.Date))
                        dateIncrements[ticket.DateAdded.Date] += 1;
                    else
                        dateIncrements.Add(ticket.DateAdded.Date, 1);
                }

                if (ticket.DateClosed > DateTime.MinValue)
                {
                    if (dateIncrements.ContainsKey(ticket.DateClosed.Date))
                        dateIncrements[ticket.DateClosed.Date] -= 1;
                    else
                        dateIncrements.Add(ticket.DateClosed.Date, -1);
                }
            }

            int currentCount = 0;
            string chartData = string.Empty;
            foreach (KeyValuePair<DateTime, int> kvp in dateIncrements)
            {
                currentCount += kvp.Value;
                chartData += $"[new Date({kvp.Key.Year}, {kvp.Key.Month-1}, {kvp.Key.Day}), {currentCount}], ";
            }
            if (chartData.EndsWith(", "))
                chartData = chartData.Remove(chartData.Length - 2, 2);

            var model = new ReportViewModel() { ChartData = chartData };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> TicketsSolveTime()
        {
            IQueryable<Ticket> tickets;

            if (User.IsInRole("SupportManager"))
            {
                tickets = await _mediator.Send(new GetTicketsQuery());
            }
            else
            {
                var client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
                tickets = await _mediator.Send(new GetTicketsByClientIdQuery { ClientId = client.Id });
            }

            int[] count = new int[4] { 0, 0, 0, 0 };
            int totalCount = 0;
            TimeSpan solveTime;
            TimeSpan totalSolveTime = new TimeSpan(0);
            foreach (Ticket ticket in tickets)
            {
                if (ticket.DateAdded > DateTime.MinValue && ticket.DateClosed > DateTime.MinValue)
                {
                    solveTime = ticket.DateClosed.Subtract(ticket.DateAdded);

                    if (solveTime <= new TimeSpan(8, 0, 0))
                    {
                        count[0]++;
                    }
                    else if (solveTime <= new TimeSpan(24, 0, 0))
                    {
                        count[1]++;
                    }
                    else if (solveTime <= new TimeSpan(5, 0, 0, 0))
                    {
                        count[2]++;
                    }
                    else if (solveTime > new TimeSpan(5, 0, 0, 0))
                    {
                        count[3]++;
                    }
                    totalCount++;
                    totalSolveTime += solveTime;
                }
            }

            solveTime = totalSolveTime / totalCount;

            string averageSolveTime = string.Empty;
            if (solveTime.Days > 1)
                averageSolveTime += solveTime.Days + " dagen, ";
            else if (solveTime.Days == 1)
                averageSolveTime += solveTime.Days + " dag, ";
            averageSolveTime += solveTime.Hours + " uur, " + solveTime.Minutes + " minuten";

            var model = new ReportViewModel() { AverageSolveTime = averageSolveTime, TicketSolveTimeCount = count };
            return View(model);
        }
    }
}
