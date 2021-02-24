namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketBaseInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        public string StatusColor {get 
            { switch (Status) 
                {
                    case "Aangemaakt":
                        return "green"; 
                    case "InBehandeling":
                        return "sandybrown";
                    case "Geannuleerd":
                        return "tomato";
                    default:
                        return "darkgrey";
                }
            }
        }
    }
}
