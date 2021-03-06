namespace TicketingSystem.Domain.Application
{
    public static class Constants
    {
        public const string ERROR_CONTRACT_NOT_FOUND = "Contract niet gevonden.";
        public const string ERROR_CONTRACT_STATUS_CLOSED = "Contract is reeds afgesloten.";
        public const string ERROR_TICKET_NOT_FOUND = "Ticket niet gevonden.";
        public const string ERROR_TICKET_STATUS_CANCELLED = "Ticket is reeds geannuleerd.";
        public const string ERROR_TICKET_STATUS_CLOSED = "Ticket is reeds afgesloten.";
        public const string ERROR_TICKET_TYPE_NOT_FOUND = "Ticket type niet gevonden.";
        public const string ERROR_EMPTY_COMMENT = "Er werd geen opmerking ingevuld";
        public const string ERROR_ACTIVE_CONTRACT_NOT_FOUND = "Er is geen contract actief voor deze klant.";
        public const string ERROR_ACTIVE_CONTRACT_FOUND = "Er is reeds een contract actief voor deze klant.";
        public const string ERROR_CONTRACT_FUTURE_DATE = "De geldigheidsdatum dient in de toekomst te liggen.";
    }
}
