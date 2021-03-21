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
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index([FromQuery] string statusFilter)
        {
            List<ContractStatus> contractStatusFilter = ConvertFilterToContractStatuses(statusFilter);
            IQueryable<Contract> contracts;

            var client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
            contracts = await _mediator.Send(new GetContractsByClientIdQuery { ClientId = client.Id });

            var filteredContracts = contracts.Where(x => contractStatusFilter.Contains(x.Status));

            var contractsIndexDto = _mapper.Map<List<Contract>, List<ContractBaseInfoViewModel>>(filteredContracts.ToList());
            var model = new ContractIndexViewModel
            {
                Contracts = contractsIndexDto,
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
        [Authorize(Roles = "Customer")]
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

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _mediator.Send(new GetContractByIdQuery { Id = id });

            if (contract == null)
            {
                return RedirectToAction("Index");
            }

            var contractDetailsDto = _mapper.Map<Contract, ContractDetailInfoViewModel>(contract);
            var model = new ContractDetailsViewModel { Contract = contractDetailsDto };

            return View(model);
        }

        private async Task<List<SelectListItem>> GetContractTypes(string selectedValue = null)
        {
            var contractTypes = await _mediator.Send(new GetContractTypesQuery());
            var result = new List<SelectListItem>();
            foreach (var contractType in contractTypes)
            {
                result.Add(new SelectListItem { Value = contractType.Name, Text = contractType.Name, Selected = contractType.Name == selectedValue });
            }

            return result;
        }


        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ContractCreateViewModel();

            model.ContractTypes = await GetContractTypes();

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create(ContractCreateViewModel model)
        {
            Client client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });

            if (ModelState.IsValid)
            {
                if (client == null)
                {
                    ModelState.AddModelError(string.Empty, "Client is required.");
                    return View(model);
                }

                try
                {
                    await _mediator.Send(new CreateContractCommand
                    {
                        Status = ContractStatus.InAanvraag,
                        Type = model.Input.Type,
                        Client = client,
                        ValidFrom = model.Input.StartDate,
                        ValidTo = model.Input.EndDate

                    }); ;
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }

                return LocalRedirect(model.ReturnUrl ?? Url.Content("~/Contract/Index"));
            }

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Cancel([FromQuery] int id, ContractDetailsViewModel model)
        {
            try
            {
                var contract = await _mediator.Send(new CancelContractCommand
                {
                    ContractId = id
                });

                model.Contract = _mapper.Map<Contract, ContractDetailInfoViewModel>(contract);
                return View("Details", model);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("ValidationError", ex.Message);
                return View("Details", model);
            }
        }
    }
}
