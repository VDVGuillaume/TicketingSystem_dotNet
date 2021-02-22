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
                _dbContext.TicketTypes.Add(new TicketType {Name = "Bug", RequiredSLA = 1});
                _dbContext.TicketTypes.Add(new TicketType {Name = "Change Request", RequiredSLA = 2});
                _dbContext.TicketTypes.Add(new TicketType {Name = "Support", RequiredSLA = 3});

                //Seed tickets
                var ticket = new Ticket("TestTitle", "TestDescription", "Support", customerUser);
                _dbContext.Tickets.Add(ticket);

                _dbContext.SaveChanges();
            }
        }
    }
}