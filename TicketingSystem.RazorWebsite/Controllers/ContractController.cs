using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Exceptions;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Models.Contracts;

namespace TicketingSystem.RazorWebsite.Controllers
{
    public class ContractController : Controller
    {
        private readonly ILogger<ContractCreateViewModel> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContractController(
            ILogger<ContractCreateViewModel> logger,
            IMediator mediator,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _userManager = userManager;
        }

        private List<ContractStatus> ConvertFilterToContractStatuses(string filter)
        {
            List<ContractStatus> contractStatuses = new List<ContractStatus>();
            if (!string.IsNullOrEmpty(filter))
            {
                var filters = filter.Split(",");
                foreach (var f in filters)
                {
                    if (Enum.TryParse<ContractStatus>(f, out var contractStatus))
                    {
                        contractStatuses.Add(contractStatus);
                    }
                }
            }
            else
            {
                contractStatuses.Add(ContractStatus.Lopend);
                contractStatuses.Add(ContractStatus.InAanvraag);
            }

            return contractStatuses;
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index([FromQuery] string statusFilter)
        {
            List<ContractStatus> contractStatusFilter = ConvertFilterToContractStatuses(statusFilter);
            IQueryable<Contract> contracts;
            if (User.IsInRole("SupportManager"))
            {
                contracts = await _mediator.Send(new GetContractsQuery());
            }
            else
            {
                var client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
                contracts = await _mediator.Send(new GetContractsByClientIdQuery { ClientId = client.Id });
            }

            var filteredContracts = contracts.Where(x => contractStatusFilter.Contains(x.Status));

            var ticketsIndexDto = _mapper.Map<List<Contract>, List<ContractBaseInfoViewModel>>(filteredContracts.ToList());
            var model = new ContractIndexViewModel
            {
                Contracts = ticketsIndexDto,
                FilterInput = new FilterInputModel
                {
                    FilterStatusPending = contractStatusFilter.Contains(ContractStatus.InAanvraag),
                    FilterStatusActive = contractStatusFilter.Contains(ContractStatus.Lopend),
                    FilterStatusClosed = contractStatusFilter.Contains(ContractStatus.Beëindigd)
                }
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index(ContractIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sb = new StringBuilder();
                string delimiter = ",";

                void AppendToStringBuilder(string text)
                {
                    if (sb.Length > 0)
                        sb.Append(delimiter);
                    sb.Append(text);
                }

                if (model.FilterInput.FilterStatusPending)
                    AppendToStringBuilder(ContractStatus.InAanvraag.ToString());

                if (model.FilterInput.FilterStatusActive)
                    AppendToStringBuilder(ContractStatus.Lopend.ToString());

                if (model.FilterInput.FilterStatusClosed)
                    AppendToStringBuilder(ContractStatus.Beëindigd.ToString());

                return await Index(sb.ToString());
            }

            return View(model);
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create() 
        {
            var model = new ContractCreateViewModel();

            //TODO get contract types and convert them to selectlistitems
            model.ContractTypes = new List<SelectListItem>();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ContractCreateViewModel model)
        {
            //TODO remove this after httpget has implemented fetching of contract types
            model.ContractTypes = new List<SelectListItem>();

            if (ModelState.IsValid) 
            {
                // code goes here...
            }

            return View(model);
        }
    }
}
