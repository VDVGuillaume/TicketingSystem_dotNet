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

        public ContractType ContractTypeAll { get; }

        public Contract ContractAll { get; }

        public TicketCreationType TicketCreationTypeEmail { get; }
        public TicketCreationType TicketCreationTypePhone { get; }
        public TicketCreationType TicketCreationTypeApplication { get; }

        public Client Client1 { get; }

        public ApplicationUser CustomerUser { get; } 
        public ApplicationUser SupportManagerUser { get; }

        public IdentityRole CustomerRole { get; }
        public IdentityRole SupportManagerRole { get; }

        public DummyApplicationDbContext()
        {
            CustomerRole = new IdentityRole("Customer");
            SupportManagerRole = new IdentityRole("SupportManager");

            //Seed clients
            Client1 = new Client("Klant1");

            //Seed users
            CustomerUser = new ApplicationUser { UserName = "customer", Email = "customer@gmail.be", Client = Client1 };          
            SupportManagerUser = new ApplicationUser { UserName = "supportmanager", Email = "supportmanager@gmail.be" };

            //Seed TicketCreationTypes
            TicketCreationTypeEmail = new TicketCreationType("Email");
            TicketCreationTypePhone = new TicketCreationType("Telefonisch");
            TicketCreationTypeApplication = new TicketCreationType("Applicatie");

            //Seed contractsType
            ContractTypeAll = new ContractType("Alle creatie types, 24/7", true, TicketCreationTime.Altijd);
            ContractTypeAll.TicketCreationTypes.Add(TicketCreationTypeEmail);
            ContractTypeAll.TicketCreationTypes.Add(TicketCreationTypePhone);
            ContractTypeAll.TicketCreationTypes.Add(TicketCreationTypeApplication);

            //Seed contracts
            ContractAll = new Contract(ContractTypeAll, ContractStatus.Lopend, new DateTime(2021, 01, 01), new DateTime(2021, 12, 31), Client1);

            //Seed ticketTypes
            TicketTypeBug = new TicketType { Name = "Bug", RequiredSLA = 1 };
            TicketTypeChangeRequest = new TicketType { Name = "Change Request", RequiredSLA = 2 };
            TicketTypeSupport = new TicketType { Name = "Support", RequiredSLA = 3 };
          

            ////Seed tickets
            TicketSupportCreated = new Ticket { Title ="TitleSupport", Description="TestDescription", Type=TicketTypeSupport, Client=Client1, Contract=ContractAll };
            
            //TicketChangeRequestCreated = new Ticket("TitleChangeRequest", "TestDescription", TicketTypeChangeRequest, Client1, ContractAll);
            //TicketBugCreated = new Ticket("TitleBug", "TestDescription", TicketTypeBug, Client1, ContractAll);

            //TicketBugInProgress = new Ticket("TitleBug2", "TestDescription", TicketTypeBug, client1) { Status = TicketStatus.InBehandeling };
            //TicketBugClosed = new Ticket("TitleBug3", "TestDescription", TicketTypeBug, CustomerUser) { Status = TicketStatus.Afgehandeld };
            //TicketBugCancelled = new Ticket("TitleBug4", "TestDescription", TicketTypeBug, CustomerUser) { Status = TicketStatus.Geannuleerd };

            Tickets = new[] { TicketBugCancelled, TicketBugClosed, TicketBugInProgress, TicketBugCreated, TicketChangeRequestCreated, TicketSupportCreated };
        }
    }
}
