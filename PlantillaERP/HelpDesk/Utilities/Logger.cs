using System;
using System.IO;
using System.Linq;
using System.Web;

namespace HelpDesk.Utilities
{
    /// <summary>
    /// Sistema de logging para registrar eventos, errores y advertencias
    /// Los logs se guardan automáticamente en la carpeta ~/Logs
    /// </summary>
    public static class Logger
    {
        private static readonly string RutaLogs = HttpContext.Current?.Server.MapPath("~/Logs") ?? 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static Logger()
        {
            // Crear carpeta de Logs si no existe
            if (!Directory.Exists(RutaLogs))
            {
                Directory.CreateDirectory(RutaLogs);
            }

            // Limpiar archivos más antiguos de 30 días
            LimpiarArchivosAntiguos();
        }

        /// <summary>
        /// Limpia archivos de log más antiguos de 30 días
        /// Se ejecuta automáticamente al inicializar la clase
        /// </summary>
        private static void LimpiarArchivosAntiguos()
        {
            try
            {
                if (!Directory.Exists(RutaLogs))
                    return;

                DateTime fechaLimite = DateTime.Now.AddDays(-30);
                DirectoryInfo directorio = new DirectoryInfo(RutaLogs);

                // Obtener todos los archivos .log más antiguos de 30 días
                FileInfo[] archivosAntiguos = directorio.GetFiles("*.log")
                    .Where(f => f.LastWriteTime < fechaLimite)
                    .ToArray();

                foreach (FileInfo archivo in archivosAntiguos)
                {
                    try
                    {
                        archivo.Delete();
                        Logger.RegistrarInfo($"Archivo de log eliminado: {archivo.Name}");
                    }
                    catch (Exception ex)
                    {
                        // No interrumpir si falla la eliminación de un archivo
                        Logger.RegistrarError($"No se pudo eliminar {archivo.Name}: {ex.Message}");
                    }
                }
            }
            catch
            {
                // Fallar silenciosamente si la limpieza falla
                Logger.RegistrarError("Fallo al limpiar archivos de log antiguos");
            }
        }

        /// <summary>
        /// Registra un error en el archivo de log
        /// Incluye mensaje, tipo de excepción y stack trace
        /// </summary>
        public static void RegistrarError(string mensaje, Exception excepcion = null)
        {
            try
            {
                string nombreArchivo = $"error_{DateTime.Now:yyyy-MM-dd}.log";
                string rutaArchivo = Path.Combine(RutaLogs, nombreArchivo);

                string mensajeLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ERROR: {mensaje}";

                if (excepcion != null)
                {
                    mensajeLog += Environment.NewLine + $"Tipo de Excepción: {excepcion.GetType().Name}" + 
                                  Environment.NewLine + $"Mensaje: {excepcion.Message}" +
                                  Environment.NewLine + $"Seguimiento de Pila: {excepcion.StackTrace}";
                }

                mensajeLog += Environment.NewLine + new string('-', 80) + Environment.NewLine;

                lock (typeof(Logger))
                {
                    File.AppendAllText(rutaArchivo, mensajeLog);
                }
            }
            catch
            {
                // Fallar silenciosamente si el logging falla
                Logger.RegistrarError($"Fallo al registrar error: {mensaje}");
            }
        }

        /// <summary>
        /// Registra un mensaje informativo
        /// Se usa para operaciones exitosas
        /// </summary>
        public static void RegistrarInfo(string mensaje)
        {
            try
            {
                string nombreArchivo = $"info_{DateTime.Now:yyyy-MM-dd}.log";
                string rutaArchivo = Path.Combine(RutaLogs, nombreArchivo);

                string mensajeLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] INFO: {mensaje}" + 
                                    Environment.NewLine;

                lock (typeof(Logger))
                {
                    File.AppendAllText(rutaArchivo, mensajeLog);
                }
            }
            catch
            {
                // Fallar silenciosamente si el logging falla
               Logger.RegistrarError($"Fallo al registrar info: {mensaje}");
            }
        }

        /// <summary>
        /// Registra un mensaje de advertencia
        /// Se usa para eventos que requieren atención
        /// </summary>
        public static void RegistrarAdvertencia(string mensaje)
        {
            try
            {
                string nombreArchivo = $"advertencia_{DateTime.Now:yyyy-MM-dd}.log";
                string rutaArchivo = Path.Combine(RutaLogs, nombreArchivo);

                string mensajeLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ADVERTENCIA: {mensaje}" + 
                                    Environment.NewLine;

                lock (typeof(Logger))
                {
                    File.AppendAllText(rutaArchivo, mensajeLog);
                }
            }
            catch
            {
                // Fallar silenciosamente si el logging falla
                Logger.RegistrarError($"Fallo al registrar advertencia: {mensaje}");
            }
        }
    }
}
