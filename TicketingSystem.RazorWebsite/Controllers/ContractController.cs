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

        public IActionResult Index()
        {
            return View();
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
