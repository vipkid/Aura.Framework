using Aura.Framework.Analystics;
using Aura.Framework.Enumerators;
using System;
using System.Net;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Aura.Framework.Core
{
    /// <summary>
    /// Contains the functionality to check if the system meets the required soft-/hardware to run.
    /// </summary>
    public static class OSRequirements
    {
        #region Public Static Methods

        /// <summary>
        /// Returns a <see cref="bool"/> value based on whether the currently used framework is compatible with the server.
        /// </summary>
        public static bool Framework()
        {
            using (Logger logger = new Logger())
            {
                logger.Write(LoggerType.Internal, $"Checking .NET Framework...");

                // Class "ReflectionContext" exists from .NET 4.5 onwards.
                return Type.GetType("System.Reflection.ReflectionContext", false) != null;
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> value which represents the public/external ip address.
        /// </summary>
        public static async Task<string> GetIPAddress()
        {
            using (Logger logger = new Logger())
            {
                Task<string> downloader = new Task<string>(() =>
                {
                    logger.Write(LoggerType.Internal, $"Downloading IPAddress...");
                    return new WebClient().DownloadString("https://api.ipify.org");
                });
                downloader.Start();
                return await downloader;
            }
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value based on whether the service is running.
        /// </summary>
        public static bool Service(string service)
        {
            using (Logger logger = new Logger())
            {
                ServiceController controller = new ServiceController(service);
                try
                {
                    switch (controller.Status)
                    {
                        case ServiceControllerStatus.Running:
                            logger.Write(LoggerType.Internal, $"The service '{service}' is running.");
                            return true;

                        default:
                            logger.Write(LoggerType.Internal, $"The service '{service}' is not running.");
                            return false;
                    }
                }
                catch (InvalidOperationException e)
                {
                    logger.Write(LoggerType.Exception, e);
                }
                return false;
            }
        }

        #endregion Public Static Methods
    }
}