using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk
{
    public class TrancisionarTickets
    { 
        public static bool PuedeTransicionar(int statusActual, int statusDestino, bool esAgente)
        {
            var transicionesPermitidas = new Dictionary<(int, int), (bool usuario, bool agente)>
        {
            // (StatusOrigenId, StatusDestinoId) -> (PermitidoUsuario, PermitidoAgente)
            { (1, 2), (false, true) },   // Abierto → En Progreso
            { (2, 3), (false, true) },   // En Progreso → En Espera
            { (2, 4), (false, true) },   // En Progreso → Resuelto
            { (3, 2), (true, true) },    // En Espera → En Progreso
            { (3, 4), (false, true) },   // En Espera → Resuelto
            { (4, 5), (true, false) },   // Resuelto → Cerrado
            { (4, 6), (true, true) },    // Resuelto → Reabierto
            { (5, 6), (true, true) },    // Cerrado → Reabierto
            { (6, 2), (false, true) },   // Reabierto → En Progreso
            { (6, 3), (false, true) }    // Reabierto → En Espera
        };

            if (transicionesPermitidas.TryGetValue((statusActual, statusDestino), out var permisos))
            {
                return esAgente ? permisos.agente : permisos.usuario;
            }

            return false; // Transición no permitida
        }
    }
}