﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure
{
    public class TicketingSystemDataInitializer
    {
        private readonly TicketingSystemDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TicketingSystemDataInitializer(TicketingSystemDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                //Seed clients
                var client1 = new Client("Klant1");
                var client2 = new Client("Klant2");
                _dbContext.Client.Add(client1);
                _dbContext.Client.Add(client2);

                //Seed roles
                var customerRole = new IdentityRole("Customer");
                var supportManagerRole = new IdentityRole("SupportManager");
                await _roleManager.CreateAsync(customerRole);
                await _roleManager.CreateAsync(supportManagerRole);

                //Seed users
                var customerUser = new ApplicationUser { UserName = "customer", Email = "customer@gmail.be", Client = client1 };
                await _userManager.CreateAsync(customerUser, "P@ssword1");
                await _userManager.AddToRoleAsync(customerUser, "Customer");
                
                var supportManagerUser = new ApplicationUser { UserName = "supportmanager", Email = "supportmanager@gmail.be" };
                await _userManager.CreateAsync(supportManagerUser, "P@ssword1");
                await _userManager.AddToRoleAsync(supportManagerUser, "SupportManager");

                //Seed ticketTypes
                var ticketTypeBug = new TicketType { Name = "Bug", RequiredSLA = 1 };
                var ticketTypeChangeRequest = new TicketType { Name = "Change Request", RequiredSLA = 2 };
                var ticketTypeSupport = new TicketType { Name = "Support", RequiredSLA = 3 };
                _dbContext.TicketTypes.Add(ticketTypeBug);
                _dbContext.TicketTypes.Add(ticketTypeChangeRequest);
                _dbContext.TicketTypes.Add(ticketTypeSupport);

                //Seed tickets
                var ticketSupportCreated = new Ticket("TitleSupport", "TestDescription", ticketTypeSupport, client1);
                var ticketChangeRequestCreated = new Ticket("TitleChangeRequest", "TestDescription", ticketTypeChangeRequest, client1);
                var ticketBugCreated = new Ticket("TitleBug", "TestDescription", ticketTypeBug, client1);

                var ticketBugInProgress = new Ticket("TitleBug2metComments", "TestDescription", ticketTypeBug, client1) { Status = TicketStatus.InBehandeling};
                var ticketBugClosed = new Ticket("TitleBug3", "TestDescription", ticketTypeBug, client1) { Status = TicketStatus.Afgehandeld };
                var ticketBugCancelled = new Ticket("TitleBug4", "TestDescription", ticketTypeBug, client1) { Status = TicketStatus.Geannuleerd };

                //Seed Comments 
                var commentTicket1 = new Comment {Text = "Dit is een korte comment",CreatedBy=customerUser.UserName,DateAdded= DateTime.Today};
                var commentTicket2 = new Comment { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum ", CreatedBy = customerUser.UserName, DateAdded = DateTime.Today };
                ticketBugInProgress.Comments.Add(commentTicket1);
                ticketBugInProgress.Comments.Add(commentTicket2);
                
                _dbContext.Tickets.Add(ticketSupportCreated);
                _dbContext.Tickets.Add(ticketChangeRequestCreated);
                _dbContext.Tickets.Add(ticketBugCreated);
                _dbContext.Tickets.Add(ticketBugInProgress);
                _dbContext.Tickets.Add(ticketBugClosed);
                _dbContext.Tickets.Add(ticketBugCancelled);
                _dbContext.Comments.Add(commentTicket1);

                //Seed TicketCreationTypes
                var ticketCreationTypeEmail = new TicketCreationType("Email");
                var ticketCreationTypePhone = new TicketCreationType("Telefonisch");
                var ticketCreationTypeApplication = new TicketCreationType("Applicatie");
                _dbContext.TicketCreationTypes.Add(ticketCreationTypeEmail);
                _dbContext.TicketCreationTypes.Add(ticketCreationTypePhone);
                _dbContext.TicketCreationTypes.Add(ticketCreationTypeApplication);

                //Seed ContractTypes
                var contractType1 = new ContractType("All TicketCreationType Options, 24/7", true, TicketCreationTime.Altijd);
                contractType1.TicketCreationTypes.Add(ticketCreationTypeEmail);
                contractType1.TicketCreationTypes.Add(ticketCreationTypePhone);
                contractType1.TicketCreationTypes.Add(ticketCreationTypeApplication);
                var contractType2 = new ContractType("Email/Application, Weekdays", true, TicketCreationTime.Weekdagen);
                contractType2.TicketCreationTypes.Add(ticketCreationTypeEmail);
                contractType2.TicketCreationTypes.Add(ticketCreationTypeApplication);
                var contractType3 = new ContractType("Email/Phone, 24/7", true, TicketCreationTime.Altijd);
                contractType3.TicketCreationTypes.Add(ticketCreationTypeEmail);
                contractType3.TicketCreationTypes.Add(ticketCreationTypePhone);
                _dbContext.ContractTypes.Add(contractType1);
                _dbContext.ContractTypes.Add(contractType2);
                _dbContext.ContractTypes.Add(contractType3);

                //Seed Contracts
                var contract1 = new Contract(contractType1, ContractStatus.Beëindigd, new DateTime(2020, 01, 01), new DateTime(2020, 12, 31), client1);
                var contract2 = new Contract(contractType1, ContractStatus.Lopend, new DateTime(2021, 01, 01), new DateTime(2021, 12, 31), client1);
                var contract3 = new Contract(contractType1, ContractStatus.InAanvraag, new DateTime(2022, 01, 01), new DateTime(2022, 12, 31), client1);
                var contract4 = new Contract(contractType2, ContractStatus.Lopend, new DateTime(2021, 01, 01), new DateTime(2021, 12, 31), client2);
                _dbContext.Contracts.Add(contract1);
                _dbContext.Contracts.Add(contract2);
                _dbContext.Contracts.Add(contract3);
                _dbContext.Contracts.Add(contract4);

                _dbContext.SaveChanges();
            }
        }
    }
}