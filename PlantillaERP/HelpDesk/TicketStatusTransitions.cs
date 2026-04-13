using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk
{
    public static class TicketStatusTransitions
    {
        /// <summary>
        /// Define valid transitions for all users (base transitions)
        /// </summary>
        private static readonly Dictionary<TicketStatus, HashSet<TicketStatus>> BaseTransitions 
            = new Dictionary<TicketStatus, HashSet<TicketStatus>>()
        {
            { TicketStatus.Nuevo, new HashSet<TicketStatus> { TicketStatus.Abierto, TicketStatus.Cancelado } },
            { TicketStatus.Abierto, new HashSet<TicketStatus> { TicketStatus.EnProgreso, TicketStatus.EnEspera, TicketStatus.Cancelado } },
            { TicketStatus.EnProgreso, new HashSet<TicketStatus> { TicketStatus.EnEspera, TicketStatus.Resuelto, TicketStatus.Escalado } },
            { TicketStatus.EnEspera, new HashSet<TicketStatus> { TicketStatus.EnProgreso, TicketStatus.Resuelto, TicketStatus.Cerrado } },
            { TicketStatus.Escalado, new HashSet<TicketStatus> { TicketStatus.EnProgreso, TicketStatus.Resuelto } },
            { TicketStatus.Resuelto, new HashSet<TicketStatus> { TicketStatus.Cerrado, TicketStatus.Reabierto } },
            { TicketStatus.Cerrado, new HashSet<TicketStatus> { TicketStatus.Reabierto } },
            { TicketStatus.Reabierto, new HashSet<TicketStatus> { TicketStatus.EnProgreso } },
            { TicketStatus.Cancelado, new HashSet<TicketStatus> { } } // Final state
        };

        /// <summary>
        /// Transiciones permitidas para Agentes
        /// </summary>
        private static readonly Dictionary<TicketStatus, HashSet<TicketStatus>> AgentTransitions 
            = new Dictionary<TicketStatus, HashSet<TicketStatus>>()
        {
            { TicketStatus.Nuevo, new HashSet<TicketStatus> { TicketStatus.Abierto, TicketStatus.Cancelado } },
            { TicketStatus.Abierto, new HashSet<TicketStatus> { TicketStatus.EnProgreso, TicketStatus.EnEspera, TicketStatus.Cancelado } },
            { TicketStatus.EnProgreso, new HashSet<TicketStatus> { TicketStatus.EnEspera, TicketStatus.Resuelto, TicketStatus.Escalado } },
            { TicketStatus.EnEspera, new HashSet<TicketStatus> { TicketStatus.EnProgreso, TicketStatus.Resuelto, TicketStatus.Cerrado } },
            { TicketStatus.Escalado, new HashSet<TicketStatus> { TicketStatus.EnProgreso, TicketStatus.Resuelto } },
            { TicketStatus.Resuelto, new HashSet<TicketStatus> { TicketStatus.Cerrado, TicketStatus.Reabierto } },
            { TicketStatus.Cerrado, new HashSet<TicketStatus> { TicketStatus.Reabierto } },
            { TicketStatus.Reabierto, new HashSet<TicketStatus> { TicketStatus.EnProgreso } },
            { TicketStatus.Cancelado, new HashSet<TicketStatus> { } }
        };

        /// <summary>
        /// Transiciones permitidas para Administradores (todas las transiciones válidas)
        /// </summary>
        private static readonly Dictionary<TicketStatus, HashSet<TicketStatus>> AdminTransitions = BaseTransitions;

        /// <summary>
        /// Transiciones permitidas para Usuarios (ninguna - solo lectura)
        /// </summary>
        private static readonly Dictionary<TicketStatus, HashSet<TicketStatus>> UserTransitions 
            = new Dictionary<TicketStatus, HashSet<TicketStatus>>()
        {
            { TicketStatus.Nuevo, new HashSet<TicketStatus> { } },
            { TicketStatus.Abierto, new HashSet<TicketStatus> { } },
            { TicketStatus.EnProgreso, new HashSet<TicketStatus> { } },
            { TicketStatus.EnEspera, new HashSet<TicketStatus> { } },
            { TicketStatus.Escalado, new HashSet<TicketStatus> { } },
            { TicketStatus.Resuelto, new HashSet<TicketStatus> { } },
            { TicketStatus.Cerrado, new HashSet<TicketStatus> { } },
            { TicketStatus.Reabierto, new HashSet<TicketStatus> { } },
            { TicketStatus.Cancelado, new HashSet<TicketStatus> { } }
        };

        /// <summary>
        /// Get valid transitions for a user based on their role
        /// </summary>
        private static Dictionary<TicketStatus, HashSet<TicketStatus>> GetTransitionsForRole(string role)
        {
            if (string.IsNullOrEmpty(role)) return UserTransitions;

            switch (role.ToLower())
            {
                case "administrador":
                    return AdminTransitions;
                case "agente":
                    return AgentTransitions;
                case "usuario":
                default:
                    return UserTransitions;
            }
        }

        /// <summary>
        /// Check if a transition is valid for a specific role
        /// </summary>
        public static bool CanTransitionTo(TicketStatus from, TicketStatus to, string userRole)
        {
            var transitions = GetTransitionsForRole(userRole);
            return transitions.ContainsKey(from) && transitions[from].Contains(to);
        }

        /// <summary>
        /// Get all valid next states for a current status and role
        /// </summary>
        public static IEnumerable<TicketStatus> GetValidNextStates(TicketStatus current, string userRole)
        {
            var transitions = GetTransitionsForRole(userRole);
            return transitions.ContainsKey(current) ? transitions[current] : Enumerable.Empty<TicketStatus>();
        }

        /// <summary>
        /// Get status display name
        /// </summary>
        public static string GetStatusDisplayName(TicketStatus status)
        {
            switch (status)
            {
                case TicketStatus.Nuevo: return "Nuevo";
                case TicketStatus.Abierto: return "Abierto";
                case TicketStatus.EnProgreso: return "En Progreso";
                case TicketStatus.EnEspera: return "En Espera";
                case TicketStatus.Escalado: return "Escalado";
                case TicketStatus.Resuelto: return "Resuelto";
                case TicketStatus.Cerrado: return "Cerrado";
                case TicketStatus.Reabierto: return "Reabierto";
                case TicketStatus.Cancelado: return "Cancelado";
                default: return status.ToString();
            }
        }

        /// <summary>
        /// Get status display name from value
        /// </summary>
        public static string GetStatusDisplayName(int statusValue)
        {
            return GetStatusDisplayName((TicketStatus)statusValue);
        }
    }
}
