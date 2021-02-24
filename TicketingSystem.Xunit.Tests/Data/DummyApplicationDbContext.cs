using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Xunit.Tests.Data
{
   
   public class DummyApplicationDbContext
        {
            
            public IEnumerable<Ticket> Tickets { get; }
            public Ticket TicketBugInProgress { get; }
            public Ticket TicketBugClosed { get; }
            public Ticket TicketBugCancelled { get; }
            public Ticket TicketSupportCreated { get; }
            public Ticket TicketChangeRequestCreated { get; }
            public Ticket TicketBugCreated { get; }

            public TicketType TicketTypeBug { get; }
            public TicketType TicketTypeChangeRequest { get; }
            public TicketType TicketTypeSupport { get; }

            public IdentityUser CustomerUser { get; } 
            public IdentityUser SupportManagerUser { get; }

            public IdentityRole CustomerRole { get; }
            public IdentityRole SupportManagerRole { get; }

        public DummyApplicationDbContext()
            {
            CustomerRole = new IdentityRole("Customer");
            SupportManagerRole = new IdentityRole("SupportManager");
            

            //Seed users
            CustomerUser = new IdentityUser { UserName = "customer", Email = "customer@gmail.be" };          
            SupportManagerUser = new IdentityUser { UserName = "supportmanager", Email = "supportmanager@gmail.be" };
            

            //Seed ticketTypes
            TicketTypeBug = new TicketType { Name = "Bug", RequiredSLA = 1 };
            TicketTypeChangeRequest = new TicketType { Name = "Change Request", RequiredSLA = 2 };
            TicketTypeSupport = new TicketType { Name = "Support", RequiredSLA = 3 };
          

            //Seed tickets
            TicketSupportCreated = new Ticket("TitleSupport", "TestDescription", TicketTypeSupport, CustomerUser);
            TicketChangeRequestCreated = new Ticket("TitleChangeRequest", "TestDescription", TicketTypeChangeRequest, CustomerUser);
            TicketBugCreated = new Ticket("TitleBug", "TestDescription", TicketTypeBug, CustomerUser);

            TicketBugInProgress = new Ticket("TitleBug2", "TestDescription", TicketTypeBug, CustomerUser) { Status = TicketStatus.InBehandeling };
            TicketBugClosed = new Ticket("TitleBug3", "TestDescription", TicketTypeBug, CustomerUser) { Status = TicketStatus.Afgehandeld };
            TicketBugCancelled = new Ticket("TitleBug4", "TestDescription", TicketTypeBug, CustomerUser) { Status = TicketStatus.Geannuleerd };

            Tickets = new[] { TicketBugCancelled, TicketBugClosed, TicketBugInProgress, TicketBugCreated, TicketChangeRequestCreated, TicketSupportCreated };
        }
    }
    }



