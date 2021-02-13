using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Infrastructure
{
    public class TicketingSystemDataInitializer
    {
        private readonly TicketingSystemDbContext _dbContext;

        public TicketingSystemDataInitializer(TicketingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeData() 
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated()) 
            {
                _dbContext.Roles.Add(new IdentityRole("Customer"));
                _dbContext.Roles.Add(new IdentityRole("SupportManager"));

                var user = new IdentityUser();

                //_dbContext.Users.Add(new IdentityUser(""))
            }

            _dbContext.SaveChanges();
        }
    }
}