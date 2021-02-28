using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TicketingSystemDataInitializer(TicketingSystemDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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
                //Seed roles
                var customerRole = new IdentityRole("Customer");
                var supportManagerRole = new IdentityRole("SupportManager");
                await _roleManager.CreateAsync(customerRole);
                await _roleManager.CreateAsync(supportManagerRole);

                //Seed users
                var customerUser = new IdentityUser { UserName = "customer", Email = "customer@gmail.be" };
                await _userManager.CreateAsync(customerUser, "P@ssword1");
                await _userManager.AddToRoleAsync(customerUser, "Customer");
                
                var supportManagerUser = new IdentityUser { UserName = "supportmanager", Email = "supportmanager@gmail.be" };
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
                var ticketSupportCreated = new Ticket("TitleSupport", "TestDescription", ticketTypeSupport, customerUser);
                var ticketChangeRequestCreated = new Ticket("TitleChangeRequest", "TestDescription", ticketTypeChangeRequest, customerUser);
                var ticketBugCreated = new Ticket("TitleBug", "TestDescription", ticketTypeBug, customerUser);

                var ticketBugInProgress = new Ticket("TitleBug2metComments", "TestDescription", ticketTypeBug, customerUser) { Status = TicketStatus.InBehandeling};
                var ticketBugClosed = new Ticket("TitleBug3", "TestDescription", ticketTypeBug, customerUser) { Status = TicketStatus.Afgehandeld };
                var ticketBugCancelled = new Ticket("TitleBug4", "TestDescription", ticketTypeBug, customerUser) { Status = TicketStatus.Geannuleerd };

                //Seed Comments 
                var commentTicket1 = new Comment {Text = "Dit is een korte comment",CreatedBy=customerUser,DateAdded= DateTime.Today};
                ticketBugInProgress.Comments.Add(commentTicket1);

                
                _dbContext.Tickets.Add(ticketSupportCreated);
                _dbContext.Tickets.Add(ticketChangeRequestCreated);
                _dbContext.Tickets.Add(ticketBugCreated);
                _dbContext.Tickets.Add(ticketBugInProgress);
                _dbContext.Tickets.Add(ticketBugClosed);
                _dbContext.Tickets.Add(ticketBugCancelled);
                _dbContext.Comments.Add(commentTicket1);

                _dbContext.SaveChanges();
            }
        }
    }
}