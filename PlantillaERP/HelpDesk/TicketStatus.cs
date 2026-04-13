namespace HelpDesk
{
    public enum TicketStatus
    {
        Nuevo = 1,              // New - Just created
        Abierto = 2,            // Open - Accepted by support
        EnProgreso = 3,         // In Progress - Agent working on it
        EnEspera = 4,           // On Hold - Waiting for customer or third party
        Escalado = 5,           // Escalated - Sent to higher level
        Resuelto = 6,           // Resolved - Solution applied, awaiting user validation
        Cerrado = 7,            // Closed - User confirmed or closed by policy
        Reabierto = 8,          // Reopened - Problem recurred
        Cancelado = 9           // Cancelled - Out of scope or duplicate
    }
}
