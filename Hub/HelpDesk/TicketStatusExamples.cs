using System;
using System.Web;
using HelpDesk;
using System.Linq;

namespace HelpDesk.Examples
{
    /// <summary>
    /// Ejemplos de uso del sistema de transiciones de estados de tickets
    /// </summary>
    public class TicketStatusExamples
    {
        // Ejemplo 1: Validar si agente puede cambiar de estado
        public static void Example1_ValidateTransition()
        {
            var currentStatus = TicketStatus.Abierto;
            var desiredStatus = TicketStatus.EnProgreso;
            var userRole = "agente";

            if (TicketStatusTransitions.CanTransitionTo(currentStatus, desiredStatus, userRole))
            {
                // Se puede hacer el cambio
                Console.WriteLine("✅ Transición permitida");
            }
            else
            {
                // No se puede hacer el cambio
                Console.WriteLine("❌ Transición NO permitida");
            }
        }

        // Ejemplo 2: Obtener todos los estados válidos para un agente
        public static void Example2_GetValidNextStates()
        {
            var currentStatus = TicketStatus.EnProgreso;
            var userRole = "agente";

            var validStates = TicketStatusTransitions.GetValidNextStates(currentStatus, userRole);

            Console.WriteLine($"Estados válidos desde '{currentStatus}':");
            foreach (var state in validStates)
            {
                Console.WriteLine($"  → {TicketStatusTransitions.GetStatusDisplayName(state)}");
            }
        }

        // Ejemplo 3: Mostrar diferencias entre roles
        public static void Example3_CompareRoles()
        {
            var currentStatus = TicketStatus.Resuelto;

            Console.WriteLine($"Estados válidos desde: {TicketStatusTransitions.GetStatusDisplayName(currentStatus)}\n");

            // Administrador
            Console.WriteLine("ADMINISTRADOR:");
            foreach (var state in TicketStatusTransitions.GetValidNextStates(currentStatus, "administrador"))
            {
                Console.WriteLine($"  ✓ {TicketStatusTransitions.GetStatusDisplayName(state)}");
            }

            // Agente
            Console.WriteLine("\nAGENTE:");
            foreach (var state in TicketStatusTransitions.GetValidNextStates(currentStatus, "agente"))
            {
                Console.WriteLine($"  ✓ {TicketStatusTransitions.GetStatusDisplayName(state)}");
            }

            // Usuario
            Console.WriteLine("\nUSUARIO:");
            var userStates = TicketStatusTransitions.GetValidNextStates(currentStatus, "usuario");
            if (!userStates.Any())
            {
                Console.WriteLine($"  (Sin permisos para cambiar)");
            }
        }

        // Ejemplo 4: Obtener nombre legible de estado
        public static void Example4_GetStatusNames()
        {
            for (int i = 1; i <= 9; i++)
            {
                string name = TicketStatusTransitions.GetStatusDisplayName(i);
                Console.WriteLine($"{i}: {name}");
            }
        }

        // Ejemplo 5: Uso en controlador (simil)
        public static void Example5_ControllerUsage()
        {
            int ticketId = 123;
            int newStatusValue = 3;
            HttpContext context = HttpContext.Current;
            
            // Obtener rol del usuario
            string userRole = context?.Session?["role"]?.ToString() ?? "usuario";
            
            // Obtener estado actual
            var currentStatus = (TicketStatus)newStatusValue; // En real, sería de BD
            var newStatus = (TicketStatus)newStatusValue;

            // Validar
            if (TicketStatusTransitions.CanTransitionTo(currentStatus, newStatus, userRole))
            {
                // Actualizar en BD
                Console.WriteLine($"✅ Actualizando ticket {ticketId} a {newStatus}");
                
                // Registrar en logs
                string log = $"Ticket {ticketId}: {currentStatus} → {newStatus} (Rol: {userRole})";
                Console.WriteLine($"📝 {log}");
            }
            else
            {
                Console.WriteLine($"❌ Usuario '{userRole}' no puede cambiar de {currentStatus} a {newStatus}");
            }
        }

        // Ejemplo 6: Flujo completo de un ticket
        public static void Example6_CompleteFlow()
        {
            Console.WriteLine("=== FLUJO COMPLETO DE TICKET ===\n");

            var statusFlow = new[] 
            { 
                TicketStatus.Nuevo, 
                TicketStatus.Abierto, 
                TicketStatus.EnProgreso, 
                TicketStatus.Resuelto, 
                TicketStatus.Cerrado 
            };

            string agentRole = "agente";

            for (int i = 0; i < statusFlow.Length - 1; i++)
            {
                var current = statusFlow[i];
                var next = statusFlow[i + 1];
                bool canChange = TicketStatusTransitions.CanTransitionTo(current, next, agentRole);

                Console.WriteLine($"{TicketStatusTransitions.GetStatusDisplayName(current)}" +
                                $" → {TicketStatusTransitions.GetStatusDisplayName(next)}: " +
                                $"{(canChange ? "✅" : "❌")}");
            }
        }

        // Ejemplo 7: Transiciones inválidas
        public static void Example7_InvalidTransitions()
        {
            Console.WriteLine("=== TRANSICIONES INVÁLIDAS PARA AGENTE ===\n");

            var invalidTransitions = new[]
            {
                (TicketStatus.Nuevo, TicketStatus.Cancelado),          // No puede cancelar directamente
                (TicketStatus.Abierto, TicketStatus.Cancelado),        // No puede cancelar
                (TicketStatus.Cerrado, TicketStatus.EnProgreso),       // No puede volver a progreso
                (TicketStatus.Cancelado, TicketStatus.Reabierto)       // No puede cambiar cancelado
            };

            string agentRole = "agente";

            foreach (var (from, to) in invalidTransitions)
            {
                bool canChange = TicketStatusTransitions.CanTransitionTo(from, to, agentRole);
                Console.WriteLine($"{TicketStatusTransitions.GetStatusDisplayName(from)}" +
                                $" → {TicketStatusTransitions.GetStatusDisplayName(to)}: " +
                                $"NO PERMITIDO");
            }
        }
    }
}

/*
=== NOTA IMPORTANTE PARA DESARROLLADOR ===

Para usar estas clases en tu código:

1. En un evento de cambio de estado:
   -------
   var userRole = Session["role"]?.ToString() ?? "usuario";
   var currentStatus = GetCurrentTicketStatus(ticketId); // Tu método
   var newStatus = (TicketStatus)newStatusValue;
   
   if (!TicketStatusTransitions.CanTransitionTo(currentStatus, newStatus, userRole))
   {
       ShowError("No tienes permiso para cambiar a este estado");
       return;
   }
   
   UpdateTicketStatus(ticketId, (int)newStatus);
   Logger.RegistrarInfo($"Ticket {ticketId}: {currentStatus} → {newStatus}");
   -------

2. Desde JavaScript (si estás usando PageMethods):
   -------
   PageMethods.GetValidTicketStatuses(
       currentStatusValue,
       function(result) {
           // result es una List<TicketStatusOption>
           // Rellenar dropdown con result
       }
   );
   -------

3. Para mostrar lista de estados en UI:
   -------
   var allStatuses = Enum.GetValues(typeof(TicketStatus));
   foreach (TicketStatus status in allStatuses)
   {
       string displayName = TicketStatusTransitions.GetStatusDisplayName(status);
       Console.WriteLine(displayName);
   }
   -------
*/
