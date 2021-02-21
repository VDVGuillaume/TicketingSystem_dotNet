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
                var customerRole = new IdentityRole("Customer");
                var supportManagerRole = new IdentityRole("SupportManager");
                await _roleManager.CreateAsync(customerRole);
                await _roleManager.CreateAsync(supportManagerRole);

                var customerUser = new IdentityUser { UserName = "customer", Email = "customer@gmail.be" };
                await _userManager.CreateAsync(customerUser, "P@ssword1");
                await _userManager.AddToRoleAsync(customerUser, "Customer");

                var supportManagerUser = new IdentityUser { UserName = "supportmanager", Email = "supportmanager@gmail.be" };
                await _userManager.CreateAsync(supportManagerUser, "P@ssword1");
                await _userManager.AddToRoleAsync(supportManagerUser, "SupportManager");

                var ticket = new Ticket("TestTitle", "TestDescription", "TestType", customerUser);
                _dbContext.Tickets.Add(ticket);

                _dbContext.SaveChanges();
            }
        }
    }
}