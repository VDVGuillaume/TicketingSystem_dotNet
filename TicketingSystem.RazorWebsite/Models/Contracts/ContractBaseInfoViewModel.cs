using System;

namespace TicketingSystem.RazorWebsite.Models.Contracts
{
    public class ContractBaseInfoViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Client { get; set; }

        public string StatusColor
        {
            get
            {
                switch (Status)
                {
                    case "Lopend":
                        return "green";
                    case "InAanvraag":
                        return "sandybrown";
                    case "Beëindigd":
                        return "tomato";
                    default:
                        return "darkgrey";
                }
            }
        }
    }
}
